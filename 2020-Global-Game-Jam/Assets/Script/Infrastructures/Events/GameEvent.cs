﻿using System.Collections.Generic;

namespace Repair.Infrastructures.Events
{
    public enum GameEvent
    {
        None,
        Restart,
        Complete,
        Action,
    }

    public class ListEvent : IEvent
    {
        public IEnumerable<ActionType> Types
        {
            get;
        }

        public ListEvent(IEnumerable<ActionType> types)
        {
            Types = types;
        }
    }
}