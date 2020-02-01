using Repair.Infrastructures.Audios;
using Repair.Infrastructures.Core;
using Repair.Infrastructures.Events;

namespace Repair.Infrastructures.Managers
{
    public class App : Singleton<App>
    {
        public void Initialize()
        {
            // NOTE: A dummy function, use to create instance.
        }

        private void Awake()
        {
            AudioManager.I.Initialize();
            RegisterEvents();
        }

        private void OnDestroy()
        {
            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            EventEmitter.Add(GameEvent.PlayMusic, OnPlayMusic);
            EventEmitter.Add(GameEvent.PlaySound, OnPlaySound);
        }

        private void UnregisterEvents()
        {
            EventEmitter.Remove(GameEvent.PlayMusic, OnPlayMusic);
            EventEmitter.Remove(GameEvent.PlaySound, OnPlaySound);
        }

        private void OnPlayMusic(IEvent @event)
        {
            var audioEvent = @event as MusicEvent;
            AudioManager.I.PlayMusic(audioEvent.Value);
        }

        private void OnPlaySound(IEvent @event)
        {
            var audioEvent = @event as SoundEvent;
            AudioManager.I.PlaySound(audioEvent.Value, audioEvent.Channel);
        }
    }
}
