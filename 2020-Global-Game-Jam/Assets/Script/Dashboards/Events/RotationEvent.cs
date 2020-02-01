using System;
using Repair.Infrastructures.Events;

namespace Repair.Dashboard.Events
{
    [Flags]
    public enum RotationStatus
    {
        None = 0,
        Left,
        Right,
    }

    public class RotationEvent : IEvent
    {
        public RotationStatus Value
        {
            get;
        }

        public RotationEvent(RotationStatus value)
        {
            Value = value;
        }
    }
}
