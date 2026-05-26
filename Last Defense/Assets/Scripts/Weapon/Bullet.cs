using UnityEngine;

public class Bullet : MonoBehaviour
{
    // This is a special Unity physics callback function Unity automatically calls it when:
    // this object collides with another object
    private void OnCollisionEnter(Collision collision)
    {
        // Did the bullet hit an object tagged "Target"?
        if (collision.collider.CompareTag("Target"))
        {
            // Gets the object that was hit.
            print("hit " + collision.gameObject.name + " !");
            
            // The GameObject this script is attached to.
            Destroy(gameObject);
        }
    }
}