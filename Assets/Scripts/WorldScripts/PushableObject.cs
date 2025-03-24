using UnityEngine;

public class PushableObject : MonoBehaviour
{
    [SerializeField] private float pushForce;
    [SerializeField] private float mass;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 1000000000;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Damage"))
        {
            rb.mass = mass;
            rb.linearVelocity = rb.linearVelocity.normalized * pushForce;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Damage"))
        {
            rb.mass = 1000000000;
            rb.linearVelocity = Vector3.zero;
        }
    }
}
