using UnityEngine;
using GoodJob.Wax.Interactables;
using GoodJob.Wax.Interactables.Waxes;
using GoodJob.Wax.State.Managers;
using GoodJob.Wax.State.Enums;

namespace GoodJob.Wax.Controllers.Meshing
{
    public class MeshCollisionController : MonoBehaviour
    {
        private void OnEnable() => Stick.OnStickReleased += AddCollider;
        private void OnDisable() => Stick.OnStickReleased -= AddCollider;

        [SerializeField]
        private GameObject _skinBlush;

        private MeshController _meshController;

        private void Start()
        {
            _meshController = gameObject.GetComponent<MeshController>();
        }

        private void AddCollider()
        {
            if (_meshController.BuildedTriangles < _meshController.BuildedTrianglesThreshold) return;
            if (gameObject.TryGetComponent<MeshCollider>(out MeshCollider col)) return;

            var collider = gameObject.AddComponent<MeshCollider>();
            collider.convex = false;
            gameObject.AddComponent<HardenedWax>();

            StateManager.Instance.ChangeState(States.PullPhase);

            SpawnSkinBlush();
        }

        private void SpawnSkinBlush()
        {
            Vector3 blushPos = new Vector3(transform.position.x, transform.position.y * 0.1f, transform.position.z);
            GameObject blush = Instantiate(_skinBlush, blushPos, transform.rotation);
            var waxMesh = gameObject.GetComponent<MeshFilter>().mesh;
            var blushMesh = blush.GetComponent<MeshFilter>().mesh;
            blushMesh.vertices = waxMesh.vertices;
            blushMesh.triangles = waxMesh.triangles;
            blushMesh.RecalculateNormals();
        }
    }
}
