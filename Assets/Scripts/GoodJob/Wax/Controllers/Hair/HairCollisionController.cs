using GoodJob.Wax.Interactables.Waxes;
using UnityEngine;

namespace GoodJob.Wax.Controllers.Hair
{
    public class HairCollisionController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<AbstractWax>(out AbstractWax wax))
            {
                transform.SetParent(other.transform);
            }
        }
    }
}
