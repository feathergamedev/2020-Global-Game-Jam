using System.Collections.Generic;
using Repair.Infrastructures.Settings;

namespace Repair.Infrastructures.Events
{
    public enum GameEvent
    {
        None,
        Restart,
        Killed,
        StageClear,
        Complete,
        Action,
        GameStart,
        PlayMusic,
        PlaySound,
        NerversRotation,
        NerversDraging,
    }

    public class ListEvent : IEvent
    {
        public IEnumerable<ActionType> Types
        {
            get;
        }
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

    public class BoolEvent : IEvent
    {
        public bool Value
        {
            get;
        }

        public BoolEvent(bool value)
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

    public class MusicEvent : IEvent
    {
        public MusicType Value
        {
            get;
        }

        public MusicEvent(MusicType value)
        {
            Value = value;
        }
    }

    public class SoundEvent : IEvent
    {
        public SoundType Value
        {
            get;
        }

        public int Channel
        {
            get;
        }

        public SoundEvent(SoundType value, int channel)
        {
            Value = value;
            Channel = channel;
        }
    }
}