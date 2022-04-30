using GoodJob.Wax.Interactables.Waxes;
using System.Collections;
using System.Collections.Generic;
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
            StartCoroutine(SmoothBlushMaterialChange());
        }

        private IEnumerator SmoothBlushMaterialChange() 
        {
            yield return new WaitForSeconds(0.5f);

            while (true)
            {
                float alpha = _renderer.material.GetFloat("_Alpha");
                float nextAlpha = Mathf.Lerp(alpha, _nextAlpha, Time.deltaTime * _speed);
                _renderer.material.SetFloat("_Alpha", nextAlpha);

                if (alpha == _nextAlpha)
                {
                    Destroy(gameObject);
                    yield break;
                }

                yield return null;
            }
        }
    }
}
