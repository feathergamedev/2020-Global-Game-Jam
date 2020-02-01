using Repair.Dashboard.Events;
using Repair.Infrastructures.Core;
using Repair.Infrastructures.Events;

namespace Repair.Dashboards.Helpers
{
    internal class RotationHelper : Singleton<RotationHelper>
    {
        private RotationStatus m_rotationStatus;
        public RotationStatus RotationStatus => m_rotationStatus;

        public const float MAX_SPEED = 350f;
        public const float MIN_SPEED = 30f;
        public const float INTERVAL_SPEED = 15f;

        public void Initialize()
        {
            EventEmitter.Remove(GameEvent.NerversRotation, Rotate);
            EventEmitter.Add(GameEvent.NerversRotation, Rotate);
        }

        private void Rotate(IEvent @event)
        {
            var value = (@event as RotationEvent).Value;
            m_rotationStatus = value;
        }
    }
}
