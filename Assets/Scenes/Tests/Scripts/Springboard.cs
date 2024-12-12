using UnityEngine;

namespace Tests
{
    [RequireComponent(typeof(Collider2D))]
    public class Springboard : MonoBehaviour
    {
        [Range(400f, 1200f)]
        [SerializeField] float springForce = 800f;

        [Header("Audio")]
        [SerializeField] AudioClip springClip;

        private bool isTriggered;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            // Debug.Log($"OnTriggerEnter2D Name: {collider.name} Tag: {collider.tag}");

            if (collider.tag.Equals("Player") && !isTriggered)
            {
                if (collider.gameObject.TryGetComponent<Rigidbody2D>(out var rigidbody))
                {
                    rigidbody.AddForce(Vector2.up * springForce);
                }

                if (collider.gameObject.TryGetComponent<AudioSource>(out var audioSource))
                {
                    audioSource.PlayOneShot(springClip, 1f);
                }

                isTriggered = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            // Debug.Log($"OnTriggerExit2D Name: {collider.name} Tag: {collider.tag}");

            if (collider.tag.Equals("Player") && isTriggered)
            {
                isTriggered = false;
            }
        }
    }
}