using UnityEngine;

public class UnloadableObstacle : MonoBehaviour
{
    private Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = transform.position;
    }

    public void ResetPosition()
    {
        transform.position = originalPosition;
    }
}
