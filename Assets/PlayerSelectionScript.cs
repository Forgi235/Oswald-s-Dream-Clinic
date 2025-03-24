using UnityEngine;

public class PlayerSelectionScript : MonoBehaviour
{
    [SerializeField] private PlayerMovement[] PlayerCharacters;
    private int ChosenCharacter;

    private void Awake()
    {
        ChosenCharacter = PlayerPrefs.GetInt("ChosenCharacter", 0);
        //if this (the second) character is not chosen then disable it
        if(ChosenCharacter >= PlayerCharacters.Length) ChosenCharacter = 0;
        PlayerCharacters[ChosenCharacter].gameObject.SetActive(true);
        PlayerMovement.PM = PlayerCharacters[ChosenCharacter];
        PlayerPrefs.Save();
    }
}
