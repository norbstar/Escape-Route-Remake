using UnityEngine;

public static class ExtensionMethods
{
    public class RaycastHitSet
    {
        public class Data
        {
            public bool hasHit;
            public bool unchanged;
            public Transform other;
            public Vector2 point;
            public float distance;
        }

        public bool hasHit;
        public Vector2 velocity;
        public Data left;
        public Data right;
        public Data up;
        public Data down;
    }

    public static float Remap (this float value, float from1, float to1, float from2, float to2) => (value - from1) / (to1 - from1) * (to2 - from2) + from2;

    public static bool HasHit(Vector2 origin, Vector2 direction, float distance, int layerMask, out RaycastHitSet.Data hitData)
    {
        var hit = Physics2D.Raycast(origin, direction, distance, layerMask);

        if (hit)
        {
            hitData = new RaycastHitSet.Data
            {
                hasHit = hit.collider != null,
                other = (hit.collider != null) ? hit.collider.transform : null,
                point = hit.point,
                distance = hit.distance,
            };

            return true;
        }

        hitData = null;
        return false;
    }
    
    public static bool HasBoxHit(Vector2 origin, Vector2 size, Vector2 direction, float distance, int layerMask, out RaycastHitSet.Data hitData)
    {
        var hit = Physics2D.BoxCast(origin, size, 0f, direction, distance, layerMask);

        if (hit)
        {
            hitData = new RaycastHitSet.Data
            {
                hasHit = hit.collider != null,
                other = (hit.collider != null) ? hit.collider.transform : null,
                point = hit.point,
                distance = hit.distance,
            };

            return true;
        }

        hitData = null;
        return false;
    }
}