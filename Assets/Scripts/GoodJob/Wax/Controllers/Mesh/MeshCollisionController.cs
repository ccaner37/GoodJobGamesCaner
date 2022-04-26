using UnityEngine;
using GoodJob.Wax.Sticks;

namespace GoodJob.Wax.Controllers.Meshing
{
    public class MeshCollisionController : MonoBehaviour
    {
        private void OnEnable() => Stick.OnStickReleased += AddCollider;
        private void OnDisable() => Stick.OnStickReleased -= AddCollider;

        private void AddCollider()
        {
            var coll = gameObject.AddComponent<MeshCollider>();
            coll.convex = true;
            coll.isTrigger = true;

            //Vector3 v3 = transform.position + Vector3.down * 0.1f;
            //GameObject sa = Instantiate(gameObject, v3, transform.rotation);
            //var samesh = sa.GetComponent<MeshFilter>().mesh;
            //var benmesh = gameObject.GetComponent<MeshFilter>().mesh;
            //samesh.vertices = benmesh.vertices;
            //samesh.triangles = benmesh.triangles;
            //samesh.RecalculateNormals();



            Destroy(gameObject, 2f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.name == "Hairs")
            {
                Destroy(other.gameObject);
                Debug.Log("z<");
            }
        }
    }
}
