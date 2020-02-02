using System.Collections;
using Repair.Infrastructures.Core;
using Repair.Infrastructures.Events;
using Repair.Infrastructures.Scenes.MainScenes;
using Repair.Infrastructures.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Repair.Infrastructures.Scenes.CreditScenes
{
    public class CreditSceneManager : MonoBehaviour
    {
        [SerializeField]
        private Button closeButton;

        [SerializeField]
        private GameObject toBeContinue;

        private void Awake()
        {
            App.I.Initialize();

            closeButton.onClick.AddListener(OnCloseButtonClicked);
            toBeContinue.SetActive(ProgressHelper.I.GetComplete());
        }

        private void OnDestroy()
        {
            closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        }

        private void Start()
        {
            EventEmitter.Emit(GameEvent.PlayMusic, new MusicEvent(MusicType.quiet_voices_roomtone));

            if (toBeContinue.activeInHierarchy)
            {
                StartCoroutine(HideToBeContinue());
            }

            IEnumerator HideToBeContinue()
            {
                yield return new WaitForSeconds(2f);

                ProgressHelper.I.SetComplete(false);
                toBeContinue.SetActive(false);
            }
        }

        private void OnCloseButtonClicked()
        {
            SceneManager.LoadScene(ProjectInfo.SceneInfos.Home.BuildIndex);
        }
    }
}
