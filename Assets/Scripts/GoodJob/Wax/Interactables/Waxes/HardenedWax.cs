using GoodJob.Wax.Interactables.Interfaces;
using GoodJob.Wax.State.Enums;
using GoodJob.Wax.State.Managers;
using GoodJob.Wax.Utility.Tweeening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodJob.Wax.Interactables.Waxes
{
    public class HardenedWax : AbstractWax, IInteractable
    {
        public void OnClick()
        {
            StateManager.Instance.ChangeState(States.PullPhase);
        }

        public void OnRelease(ref float distance, float threshold)
        {
            if (distance < threshold)
            {
                _customTween.LocalMove(transform, _defaultPosition, 2);
                _customTween.LocalRotate(transform, _defaultRotation, 10);
            }
            distance = 0;
        }
    }
}
