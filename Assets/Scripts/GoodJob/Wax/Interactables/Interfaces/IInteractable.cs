using GoodJob.Wax.Controllers.Inputs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodJob.Wax.Interactables.Interfaces
{
    public interface IInteractable
    {
        public void OnClick();
        public void OnRelease(BendInputController bendInput);
    }
}
