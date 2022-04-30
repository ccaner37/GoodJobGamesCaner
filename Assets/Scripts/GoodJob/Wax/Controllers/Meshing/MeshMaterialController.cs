using UnityEngine;
using GoodJob.Wax.Interactables;
using System.Collections;

namespace GoodJob.Wax.Controllers.Meshing
{
    public class MeshMaterialController : MonoBehaviour
    {
        [SerializeField]
        private Material _hardMaterial;

        [SerializeField]
        private float _colorSpeed, _smoothnessSpeed;

        private Renderer _renderer;

        private void OnEnable() => Stick.OnStickReleased += SetHardMaterial;
        private void OnDisable() => Stick.OnStickReleased -= SetHardMaterial;

        private void Start()
        {
            _renderer = gameObject.GetComponent<MeshRenderer>();
        }

        private void SetHardMaterial()
        {
            StartCoroutine(SmoothMaterialChange());
        }

        private IEnumerator SmoothMaterialChange()
        {
            while (true)
            {
                _renderer.material.color = Color.Lerp(_renderer.material.color, _hardMaterial.color, Time.deltaTime * _colorSpeed);
                float lerpSmoothnes = Mathf.Lerp(_renderer.material.GetFloat("_Smoothness"), _hardMaterial.GetFloat("_Smoothness"), Time.deltaTime * _smoothnessSpeed);
                _renderer.material.SetFloat("_Smoothness", lerpSmoothnes);

                if (_renderer.material.color == _hardMaterial.color)
                    yield break;

                yield return null;
            }
        } 
    }
}
