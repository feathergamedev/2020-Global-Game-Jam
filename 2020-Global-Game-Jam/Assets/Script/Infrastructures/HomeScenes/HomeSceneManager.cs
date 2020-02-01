using Repair.Infrastructures.Events;
using Repair.Infrastructures.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Repair.Infrastructures.Managers
{
    public class HomeSceneManager : MonoBehaviour
    {
        private void Awake()
        {
            App.I.Initialize();
        }

        private void Start()
        {
            EventEmitter.Emit(GameEvent.PlayMusic, new MusicEvent(MusicType.Slug_Love_87));
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.S))
            {
                SceneManager.LoadScene(ProjectInfo.SceneInfos.Main.BuildIndex);
            }
        }
    }
}
