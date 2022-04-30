using GoodJob.Wax.State.Enums;
using GoodJob.Wax.State.Managers;
using GoodJob.Wax.Utility.Tweeening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodJob.Wax.Controllers.Sticks
{
    public class StickPositionController : MonoBehaviour
    {
        [SerializeField]
        private Transform _hidePoint, _showPoint;

        private CustomTween _tween;

        private void OnEnable() => StateManager.OnStateChanged += StateChanged;
        private void OnDisable() => StateManager.OnStateChanged -= StateChanged;

        private void Start() => _tween = CustomTween.Instance;

        private void StateChanged()
        {
            switch (StateManager.Instance.CurrentState)
            {
                case States.StickPhase:
                    ShowStick();
                    break;
                case States.PullPhase:
                    HideStick();
                    break;                
            }
        }

        private void ShowStick()
        {
            _tween.LocalMove(transform, _showPoint.position, 2f);
        }

        private void HideStick()
        {
            _tween.LocalMove(transform, _hidePoint.position, 2f);
        }
    }
}
