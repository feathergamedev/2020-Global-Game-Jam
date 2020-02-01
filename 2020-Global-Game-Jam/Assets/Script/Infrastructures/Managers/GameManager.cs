using System.Collections.Generic;
using Repair.Infrastructures.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Repair.Infrastructures.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> stagePrefabs;

        [SerializeField]
        private Transform stage;

        [SerializeField]
        private GameObject dashboardPrefab;

        [SerializeField]
        private Transform dashboard;

        private static int currentStage = 0;

        private void Awake()
        {
            LoadStage();
            LoadDashboard();

            EventEmitter.Add(GameEvent.Complete, OnComplete);
            EventEmitter.Add(GameEvent.Restart, OnRestart);

            void OnComplete(IEvent @event)
            {
                HandleOnComplete();
            }

            void OnRestart(IEvent @event)
            {
                HandleOnRestart();
            }
        }

        private void LoadStage()
        {
            currentStage = PlayerPrefs.GetInt("Scene");
            if (currentStage >= stagePrefabs.Count)
            {
                currentStage = 0;
            }

            var prefab = stagePrefabs[currentStage];
            Instantiate(prefab, stage);
        }

        private void LoadDashboard()
        {
            Instantiate(dashboardPrefab, dashboard);
        }

        private void HandleOnComplete()
        {
            if (++currentStage >= stagePrefabs.Count)
            {
                currentStage = 0;
            }

            ReloadScene();
        }

        private void HandleOnRestart()
        {
            currentStage = 0;

            ReloadScene();
        }

        private void ReloadScene()
        {
            PlayerPrefs.SetInt("Scene", currentStage);

            SceneManager.LoadScene("Main");
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                HandleOnComplete();
            }
            else if (Input.GetKey(KeyCode.B))
            {
                HandleOnRestart();
            }
        }
    }
}
