using System;

// [Flags]
public enum PlayerStateEnum
{
    Idle = 1,
    Running = 2,
    Jumping = 4,
    Falling = 8,
    Dashing = 16,
    Leaping = 32
}
