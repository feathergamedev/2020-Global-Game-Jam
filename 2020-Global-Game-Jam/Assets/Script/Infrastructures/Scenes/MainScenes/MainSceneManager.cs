using System.Collections.Generic;
using Repair.Infrastructures.Core;
using Repair.Infrastructures.Events;
using Repair.Infrastructures.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Repair.Infrastructures.Scenes.MainScenes
{
    public class MainSceneManager : MonoBehaviour
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
            App.I.Initialize();
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
            EventEmitter.Emit(GameEvent.PlayMusic, new MusicEvent(MusicType.Twirly_Tops));
        }

        private void OnDestroy()
        {
            UnregisterEvent();
        }

        #region Event

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

        #endregion

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
            var currentStage = ProgressHelper.I.NextStage();
            if (currentStage == 0)
            {
                ShowCredit();
            }
            else
            {
                ReloadScene();
            }
        }

        private void HandleOnRestart()
        {
            ReloadScene();
        }

        private void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void ShowCredit()
        {
            ProgressHelper.I.SetComplete(true);
            SceneManager.LoadScene(ProjectInfo.SceneInfos.Credit.BuildIndex);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                HandleOnComplete();
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                HandleOnRestart();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                ProgressHelper.I.SetStage(0);
                ReloadScene();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ProgressHelper.I.SetStage(0);
                ReloadScene();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ProgressHelper.I.SetStage(1);
                ReloadScene();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ProgressHelper.I.SetStage(2);
                ReloadScene();
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ProgressHelper.I.SetStage(3);
                ReloadScene();
            }
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                EventEmitter.Emit(GameEvent.PlaySound, new SoundEvent(SoundType.Cartoon_Boing, 0));
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                EventEmitter.Emit(GameEvent.PlayMusic, new MusicEvent(MusicType.Mute));
            }
        }
    }
}
