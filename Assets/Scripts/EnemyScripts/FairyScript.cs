using UnityEngine;

public class FairyScript : MonoBehaviour
{
    private void Update()
    {
        transform.RotateAround(transform.parent.position, Vector3.forward, Time.deltaTime * 100);
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
