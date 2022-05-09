using GoodJob.Wax.Utility.Tweeening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodJob.Wax.Interactables.Waxes
{
    public abstract class AbstractWax : MonoBehaviour
    {
        public static Action OnWaxPulled;

        protected Vector3 _defaultPosition;
        protected Vector3 _defaultRotation;

        protected GameObject _defaultWax;

        protected CustomTween _customTween;

        private bool _isPulled;

        private void OnEnable()
        {
            _defaultPosition = transform.localPosition;
            _defaultRotation = transform.localEulerAngles;

            _customTween = CustomTween.Instance;
        }

        public virtual void PullWax()
        {
            if (_isPulled) return;
            _isPulled = true;

            Destroy(gameObject.GetComponent<Rigidbody>());
            Destroy(gameObject.GetComponent<MeshCollider>());

            Vector3 nextScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
            CustomTween.Instance.Scale(transform, nextScale, 2);
            Destroy(gameObject, 1);

            OnWaxPulled();
        }
    }
}
