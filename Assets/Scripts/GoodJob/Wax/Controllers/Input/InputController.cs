using GoodJob.Wax.State.Managers;
using GoodJob.Wax.Interactables.Interfaces;
using UnityEngine;
using GoodJob.Wax.State.Enums;
using System;
using GoodJob.Wax.Interactables;
using GoodJob.Wax.Interactables.Waxes;

namespace GoodJob.Wax.Controllers.Inputs
{
    public class InputController : MonoBehaviour
    {
        private RaycastHit _raycastHit;

        private IInteractable _interactable;

        private StateManager _stateManager;

        private BendInputController _bendInput;

        public Transform SelectedItem;

        public Vector3 FirstInputPosition, TargetInput;

        [SerializeField]
        private float _stickDragSpeed;

        private bool _isAlreadySelected;
        private bool _isItemSelected => SelectedItem != null;
        private bool _inputReleased => Input.GetMouseButtonUp(0);
        private bool _inputHold => Input.GetMouseButton(0);
        private bool _inputDown => Input.GetMouseButtonDown(0);
        private bool _isInteractable => _raycastHit.transform.TryGetComponent<IInteractable>(out _interactable);

        private void Start()
        {
             _stateManager = StateManager.Instance;
            _bendInput = gameObject.GetComponent<BendInputController>();
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (_inputDown)
            {
                FirstInputPosition = Input.mousePosition;
                TargetInput = Input.mousePosition * 2;

                if (!Physics.Raycast(ray, out _raycastHit)) return;

                if (_isInteractable && !_isAlreadySelected)
                {
                    _isAlreadySelected = true;
                    SelectedItem = _raycastHit.transform;
                    _interactable.OnClick();
                }
            }

            if (_inputReleased)
                OnInputReleased();

            if (_isItemSelected && _inputHold)
                FollowInput();
        }

        private void FollowInput()
        {
            if(_stateManager.CurrentState == States.StickPhase)
                ControlStick();

            if (_stateManager.CurrentState == States.PullPhase)
                ControlHardenedWax();
        }

        private void ControlStick()
        {
            //Vector3 itemPosition = new Vector3(_raycastHit.point.x, _raycastHit.point.y + 2f, _raycastHit.point.z + 0.1f);
            //_selectedItem.position = itemPosition;
            float distanceToScreen = Camera.main.WorldToScreenPoint(SelectedItem.transform.position).z;
            Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen));
            Vector3 nextPosition = new Vector3(position.x, SelectedItem.position.y, position.z);
            SelectedItem.position = Vector3.Lerp(SelectedItem.position, nextPosition, Time.deltaTime * _stickDragSpeed);
        }

        private void ControlHardenedWax()
        {
            _bendInput.HandleBending();
        }

        private void OnInputReleased()
        {
            if (SelectedItem == null) return;
            _interactable.OnRelease(_bendInput);
            SelectedItem = null;
            _isAlreadySelected = false;
            FirstInputPosition = Vector3.zero;
        }
    }
}
