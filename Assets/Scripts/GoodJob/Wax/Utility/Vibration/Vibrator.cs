using GoodJob.Wax.Interactables.Waxes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodJob.Wax.Utility.Vibration
{
    public class Vibrator : MonoBehaviour
    {
        private void OnEnable()
        {
            AbstractWax.OnWaxPulled += Vibrate;
        }

        private void Vibrate()
        {
            Handheld.Vibrate();
        }
    }
}
