using System;

namespace Tests
{
    [Flags]
    public enum PlayerStateEnum
    {
        Idle = 0x01,
        Running = 0x02,
        Jumping = 0x04,
        Shifting = 0x08,
        Falling = 0x10,
        Dashing = 0x20,
        Grabbing = 0x40,
        Traversing = 0x80
    }
}