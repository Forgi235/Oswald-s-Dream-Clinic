using UnityEngine;

public class HeartHeal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HealHP();
            disableHeart();
        }
    }
    private void HealHP()
    {
        PlayerMovement.PM.Heal(1);
    }
    private void disableHeart()
    {
        gameObject.SetActive(false);
    }
}
