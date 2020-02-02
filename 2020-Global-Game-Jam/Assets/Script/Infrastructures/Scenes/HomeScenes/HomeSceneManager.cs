using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Repair.Infrastructures.Core;
using Repair.Infrastructures.Events;
using Repair.Infrastructures.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Repair.Infrastructures.Scenes.HomeScenes
{
    public class HomeSceneManager : MonoBehaviour
    {
        [SerializeField]
        private Button playButton;

        [SerializeField]
        private Button creditButton;

        [SerializeField]
        private Button skipButton;

        [SerializeField]
        private GameObject storyRoot;

        [SerializeField]
        private List<Image> stories = new List<Image>();

        [SerializeField]
        private Image mask;

        [SerializeField]
        private MaskHelper sceneMask;

        private void Awake()
        {
            App.I.Initialize();

            skipButton.onClick.AddListener(OnSkipButtonClicked);
            playButton.onClick.AddListener(OnPlayButtonClicked);
            creditButton.onClick.AddListener(OnCreditButtonClicked);

            mask.gameObject.SetActive(true);

            storyRoot.SetActive(false);
            foreach (var story in stories)
            {
                story.gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            skipButton.onClick.AddListener(OnSkipButtonClicked);
            playButton.onClick.RemoveListener(OnPlayButtonClicked);
            creditButton.onClick.RemoveListener(OnCreditButtonClicked);
        }

        private void Start()
        {
            EventEmitter.Emit(GameEvent.PlayMusic, new MusicEvent(MusicType.Mute));

            if (!ProgressHelper.I.GetSawStory())
            {
                sceneMask.Init();
                ProgressHelper.I.SetSawStory(true);
                StartCoroutine(ShowStory());
            }
            else
            {
                sceneMask.Init();
                StartCoroutine(ShowTitle());
            }

            IEnumerator ShowMask(Color maskColor, float duration = 1f)
            {
                var color = maskColor;
                mask.color = new Color(color.r, color.g, color.b, 0);
                mask.gameObject.SetActive(true);
                DOTween.ToAlpha(() => mask.color, (c) => mask.color = c, 1, duration);
                yield return new WaitForSeconds(duration);
            }

            IEnumerator HideMask(float duration = 1f)
            {
                var color = mask.color;
                mask.color = new Color(color.r, color.g, color.b, 1);
                DOTween.ToAlpha(() => mask.color, (c) => mask.color = c, 0, duration);
                yield return new WaitForSeconds(duration);
                mask.gameObject.SetActive(false);
            }

            IEnumerator ShowTitle()
            {
                yield return HideMask();
                OnTitleShow();
            }

            IEnumerator ShowStory()
            {
                EventEmitter.Emit(GameEvent.PlayMusic, new MusicEvent(MusicType.Story_BGM));

                storyRoot.SetActive(true);
                foreach (var story in stories)
                {
                    story.gameObject.SetActive(true);
                    yield return HideMask();
                    yield return new WaitForSeconds(2f);
                    if (story != stories.Last())
                    {
                        yield return ShowMask(Color.black);
                        story.gameObject.SetActive(false);
                    }
                    else
                    {
                        EventEmitter.Emit(GameEvent.PlaySound, new SoundEvent(SoundType.Car_Hit, 3));
                        skipButton.gameObject.SetActive(false);
                        yield return ShowMask(Color.white, 4f);
                        story.gameObject.SetActive(false);
                    }
                }

                EventEmitter.Emit(GameEvent.PlayMusic, new MusicEvent(MusicType.Mute));
                storyRoot.SetActive(false);
                yield return HideMask();
                OnTitleShow();
            }
        }

        private void OnTitleShow()
        {
            EventEmitter.Emit(GameEvent.PlayMusic, new MusicEvent(MusicType.Twirly_Tops));
        }

        private void OnPlayButtonClicked()
        {
            sceneMask.Show(() => SceneManager.LoadScene(ProjectInfo.SceneInfos.Main.BuildIndex));
        }

        private void OnCreditButtonClicked()
        {
            sceneMask.Show(() => SceneManager.LoadScene(ProjectInfo.SceneInfos.Credit.BuildIndex));
        }

        private void OnSkipButtonClicked()
        {
            StopAllCoroutines();
            skipButton.onClick.AddListener(OnSkipButtonClicked);

            mask.gameObject.SetActive(false);
            storyRoot.SetActive(false);
            foreach (var story in stories)
            {
                story.gameObject.SetActive(false);
            }

            OnTitleShow();
        }
    }
}
