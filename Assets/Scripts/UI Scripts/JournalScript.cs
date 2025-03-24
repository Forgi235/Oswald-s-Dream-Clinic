using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JournalScript : MonoBehaviour
{
    [SerializeField] private GameObject TutorialPage;
    [SerializeField] private GameObject[] Pages;

    public static JournalScript JRScript;

    private List<GameObject> UnlockedPages = new List<GameObject>();
    private bool[] UnlockedPagesIndexes;

    private int pagesIndex = 0;

    private const string PlayerPrefsKey = "UnlockedPages";

    private void Awake()
    {
        //PlayerPrefs.SetString(PlayerPrefsKey, string.Empty); // this line is a journal reset
        UnlockedPagesIndexes = new bool[Pages.Length];
        for (int i = 0; i < Pages.Length; i++)
        {
            UnlockedPagesIndexes[i] = false;
        }
        LoadUnlockedPages();
    }

    public bool IsPageUlnocked(int index)
    {
        return UnlockedPagesIndexes[index];
    }

    public void UnlockPage(int index)
    {
        if (index < 0 || index >= Pages.Length) return; // Prevent out-of-bounds access
        UnlockedPagesIndexes[index] = true;
        SaveBoolArray(UnlockedPagesIndexes, PlayerPrefsKey);
        UpdateUnlockedPageList();
    }
    void LoadUnlockedPages()
    {
        UnlockedPagesIndexes = LoadBoolArray(PlayerPrefsKey, Pages.Length);
        UpdateUnlockedPageList();
    }

    // Converts the boolean array to a list of unlocked page indices
    void UpdateUnlockedPageList()
    {
        UnlockedPages.Clear();
        for (int i = 0; i < Pages.Length; i++)
        {
            if (UnlockedPagesIndexes[i])
            {
                UnlockedPages.Add(Pages[i]);
            }
        }
    }
    void Start()
    {
        //LoadUnlockedPages();
    }
    void SaveBoolArray(bool[] boolArray, string key)
    {
        string boolString = "";

        foreach (bool b in boolArray)
        {
            boolString += b ? "1" : "0";  // Convert bool to '1' or '0'
        }
        PlayerPrefs.SetString(key, boolString);
        PlayerPrefs.Save();
    }
    bool[] LoadBoolArray(string key, int size)
    {
        string boolString = PlayerPrefs.GetString(key, new string('0', size)); //defauts to only zeros
        if(boolString == string.Empty || boolString == null)
        {
            boolString = "";
            for (int i = 0;i < size;i++)
            {
                boolString += "0";
            }
        }
        bool[] boolArray = new bool[boolString.Length];
        for (int i = 0; i < boolString.Length; i++)
        {
            boolArray[i] = boolString[i] == '1';  // Convert '1' to true, '0' to false
        }
        return boolArray;
    }

    public void NextPage(InputAction.CallbackContext context)
    {
        if (context.performed && UnlockedPages.Count > 0)
        {
            TutorialPage.SetActive(false);
            if(pagesIndex == -1) pagesIndex = 0;
            else pagesIndex++;
            if (pagesIndex >= UnlockedPages.Count) pagesIndex = 0;
            if (pagesIndex < 0) pagesIndex = UnlockedPages.Count - 1;
            foreach (GameObject page in Pages)
            {
                page.SetActive(false);
            }
            UnlockedPages[pagesIndex].SetActive(true);
        }
    }
    public void LastPage(InputAction.CallbackContext context)
    {
        if (context.performed && UnlockedPages.Count > 0)
        {
            TutorialPage.SetActive(false);
            if (pagesIndex == -1) pagesIndex = 0;
            else pagesIndex--;
            if (pagesIndex >= UnlockedPages.Count) pagesIndex = 0;
            if (pagesIndex < 0) pagesIndex = UnlockedPages.Count - 1;
            foreach (GameObject page in Pages)
            {
                page.SetActive(false);
            }
            UnlockedPages[pagesIndex].SetActive(true);
        }
    }
    public void FirstOpen()
    {
        pagesIndex = -1;
        foreach (GameObject page in Pages)
        {
            page.SetActive(false);
        }
        TutorialPage.SetActive(true);
    }
}
