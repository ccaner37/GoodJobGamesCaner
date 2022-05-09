using GoodJob.Wax.Interactables.Waxes;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GoodJob
{
    public class SkinBlushMaterialController : MonoBehaviour
    {
        private Renderer _renderer;

        [SerializeField]
        private float _nextAlpha, _speed;

        private void OnEnable() => AbstractWax.OnWaxPulled += BlushMaterialChange;
        private void OnDisable() => AbstractWax.OnWaxPulled -= BlushMaterialChange;

        private void Start()
        {
            _renderer = gameObject.GetComponent<Renderer>();
        }

        private void BlushMaterialChange()
        {
            SmoothBlushMaterialChange();
        }

        private async void SmoothBlushMaterialChange() 
        {
            await Task.Delay(500);

            while (true)
            {
                _speed += 0.05f;
                float alpha = _renderer.material.GetFloat("_Alpha");
                float nextAlpha = Mathf.Lerp(alpha, _nextAlpha, Time.deltaTime * _speed);
                _renderer.material.SetFloat("_Alpha", nextAlpha);

                if (alpha == _nextAlpha)
                {
                    Destroy(gameObject);
                    break;
                }

                await Task.Yield();
            }
        }
    }
}
