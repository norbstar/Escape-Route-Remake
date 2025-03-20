using System;

namespace Tests
{
    [Flags]
    public enum PlayerStateEnum
    {
        Idle = 0x01,
        Moving = 0x02,
        Jumping = 0x04,
        Shifting = 0x08,
        Falling = 0x10,
        Dashing = 0x20,
        Crouching = 0x40,
        Sneaking = 0x80,
        Sliding = 0x100,
        Grabbing = 0x200,
        Traversing = 0x400
    }
}