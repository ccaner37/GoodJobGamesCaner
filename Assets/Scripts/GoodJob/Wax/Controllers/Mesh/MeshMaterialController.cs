using UnityEngine;
using GoodJob.Wax.Sticks;

namespace GoodJob.Wax.Controllers.Meshing
{
    public class MeshMaterialController : MonoBehaviour
    {
        [SerializeField]
        private Material _hardMaterial;

        private Renderer _renderer;

        private void OnEnable() => Stick.OnStickReleased += SetHardMaterial;
        private void OnDisable() => Stick.OnStickReleased -= SetHardMaterial;

        private void Start()
        {
            _renderer = gameObject.GetComponent<MeshRenderer>();
        }

        private void SetHardMaterial()
        {
            _renderer.material = _hardMaterial;
        }
    }
}
