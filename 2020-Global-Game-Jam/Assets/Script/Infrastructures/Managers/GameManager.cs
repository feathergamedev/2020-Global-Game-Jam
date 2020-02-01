using System.Collections.Generic;
using Repair.Infrastructures.Audios;
using Repair.Infrastructures.Events;
using Repair.Infrastructures.Managers.Helpers;
using Repair.Infrastructures.Settings;
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
            AudioManager.I.Initialize();

            var currentStage = ProgressHelper.I.GetStage();
            Debug.Log($"currentStage: {currentStage}");

            LoadStage();
            LoadDashboard();
            RegisterEvent();
        }

        private void Start()
        {
            NotifyGameStart();
            EventEmitter.Emit(GameEvent.PlayMusic, new MusicEvent(MusicType.Slug_Love_87));
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
            EventEmitter.Add(GameEvent.PlayMusic, OnPlayMusic);
            EventEmitter.Add(GameEvent.PlaySound, OnPlaySound);
        }

        private void UnregisterEvent()
        {
            EventEmitter.Remove(GameEvent.Complete, OnComplete);
            EventEmitter.Remove(GameEvent.Restart, OnRestart);
            EventEmitter.Remove(GameEvent.PlayMusic, OnPlayMusic);
            EventEmitter.Remove(GameEvent.PlaySound, OnPlaySound);
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

        private void OnPlayMusic(IEvent @event)
        {
            HandleOnPlayMusic(@event as MusicEvent);
        }

        private void OnPlaySound(IEvent @event)
        {
            HandleOnPlaySound(@event as SoundEvent);
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

            ReloadScene();
        }

        private void HandleOnRestart()
        {
            ReloadScene();
        }

        private void HandleOnPlayMusic(MusicEvent musicEvent)
        {
            AudioManager.I.PlayMusic(musicEvent.Value);
        }

        private void HandleOnPlaySound(SoundEvent soundEvent)
        {
            AudioManager.I.PlaySound(soundEvent.Value, soundEvent.Channel);
        }

        private void ReloadScene()
        {
            SceneManager.LoadScene(ProjectInfo.SceneInfos.Main.BuildIndex);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.A))
            {
                HandleOnComplete();
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                HandleOnRestart();
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                ProgressHelper.I.SetStage(0);
                ReloadScene();
            }
            else if (Input.GetKeyUp(KeyCode.F))
            {
                EventEmitter.Emit(GameEvent.PlaySound, new SoundEvent(SoundType.Cartoon_Boing, 0));
            }
            else if (Input.GetKeyUp(KeyCode.G))
            {
                EventEmitter.Emit(GameEvent.PlayMusic, new MusicEvent(MusicType.Mute));
            }


        }
    }
}
