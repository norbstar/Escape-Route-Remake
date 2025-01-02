using UnityEngine;

public static class VectorExtensions
{
    public static float ToAngle(this Vector2 value)
    {
        var radians = Mathf.Atan2(value.y, value.x);
        return radians * (180f / Mathf.PI) - 90f;
    }
    
    public static float ToAngle360(this Vector2 value)
    {
        var angle = Mathf.Atan2(value.x, value.y) * Mathf.Rad2Deg;
        return (angle + 360) % 360;
    }
}
