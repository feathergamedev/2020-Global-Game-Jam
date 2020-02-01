using System.Collections.Generic;

namespace Repair.Infrastructures.Events
{
    public enum GameEvent
    {
        None,
        Restart,
        Complete,
        Action,
        GameStart,

        NerversRotation,
    }

    public class IntEvent : IEvent
    {
        public int Value
        {
            get;
        }

        public IntEvent(int value)
        {
            Value = value;
        }
    }

    public class ActionEvent : IEvent
    {
        public ActionType Value
        {
            get;
        }

        public ActionEvent(ActionType value)
        {
            Value = value;
        }
    }
}