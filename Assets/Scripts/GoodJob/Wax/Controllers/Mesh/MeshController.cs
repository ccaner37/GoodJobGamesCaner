using System.Collections.Generic;
using UnityEngine;

namespace GoodJob.Wax.Controllers.Meshing
{
    public class MeshController : MonoBehaviour
    {
        [SerializeField]
        private Mesh _waxMesh;
        private Mesh _newMesh;

        [SerializeField]
        private Transform _stick;

        [SerializeField]
        [Range(0.01f, 0.3f)]
        private float _vertexDetectRange;

        private int[] _waxTriangles, _newTrianglesArray;

        private Vector3[] _waxVertices, _waxVerticesWorld;

        private List<List<int>> _verticesToTrianglesList;

        private void OnEnable()
        {
            CreateNewMesh();

            SetVerticesWorldPositions();

            SetVerticesToTrianglesList();
        }

        private void CreateNewMesh()
        {
            _newMesh = new Mesh
            {
                name = "Wax",
            };

            _waxVertices = _waxMesh.vertices;
            _waxVerticesWorld = _waxMesh.vertices;
            _waxTriangles = _waxMesh.triangles;

            gameObject.GetComponent<MeshFilter>().mesh = _newMesh;

            _newMesh.vertices = _waxVertices;
            _newTrianglesArray = new int[_waxTriangles.Length];
            _newMesh.triangles = _newTrianglesArray;
        }

        private void SetVerticesWorldPositions()
        {
            for (int i = 0; i < _waxVertices.Length; i++)
            {
                _waxVerticesWorld[i] = transform.TransformPoint(_waxVertices[i]);
            }
        }

        private void SetVerticesToTrianglesList()
        {
            _verticesToTrianglesList = new List<List<int>>(_newMesh.vertices.Length);

            /// Init list
            for (int i = 0; i < _newMesh.vertices.Length; i++)
            {
                _verticesToTrianglesList.Add(new List<int>());
            }

            /// Store containing triangle indices for vertices
            int stride = 3;
            for (int i = 0; i < _waxTriangles.Length; i += stride)
            {
                _verticesToTrianglesList[_waxTriangles[i + 0]].Add(i);
                _verticesToTrianglesList[_waxTriangles[i + 1]].Add(i + 1);
                _verticesToTrianglesList[_waxTriangles[i + 2]].Add(i + 2);
            }
        }

        private void Update()
        {
            BuildMesh();
        }

        private void BuildMesh()
        {
            for (int i = 0; i < _waxVerticesWorld.Length; i++)
            {
                if (!IsVertexNear(i)) continue;

                var triangleList = _verticesToTrianglesList[i];
                for (int j = 0; j < triangleList.Count; j++)
                {
                    _newTrianglesArray[triangleList[j]] = _waxTriangles[triangleList[j]];
                }
                _newMesh.triangles = _newTrianglesArray;
                _newMesh.RecalculateNormals();
            }
        }

        private bool IsVertexNear(int i)
        {
            return Vector3.Distance(_waxVerticesWorld[i], _stick.position) < _vertexDetectRange;
        }
    }
}
