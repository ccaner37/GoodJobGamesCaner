using GoodJob.Wax.Managers;
using GoodJob.Wax.Utility.Tweeening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodJob.Wax.Controllers.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _levelCompletedPanel;

        [SerializeField]
        private float _completedPanelDelay = 0.8f, _completedPanelScaleSpeed = 4f;

        private void OnEnable() => GameManager.OnLevelCompleted += EnableLevelCompletedPanel;
        private void OnDisable() => GameManager.OnLevelCompleted -= EnableLevelCompletedPanel;

        private void EnableLevelCompletedPanel() => StartCoroutine(EnableLevelCompletedPanelCoroutine());

        private IEnumerator EnableLevelCompletedPanelCoroutine()
        {
            yield return new WaitForSeconds(_completedPanelDelay);
            _levelCompletedPanel.transform.localScale = Vector3.zero;
            _levelCompletedPanel.SetActive(true);
            CustomTween.Instance.Scale(_levelCompletedPanel.transform, Vector3.one, _completedPanelScaleSpeed);
        }

        public void PlayAgainButton()
        {
            GameManager.Instance.RestartScene();
        }
    }
}
