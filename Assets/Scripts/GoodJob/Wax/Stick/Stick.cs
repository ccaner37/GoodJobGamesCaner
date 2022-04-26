using GoodJob.Wax.Sticks.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodJob.Wax.Sticks
{
    public class Stick : MonoBehaviour, IInteractable
    {
        public static Action OnStickReleased;
        public void OnRelease()
        {
            OnStickReleased();
        }
    }
}
