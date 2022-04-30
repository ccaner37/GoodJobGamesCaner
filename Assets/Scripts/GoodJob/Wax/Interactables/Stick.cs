using GoodJob.Wax.Interactables.Interfaces;
using GoodJob.Wax.State.Enums;
using GoodJob.Wax.State.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodJob.Wax.Interactables
{
    public class Stick : MonoBehaviour, IInteractable
    {
        public static Action OnStickReleased;

        public void OnClick()
        {
            StateManager.Instance.ChangeState(States.StickPhase);
        }

        public void OnRelease(ref float distance, float threshold)
        {
            OnStickReleased();
        }
    }
}
