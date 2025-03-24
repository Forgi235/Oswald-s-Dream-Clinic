using NUnit.Framework.Internal;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class MainUIScript : MonoBehaviour
{
    [SerializeField] private InputSystemUIInputModule EventSystem;
    [SerializeField] private GameObject JournalPanel; // Assign the UI panel in the Inspector.
    [SerializeField] private JournalScript JournalScript;

    public event Action JournalLoaded;

    private bool isUIVisible = false;

    private void Awake()
    {
        JournalPanel.SetActive(true);
        JournalScript.JRScript = JournalScript;
    }

    public void OnToggleUI(InputAction.CallbackContext context)
    {
        if (context.performed) // Ensure the toggle happens on key press.
        {
            isUIVisible = !isUIVisible; // Toggle visibility state.
            JournalPanel.SetActive(isUIVisible);

            // Optionally pause game mechanics if UI is active.
            if (isUIVisible)
            {
                Time.timeScale = 0; // Pause the game.
                JournalPanel.GetComponent<JournalScript>().FirstOpen(); 
            }
            else
            {
                Time.timeScale = 1; // Resume the game.
            }
        }
    }
    private void Start()
    {
        JournalLoaded?.Invoke();
        JournalPanel.SetActive(false);
    }
}
