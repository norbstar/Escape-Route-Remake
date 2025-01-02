using UnityEngine;

namespace Tests
{
    public class Box : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.tag.Equals("Player")) return;
            Debug.Log($"{name} OnTriggerEnter2D Name: {collider.name} Tag: {collider.tag}");
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (!collider.tag.Equals("Player")) return;
            Debug.Log($"{name} OnTriggerExit2D Name: {collider.name} Tag: {collider.tag}");
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.collider.tag.Equals("Player")) return;
            Debug.Log($"{name} OnCollisionEnter2D Name: {collision.collider.name} Tag: {collision.collider.tag}");
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!collision.collider.tag.Equals("Player")) return;
            Debug.Log($"{name} OnCollisionExit2D Name: {collision.collider.name} Tag: {collision.collider.tag}");
        }
    }
}
