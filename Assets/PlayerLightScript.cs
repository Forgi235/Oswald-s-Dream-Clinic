using UnityEngine;

public class PlayerLightScript : MonoBehaviour
{
    void Start()
    {
        //set the player light to the chosen player character
        transform.position = new Vector3(PlayerMovement.PM.transform.position.x, PlayerMovement.PM.transform.position.y, -2);
        transform.parent = PlayerMovement.PM.transform;
    }
}
