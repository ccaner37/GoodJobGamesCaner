using GoodJob.Wax.Sticks.Interfaces;
using UnityEngine;

namespace GoodJob.Wax.Controllers.Inputs
{
    public class InputController : MonoBehaviour
    {
        private Transform _selectedItem;

        private RaycastHit _raycastHit;

        private IInteractable _interactable;

        private bool _isAlreadySelected;
        private bool _isItemSelected => _selectedItem != null;
        private bool _inputReleased => Input.GetMouseButtonUp(0);
        private bool _inputHold => Input.GetMouseButton(0);
        private bool _inputDown => Input.GetMouseButtonDown(0);
        private bool _isInteractable => _raycastHit.transform.TryGetComponent<IInteractable>(out _interactable);

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (_inputDown)
            {

                if (!Physics.Raycast(ray, out _raycastHit)) return;

                if (_isInteractable && !_isAlreadySelected)
                {
                    _isAlreadySelected = true;
                    _selectedItem = _raycastHit.transform;
                }
            }

            if (_inputReleased)
                OnInputReleased();

            if (_isItemSelected && _inputHold)
                FollowInput();
        }

        private void FollowInput()
        {
            //Vector3 itemPosition = new Vector3(_raycastHit.point.x, _raycastHit.point.y + 2f, _raycastHit.point.z + 0.1f);
            //_selectedItem.position = itemPosition;
            float distanceToScreen = Camera.main.WorldToScreenPoint(_selectedItem.transform.position).z;
            Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen));
            _selectedItem.position = new Vector3(position.x, position.y, position.z);
        }

        private void OnInputReleased()
        {
            if (_selectedItem == null) return;
            _interactable.OnRelease();
            _selectedItem = null;
            _isAlreadySelected = false;
        }
    }
}
