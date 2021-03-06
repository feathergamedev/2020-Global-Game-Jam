﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Repair.Infrastructures.Core;
using Repair.Infrastructures.Events;
using Repair.Infrastructures.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        [SerializeField]
        private Image stageId;

        [SerializeField]
        private List<Sprite> stageSprites;

        [SerializeField]
        private MaskHelper sceneMask;

        private void Awake()
        {
            App.I.Initialize();
            ProgressHelper.I.Initialize(stagePrefabs.Count);

            sceneMask.Init(Color.black);

            var currentStage = ProgressHelper.I.GetStage();
            Debug.Log($"currentStage: {currentStage}");

            StartCoroutine(DisplayStageId());
            LoadStage();
            LoadDashboard();
            RegisterEvent();
        }

        private void Start()
        {
            sceneMask.Hide(NotifyGameStart);
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

        private IEnumerator DisplayStageId()
        {
            var currentStage = ProgressHelper.I.GetStage();

            if (currentStage >= stageSprites.Count)
            {
                Debug.LogError("Don't have stage sprite.");
                yield break;
            }

            stageId.sprite = stageSprites[currentStage];
            yield return new WaitForSeconds(2);
            DOTween.ToAlpha(() => stageId.color, (c) => stageId.color = c, 0, 1);
            yield return new WaitForSeconds(1);
            stageId.gameObject.SetActive(false);
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
            sceneMask.Show(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
        }

        private void ShowCredit()
        {
            ProgressHelper.I.SetComplete(true);
            sceneMask.Show(() => SceneManager.LoadScene(ProjectInfo.SceneInfos.Credit.BuildIndex));
        }

        private void Update()
        {
            /*
            if (Input.GetKeyDown(KeyCode.Q))
            {
                HandleOnComplete();
            }
            */

            if (Input.GetKeyDown(KeyCode.R))
            {
                HandleOnRestart();
            }

#if UNITY_EDITOR

            DetectSwitchLevel();

#endif
        }

        void DetectSwitchLevel()
        {
            var alpha1 = 49;

            for (int i=0; i<10; i++)
            {
                var levelIndex = alpha1 + i;
                if (Input.GetKeyDown((KeyCode)levelIndex))
                {
                    ProgressHelper.I.SetStage(levelIndex - 49);
                    ReloadScene();
                }
            }
        }
    }
}
