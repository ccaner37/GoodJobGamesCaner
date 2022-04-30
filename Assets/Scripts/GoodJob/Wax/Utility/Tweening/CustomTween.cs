using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GoodJob.Wax.Utility.Tweeening
{
    public class CustomTween : MonoBehaviour
    {
        public static CustomTween Instance { get; private set; }

        private void OnEnable()
        {
            Instance = this;
        }

        public void LocalMove(Transform transform, Vector3 target, float speed)
        {
            DoLocalMove(transform, target, speed);
        }

        public void LocalRotate(Transform transform, Vector3 target, float speed)
        {
            DoLocalRotate(transform, target, speed);
        }

        public void Scale(Transform transform, Vector3 target, float speed)
        {
            DoScale(transform, target, speed);
        }

        private async void DoLocalMove(Transform transform, Vector3 target, float speed)
        {
            while (true)
            {
                if (transform == null)
                    break;

                speed += 0.5f;
                transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * speed);

                if (Vector3.Distance(transform.localPosition, target) <= 0)
                    break;

                await Task.Yield();
            }
        }

        private async void DoLocalRotate(Transform transform, Vector3 target, float speed)
        {
            while (true)
            {
                if (transform == null)
                    break;

                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(target), Time.deltaTime * speed * 10);

                if (Vector3.Distance(transform.localEulerAngles, target) <= 0)
                    break;

                await Task.Yield();
            }
        }

        private async void DoScale(Transform transform, Vector3 target, float speed)
        {
            while (true)
            {
                if (transform == null)
                    break;

                speed += 0.5f;
                transform.localScale = Vector3.Lerp(transform.localScale, target, Time.deltaTime * speed);

                if (Vector3.Distance(transform.localScale, target) <= 0)
                     break;

                await Task.Yield();
            }
        }
    }
}
