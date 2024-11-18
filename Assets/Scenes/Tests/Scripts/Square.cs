using UnityEngine;

public class Square : MonoBehaviour
{
    [Range(-1f, 1f)]
    [SerializeField] float target = 0f;

    private SpriteRenderer spriteRenderer;
    private Sprite sprite;
    private Vector2[] spriteVertices;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        sprite = spriteRenderer.sprite;
        spriteVertices = sprite.vertices;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int itr = 0; itr < spriteVertices.Length; itr++)
        {
            // Debug.Log($"[{itr}] [{spriteVertices[itr].x}, {spriteVertices[itr].y}]");

            spriteVertices[itr].x = Mathf.Clamp(
                (sprite.vertices[itr].x - sprite.bounds.center.x -
                    (sprite.textureRectOffset.x / sprite.texture.width) + sprite.bounds.extents.x) /
                (2.0f * sprite.bounds.extents.x) * sprite.rect.width,
                0.0f, sprite.rect.width);

            spriteVertices[itr].y = Mathf.Clamp(
                (sprite.vertices[itr].y - sprite.bounds.center.y -
                    (sprite.textureRectOffset.y / sprite.texture.height) + sprite.bounds.extents.y) /
                (2.0f * sprite.bounds.extents.y) * sprite.rect.height,
                0.0f, sprite.rect.height);

            if (itr == 2)
            {
                if (spriteVertices[itr].x < sprite.rect.size.x - 5)
                {
                    spriteVertices[itr].x = spriteVertices[itr].x + 5;
                }
                else
                {
                    spriteVertices[itr].x = 0;
                }
            }
        }

        sprite.OverrideGeometry(spriteVertices, sprite.triangles);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
