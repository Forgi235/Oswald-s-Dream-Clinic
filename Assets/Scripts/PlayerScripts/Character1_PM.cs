using UnityEngine;

public class Character1_PM : PlayerMovement
{
    private bool slowingDown;

    protected override void Awake()
    {
        /*PlayerPrefs.SetInt("ChosenCharacter", 2);
        //if the "ChosenCharacter" playerpref is not setup do defaut stuff and set it up
        if (PlayerPrefs.GetInt("ChosenCharacter", -1) != -1)
        {
            //if this (the first) character is not chosen then disable it
            if(PlayerPrefs.GetInt("ChosenCharacter") != 1)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                PM = this;
            }
        }
        else
        {
            // this is here because this is the default character
            // dont add all this if you make a diferent character
            PM = this;
            PlayerPrefs.SetInt("ChosenCharacter", 1);
        }
        PlayerPrefs.Save();*/
        base.Awake();
        attackInstanceDuration = 0.1666f;
        AttackFrequency = 2f;
        baseMoveSpeed = 5;
}
    protected override void Attack()
    {
        GameObject AttackObject = Instantiate(AttackObjectPrefab);
        AttackObject.transform.SetParent(transform);
        AttackObject.transform.position = new Vector2(transform.position.x + attackDirrectionSave.x, transform.position.y + attackDirrectionSave.y);
        if (!isAttackingDiagonally)
        {
            AttackObject.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(new Vector2(0, -1), attackDirrectionSave));
        }
        else
        {
            AttackObject.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(new Vector2(0, -1), attackDirrectionSave) - 45);
        }
        Destroy(AttackObject, attackInstanceDuration);
    }
    protected override void Update()
    {
        base.Update();
        anxietyTime();
    }
    protected void anxietyTime()
    {
        //slowing down

        //while not moving the character begins to grow anxious making him hesitate (move slower), moving will make him more decisive again
        if (!slowingDown && moveDirection.sqrMagnitude < 0.01f) slowingDown = true;
        if (slowingDown)
        {
            if (moveSpeed > baseMoveSpeed / 3)
            {
                moveSpeed -= (baseMoveSpeed / 3) * Time.deltaTime;
            }
            else moveSpeed = baseMoveSpeed / 3;
        }

        //speeding up

        if(slowingDown && moveDirection.sqrMagnitude > 0.01f) slowingDown = false;
        if (!slowingDown)
        {
            if (moveSpeed < baseMoveSpeed)
            {
                // 3 because the lowest movespeed is a third so i need two thirds (2/3) movespeed to reach normal movespeed after 2 seconds (1/2) so thats 2/6 or 1/3
                moveSpeed += (baseMoveSpeed / 3) * Time.deltaTime;
            }
            else moveSpeed = baseMoveSpeed;
        }
    }
}
