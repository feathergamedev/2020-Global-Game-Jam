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

        private void Awake()
        {
            App.I.Initialize();

            playButton.onClick.AddListener(OnPlayButtonClicked);
            creditButton.onClick.AddListener(OnCreditButtonClicked);
        }

        private void OnDestroy()
        {
            playButton.onClick.RemoveListener(OnPlayButtonClicked);
            creditButton.onClick.RemoveListener(OnCreditButtonClicked);
        }

        private void Start()
        {
            EventEmitter.Emit(GameEvent.PlayMusic, new MusicEvent(MusicType.Twirly_Tops));
        }

        private void OnPlayButtonClicked()
        {
            SceneManager.LoadScene(ProjectInfo.SceneInfos.Main.BuildIndex);
        }

        private void OnCreditButtonClicked()
        {
            SceneManager.LoadScene(ProjectInfo.SceneInfos.Credit.BuildIndex);
        }
    }
}
