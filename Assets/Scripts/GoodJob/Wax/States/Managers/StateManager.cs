using GoodJob.Wax.Interactables.Waxes;
using GoodJob.Wax.State.Enums;
using System;
using UnityEngine;

namespace GoodJob.Wax.State.Managers
{
    public class StateManager : MonoBehaviour
    {
        public static StateManager Instance { get; private set; }

        public static Action OnStateChanged;

        public States CurrentState { get; private set; }

        private void OnEnable()
        {
            Instance = this;
            AbstractWax.OnWaxPulled += ChangeStateOnWaxPull;
        }

        private void OnDisable() => AbstractWax.OnWaxPulled -= ChangeStateOnWaxPull;

        public void ChangeState(States state)
        {
            if (state == CurrentState) return;

            CurrentState = state;
            OnStateChanged();
        }

        private void ChangeStateOnWaxPull()
        {
            ChangeState(States.StickPhase);
        }
    }
}
