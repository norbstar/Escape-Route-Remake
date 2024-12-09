using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Springboard : MonoBehaviour
{
    [Range(600f, 1800f)]
    [SerializeField] float springForce = 1500f;

    [Header("Audio")]
    [SerializeField] AudioClip springClip;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag.Equals("Player"))
        {
            collider.enabled = false;
            
            if (collider.gameObject.TryGetComponent<Rigidbody2D>(out var rigidbody))
            {
                rigidbody.AddForce(Vector2.up * springForce);
            }

            if (collider.gameObject.TryGetComponent<AudioSource>(out var audioSource))
            {
                audioSource.PlayOneShot(springClip, 1f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag.Equals("Player"))
        {
            collider.enabled = true;
        }
    }
}
