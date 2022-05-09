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
        private Transform _selectedItem;

        private RaycastHit _raycastHit;

        private IInteractable _interactable;

        private StateManager _stateManager;

        private Vector3 _firstInputPosition;
        private Vector3 _itemUpPosition;

        [SerializeField]
        private float _pullDistanceThreshold = 85f, _rotateSpeed = 88f, _positionSpeed = 0.2f, _stickDragSpeed;
        private float _pullDistance;

        private bool _isAlreadySelected;
        private bool _isItemSelected => _selectedItem != null;
        private bool _inputReleased => Input.GetMouseButtonUp(0);
        private bool _inputHold => Input.GetMouseButton(0);
        private bool _inputDown => Input.GetMouseButtonDown(0);
        private bool _isInteractable => _raycastHit.transform.TryGetComponent<IInteractable>(out _interactable);

        private void Start()
        {
             _stateManager = StateManager.Instance;
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (_inputDown)
            {
                _firstInputPosition = Input.mousePosition;
                _itemUpPosition = Input.mousePosition;

                if (!Physics.Raycast(ray, out _raycastHit)) return;

                if (_isInteractable && !_isAlreadySelected)
                {
                    _isAlreadySelected = true;
                    _selectedItem = _raycastHit.transform;
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
            float distanceToScreen = Camera.main.WorldToScreenPoint(_selectedItem.transform.position).z;
            Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen));
            Vector3 nextPosition = new Vector3(position.x, _selectedItem.position.y, position.z);
            _selectedItem.position = Vector3.Lerp(_selectedItem.position, nextPosition, Time.deltaTime * _stickDragSpeed);
        }

        private void ControlHardenedWax()
        {
            HandleRotation();
            HandlePosition();
            CheckIsWaxPulled();
        }

        private void HandleRotation()
        {
            Vector3 diff = Input.mousePosition - _firstInputPosition;
            diff.y *= 0.8f;
            diff.x *= -1.1f;
            _selectedItem.localRotation = Quaternion.RotateTowards(_selectedItem.localRotation, Quaternion.Euler(diff), Time.deltaTime * _rotateSpeed);
        }

        private void HandlePosition()
        {
            Vector3 nextPos = transform.localPosition + (-transform.forward * Vector3.Distance(Input.mousePosition, _itemUpPosition) * (Mathf.InverseLerp(-50, 50, transform.eulerAngles.z) * 0.1f));
            _selectedItem.localPosition = Vector3.Lerp(_selectedItem.localPosition, nextPos, Time.deltaTime * _positionSpeed);

            _itemUpPosition = Vector3.Lerp(_itemUpPosition, Input.mousePosition, Time.deltaTime * 2f);
        }

        private void CheckIsWaxPulled()
        {
            _pullDistance = Vector3.Distance(Input.mousePosition, _firstInputPosition);

            if (_pullDistance > _pullDistanceThreshold)
                _selectedItem.GetComponent<AbstractWax>().PullWax();
        }

        private void OnInputReleased()
        {
            if (_selectedItem == null) return;
            _interactable.OnRelease(ref _pullDistance, _pullDistanceThreshold);
            _selectedItem = null;
            _isAlreadySelected = false;
            _firstInputPosition = Vector3.zero;
        }
    }
}
