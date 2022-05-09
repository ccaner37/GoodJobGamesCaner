using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MegaFiers;
using GoodJob.Wax.State.Managers;
using GoodJob.Wax.State.Enums;
using System.Threading.Tasks;
using GoodJob.Wax.Interactables.Waxes;

namespace GoodJob.Wax.Controllers.Inputs
{
    public class BendInputController : MonoBehaviour
    {
        private MegaBend _downBend, _upBend, _rightBend, _leftBend;

        private MegaModifyObject _modifyObject;

        [SerializeField]
        private float _bendSpeed = 90;

        [SerializeField]
        [Range(0, 1)]
        private float _waxPullThreshold; 

        private float _upBendY, _downBendY, _rightBendZ, _leftBendZ;
        private float _upLerp, _downLerp, _rightLerp, _leftLerp;

        private InputController _inputController;

        public bool IsPulledEnough;

        private void OnEnable()
        {
            StateManager.OnStateChanged += ActivateModify;

            _inputController = gameObject.GetComponent<InputController>();
        }

        private void OnDisable() => StateManager.OnStateChanged -= ActivateModify;

        public async void ActivateModify()
        {
            //if(StateManager.Instance.CurrentState == States.PullPhase)
            //{
            //    _modifyObject.enabled = true;
            //    await Task.Delay(1000);
            //    _modifyObject.AttachChildren();
            //}
        }

        public void HandleBending()
        {
            Vector3 firstInput = _inputController.FirstInputPosition;
            Vector3 targetInput = _inputController.TargetInput;
            Vector3 currentInput = Input.mousePosition;

            CalculateBending(ref _downBend.gizmoPos.y, ref _downLerp, targetInput.y, currentInput.y, firstInput.y, _downBendY, _bendSpeed);

            CalculateBending(ref _upBend.gizmoPos.y, ref _upLerp, -targetInput.y, currentInput.y, firstInput.y, _upBendY, _bendSpeed);

            CalculateBending(ref _rightBend.gizmoPos.z, ref _rightLerp, targetInput.x, currentInput.x, firstInput.x, _rightBendZ, -_bendSpeed);

            CalculateBending(ref _leftBend.gizmoPos.z, ref _leftLerp, -targetInput.x, currentInput.x, firstInput.x, _leftBendZ, -_bendSpeed);

            CheckIsWaxPulledEnough();
        }

        private void CalculateBending(ref float bendGizmoPosValue, ref float lerp, float target, float current, float first, float bendFirstPosValue, float speed)
        {
            lerp = Mathf.InverseLerp(first, target, current);
            float amount = bendFirstPosValue + (lerp * speed);
            bendGizmoPosValue = amount;
        }

        private void CheckIsWaxPulledEnough()
        {
            IsPulledEnough = _upLerp > _waxPullThreshold || _downLerp > _waxPullThreshold || _leftLerp > _waxPullThreshold || _rightLerp > _waxPullThreshold;
        }

        public void SetDefaultBend()
        {
            _upBend.gizmoPos.y = _upBendY;
            _downBend.gizmoPos.y = _downBendY;
            _rightBend.gizmoPos.z = _rightBendZ;
            _leftBend.gizmoPos.z = _leftBendZ;
        }

        public async void InstallBendingComponents(GameObject wax)
        {
            var bends = wax.GetComponents<MegaBend>();
            _modifyObject = wax.GetComponent<MegaModifyObject>();

            _modifyObject.ForceUpdate();

            _downBend = bends[0];
            _upBend = bends[1];
            _rightBend = bends[2];
            _leftBend = bends[3];

            _upBendY = _upBend.gizmoPos.y;
            _downBendY = _downBend.gizmoPos.y;
            _rightBendZ = _rightBend.gizmoPos.z;
            _leftBendZ = _leftBend.gizmoPos.z;

            _modifyObject.enabled = true;
            await Task.Delay(500);
            _modifyObject.AttachChildren();
        }

        // Example code block

        //float lerp = Mathf.InverseLerp(firstInput.y, targetInput.y, currentInput.y);
        //float amount = _downBendY + (lerp * 100);
        //_downBend.gizmoPos.y = amount;
    }
}
