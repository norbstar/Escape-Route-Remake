using System;

namespace Tests
{
    [Flags]
    public enum PlayerStateEnum
    {
        Idle = 1,
        Running = 2,
        Jumping = 4,
        Shifting = 8,
        Falling = 16,
        Dashing = 32,
        Leaping = 64
    }
}