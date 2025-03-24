using UnityEngine;
using System;

public class DamageDetection : MonoBehaviour
{
    public event Action TookDamage;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Damage")) TookDamage?.Invoke();
    }

}
