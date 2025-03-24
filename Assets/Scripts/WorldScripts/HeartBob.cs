using UnityEngine;

public class HeartBob : MonoBehaviour
{
    private Vector3 originalPosition;
    private float time = 0;
    [SerializeField] float devider;

    private void Awake()
    {
        originalPosition = transform.localPosition;
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        time += (Time.deltaTime / devider);
        //if (time > 2 * Mathf.PI) time %= (2 * Mathf.PI);
        transform.localPosition = new Vector3(originalPosition.x, originalPosition.y + (Mathf.Sin(time) * 0.1f), originalPosition.z) ;
    }
}
