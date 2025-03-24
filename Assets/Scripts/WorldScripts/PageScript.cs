using UnityEngine;

public class PageScript : MonoBehaviour
{
    [SerializeField] private int pageIndex;
    [SerializeField] private MainUIScript MUIScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            JournalScript.JRScript.UnlockPage(pageIndex);
            disablePageFragment();
        }
    }
    private void OnEnable()
    {
        Debug.Log("AAAAA");
        disableIfCollected();
    }
    private void disableIfCollected()
    {
        if(JournalScript.JRScript == null) return;
        Debug.Log("BBBB");
        if (JournalScript.JRScript.IsPageUlnocked(pageIndex))
        {

            Debug.Log("CCCC");
            disablePageFragment();
        }
    }
    private void disablePageFragment()
    {
        gameObject.SetActive(false);
    }
}
