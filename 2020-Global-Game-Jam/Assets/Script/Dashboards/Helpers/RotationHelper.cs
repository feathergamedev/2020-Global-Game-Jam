using Repair.Dashboard.Events;
using Repair.Infrastructures.Core;
using Repair.Infrastructures.Events;
using UnityEngine;

namespace Repair.Dashboards.Helpers
{
    internal class RotationHelper : Singleton<RotationHelper>
    {
        private RotationStatus m_rotationStatus;
        public RotationStatus RotationStatus => m_rotationStatus;


        private const float BASE_SPEED = 90.0f;
        public float RotationSpeed
        {
            get
            {
                return RotationStatus == RotationStatus.Left ? BASE_SPEED : -BASE_SPEED;
            }
        }

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
