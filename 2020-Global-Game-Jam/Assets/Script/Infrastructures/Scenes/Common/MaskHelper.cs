using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Repair.Infrastructures.Scenes
{
    public class MaskHelper : MonoBehaviour
    {
        [SerializeField]
        private GraphicRaycaster raycaster;

        [SerializeField]
        private Image sceneMask;

        internal void Init()
        {
            raycaster.enabled = false;
            sceneMask.gameObject.SetActive(false);
        }

        internal void Init(Color color)
        {
            raycaster.enabled = true;
            sceneMask.color = color;
            sceneMask.gameObject.SetActive(true);
        }

        internal void Show(Action callback)
        {
            StartCoroutine(DoShow(callback));
        }

        internal void Hide(Action callback = null)
        {
            StartCoroutine(DoHide(callback));
        }

        private IEnumerator DoShow(Action callback)
        {
            raycaster.enabled = true;
            var color = Color.black;
            sceneMask.color = new Color(color.r, color.g, color.b, 0);
            sceneMask.gameObject.SetActive(true);
            DOTween.ToAlpha(() => sceneMask.color, (c) => sceneMask.color = c, 1, 1);
            yield return new WaitForSeconds(1);
            callback?.Invoke();
        }

        private IEnumerator DoHide(Action callback)
        {
            var color = Color.black;
            sceneMask.color = new Color(color.r, color.g, color.b, 1);
            sceneMask.gameObject.SetActive(true);
            DOTween.ToAlpha(() => sceneMask.color, (c) => sceneMask.color = c, 0, 1);
            yield return new WaitForSeconds(1);
            callback?.Invoke();
            raycaster.enabled = false;
        }
    }
}
