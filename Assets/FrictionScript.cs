using UnityEngine;

public class FrictionScript : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    private void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude > 0.2f) rb.linearVelocity = Vector3.zero;
        else rb.linearVelocity /= 1.2f;
    }
}
