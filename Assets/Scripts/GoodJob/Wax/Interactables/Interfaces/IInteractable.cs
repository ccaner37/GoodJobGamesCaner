using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodJob.Wax.Interactables.Interfaces
{
    public interface IInteractable
    {
        public void OnClick();
        public void OnRelease(ref float distance, float threshold);
    }
}
