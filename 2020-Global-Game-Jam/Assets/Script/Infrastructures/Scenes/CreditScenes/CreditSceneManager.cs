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

        [SerializeField]
        private MaskHelper sceneMask;

        private void Awake()
        {
            App.I.Initialize();

            closeButton.onClick.AddListener(OnCloseButtonClicked);
            toBeContinue.SetActive(ProgressHelper.I.GetComplete());

            if (toBeContinue.activeInHierarchy)
            {
                sceneMask.Init();
            }
            else
            {
                sceneMask.Init(Color.black);
            }
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
            else
            {
                sceneMask.Hide();
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
            sceneMask.Show(() => SceneManager.LoadScene(ProjectInfo.SceneInfos.Home.BuildIndex));
        }
    }
}
