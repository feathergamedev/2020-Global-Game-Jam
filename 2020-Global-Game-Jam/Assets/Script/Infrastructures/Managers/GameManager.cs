using System.Collections.Generic;
using Repair.Infrastructures.Events;
using Repair.Infrastructures.Managers.Helpers;
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
        private List<GameObject> dashboardPrefabs;

        [SerializeField]
        private Transform dashboard;

        private void Awake()
        {
            ProgressHelper.I.Initialize(stagePrefabs.Count);

            var currentStage = ProgressHelper.I.GetStage();
            Debug.Log($"currentStage: {currentStage}");

            LoadStage();
            LoadDashboard();
            RegisterEvent();
        }

        private void Start()
        {
            NotifyGameStart();
        }

        private void OnDestroy()
        {
            UnregisterEvent();
        }

        private void RegisterEvent()
        {
            EventEmitter.Add(GameEvent.Complete, OnComplete);
            EventEmitter.Add(GameEvent.Restart, OnRestart);
        }

        private void UnregisterEvent()
        {
            EventEmitter.Remove(GameEvent.Complete, OnComplete);
            EventEmitter.Remove(GameEvent.Restart, OnRestart);
        }

        private void NotifyGameStart()
        {
            EventEmitter.Emit(GameEvent.GameStart, null);
        }

        private void OnComplete(IEvent @event)
        {
            HandleOnComplete();
        }

        private void OnRestart(IEvent @event)
        {
            HandleOnRestart();
        }

        private void LoadStage()
        {
            var currentStage = ProgressHelper.I.GetStage();
            var prefab = stagePrefabs[currentStage];
            Instantiate(prefab, stage);
        }

        private void LoadDashboard()
        {
            var currentStage = ProgressHelper.I.GetStage();
            var prefab = dashboardPrefabs[currentStage];
            Instantiate(prefab, dashboard);
        }

        private void HandleOnComplete()
        {
            Debug.Log($"HandleOnComplete");
            var currentStage = ProgressHelper.I.NextStage();
            Debug.Log($"currentStage: {currentStage}");

            ReloadScene();
        }

        private void HandleOnRestart()
        {
            Debug.Log($"HandleOnRestart");
            ReloadScene();
        }

        private void ReloadScene()
        {
            SceneManager.LoadScene("Main");
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                HandleOnComplete();
            }
            else if (Input.GetKey(KeyCode.S))
            {
                HandleOnRestart();
            }
            else if (Input.GetKey(KeyCode.D))
            {
                ProgressHelper.I.SetStage(0);
                ReloadScene();
            }
        }
    }
}
