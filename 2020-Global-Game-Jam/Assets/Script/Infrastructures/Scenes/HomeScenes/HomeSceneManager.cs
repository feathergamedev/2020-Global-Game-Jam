using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Repair.Infrastructures.Core;
using Repair.Infrastructures.Events;
using Repair.Infrastructures.Scenes.MainScenes;
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
            EventEmitter.Emit(GameEvent.PlayMusic, new MusicEvent(MusicType.Twirly_Tops));

            if (!ProgressHelper.I.GetSawStory())
            {
                ProgressHelper.I.SetSawStory(true);
                StartCoroutine(ShowStory());
            }
            else
            {
                StartCoroutine(HideMask());
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

            IEnumerator ShowStory()
            {
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
                        yield return ShowMask(Color.white);
                        story.gameObject.SetActive(false);
                    }
                }

                storyRoot.SetActive(false);
                yield return HideMask(3f);
            }
        }

        private void OnPlayButtonClicked()
        {
            SceneManager.LoadScene(ProjectInfo.SceneInfos.Main.BuildIndex);
        }

        private void OnCreditButtonClicked()
        {
            SceneManager.LoadScene(ProjectInfo.SceneInfos.Credit.BuildIndex);
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
        }
    }
}
