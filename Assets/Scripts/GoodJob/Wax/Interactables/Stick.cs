using GoodJob.Wax.Interactables.Interfaces;
using GoodJob.Wax.State.Enums;
using GoodJob.Wax.State.Managers;
using GoodJob.Wax.Utility.Tweeening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodJob.Wax.Interactables
{
    public class Stick : MonoBehaviour, IInteractable
    {
        public static Action OnStickReleased;

        private bool _isCloseToWax;

        private void OnEnable() => StateManager.OnStateChanged += ControlBooleanWithState;
        private void OnDisable() => StateManager.OnStateChanged -= ControlBooleanWithState;

        private void ControlBooleanWithState()
        {
            if (StateManager.Instance.CurrentState == States.StickPhase) 
                _isCloseToWax = false;
        }

        public void OnClick()
        {
            if (_isCloseToWax) return;
            _isCloseToWax = true;
            Vector3 pos = new Vector3(transform.position.x, transform.position.y * 0.4f, transform.position.z);
            CustomTween.Instance.Move(transform, pos, 0.1f);
        }

        public void OnRelease(ref float distance, float threshold)
        {
            OnStickReleased();
        }
    }
}
