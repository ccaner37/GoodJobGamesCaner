using GoodJob.Wax.Interactables.Waxes;
using GoodJob.Wax.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodJob.Wax.Controllers.Hair
{
    public class HairCountController : MonoBehaviour
    {
        private void OnEnable() => AbstractWax.OnWaxPulled += CheckHairCount;
        private void OnDisable() => AbstractWax.OnWaxPulled -= CheckHairCount;

        private void CheckHairCount()
        {
            if (transform.childCount == 0)
                GameManager.Instance.LevelCompleted();
        }
    }
}
