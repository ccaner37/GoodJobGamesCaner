using GoodJob.Wax.Controllers.Inputs;
using GoodJob.Wax.Interactables.Interfaces;
using GoodJob.Wax.State.Enums;
using GoodJob.Wax.State.Managers;

namespace GoodJob.Wax.Interactables.Waxes
{
    public class HardenedWax : AbstractWax, IInteractable
    {
        public void OnClick()
        {
            StateManager.Instance.ChangeState(States.PullPhase);
        }

        public void OnRelease(BendInputController bendInput)
        {
            if (bendInput.IsPulledEnough)
            {
                PullWax();
            }
            else
            {
                bendInput.SetDefaultBend();
            }
        }
    }
}
