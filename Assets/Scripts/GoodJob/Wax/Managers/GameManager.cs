using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using GoodJob.Wax.Controllers.Inputs;

namespace GoodJob.Wax.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public static Action OnLevelCompleted;

        public BendInputController BendInput;

        private void OnEnable() => Instance = this;

        public void LevelCompleted()
        {
            OnLevelCompleted();
        }

        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
