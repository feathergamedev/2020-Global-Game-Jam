using System;

namespace Repair.Infrastructures.Events
{
    [Flags]
    public enum ActionType
    {
        None = 0,
        Left = 1,
        Right = 2,
//        Jump = 4,
        NewJump = 4,
        Sprint = 8,
        Hit = 16,
    }
}
