using Repair.Infrastructures.Events;
using UnityEngine;

namespace Repair.Infrastructures.Managers
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            EventEmitter.Add(GameEvent.Complete, OnComplete);
            EventEmitter.Add(GameEvent.Restart, OnRestart);

            void OnComplete(IEvent @event)
            {
                HandleOnComplete();
            }

            void OnRestart(IEvent @event)
            {
                HandleOnRestart();
            }
        }

        private void HandleOnComplete()
        {
        }

        private void HandleOnRestart()
        {
        }
    }
}
