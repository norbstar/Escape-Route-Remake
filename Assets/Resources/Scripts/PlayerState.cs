using System;

// [Flags]
public enum PlayerState
{
    Idle = 1,
    Running = 2,
    Jumping = 4,
    InAir = 8,
    Falling = 16,
    Dashing = 32
}
