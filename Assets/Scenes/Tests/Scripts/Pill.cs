using UnityEngine;

namespace Tests
{
    [RequireComponent(typeof(AudioSource))]
    public class Pill : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Animator animator;

        [Header("Audio")]
        [SerializeField] AudioClip consumeClip;

        private AudioSource audioSource;

        void Awake() => audioSource = GetComponent<AudioSource>();

        private bool isTriggered;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            // Debug.Log($"OnTriggerEnter2D Name: {collider.name} Tag: {collider.tag}");
            
            if (collider.tag.Equals("Player") && !isTriggered)
            {
                audioSource.PlayOneShot(consumeClip, 1f);
                animator.SetTrigger("Consume");
                Destroy(gameObject, 1f);

                isTriggered = true;
            }
        }
    }
}