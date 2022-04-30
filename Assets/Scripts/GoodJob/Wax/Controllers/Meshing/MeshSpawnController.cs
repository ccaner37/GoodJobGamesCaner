using GoodJob.Wax.Interactables.Waxes;
using UnityEngine;

namespace GoodJob.Wax.Controllers.Meshing
{
    public class MeshSpawnController : MonoBehaviour
    {
        [SerializeField]
        private Transform _wax;

        private Vector3 _defaultPosition, _defaultRotation;

        private void OnEnable() => AbstractWax.OnWaxPulled += ActivateNewWax;

        private void OnDisable() => AbstractWax.OnWaxPulled -= ActivateNewWax;

        private void Start()
        {
            _defaultPosition = _wax.localPosition;
            _defaultRotation = _wax.localEulerAngles;

            CreateNewWax();
        }

        private void CreateNewWax()
        {
            _wax = Instantiate(_wax, transform);
            _wax.transform.localPosition = _defaultPosition;
            _wax.transform.localEulerAngles = _defaultRotation;
            _wax.gameObject.SetActive(false);
        }

        private void ActivateNewWax()
        {
            _wax.gameObject.SetActive(true);
            CreateNewWax();
        }
    }
}
