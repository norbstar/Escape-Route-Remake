using UnityEngine;

namespace Tests
{
    [RequireComponent(typeof(Collider2D))]
    public class Grip : MonoBehaviour
    {
        private bool isTriggered;

        private void OnTriggerEnter2D(Collider2D collider)
        {           
            if (collider.tag.Equals("Player") && !isTriggered)
            {
                if (collider.gameObject.TryGetComponent<PlayerEssentials>(out var essentials))
                {
                    essentials.Gripping(true);
                }

                isTriggered = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.tag.Equals("Player") && isTriggered)
            {
                if (collider.gameObject.TryGetComponent<PlayerEssentials>(out var essentials))
                {
                    essentials.Gripping(false);
                }

                isTriggered = false;
            }
        }
    }
}
