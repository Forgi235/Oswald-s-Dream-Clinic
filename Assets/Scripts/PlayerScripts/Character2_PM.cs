using System.Collections.Generic;
using UnityEngine;

public class Character2_PM : PlayerMovement
{
    //this character has depersonalisation disorder causing dissociation (ingame: delayed inputs)

    protected Queue<Vector2> attackQueue = new Queue<Vector2>();
    protected Queue<Vector2> moveQueue = new Queue<Vector2>();
    protected Queue<float> timeQueue = new Queue<float>();

    private float inputDelay = 0.15f;

    protected override void Awake()
    {
        /*PlayerPrefs.SetInt("ChosenCharacter", 2);
        //if the "ChosenCharacter" playerpref is not setup then dont do shit (the default stuff is in the first characters script)
        if (PlayerPrefs.GetInt("ChosenCharacter", -1) != -1)
        {
            //if this (the second) character is not chosen then disable it
            if (PlayerPrefs.GetInt("ChosenCharacter") != 2)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                PM = this;
            }
        }

        PlayerPrefs.Save();*/
        base.Awake();
        attackInstanceDuration = 0.1666f;
        AttackFrequency = 2f;
        baseMoveSpeed = 5;
    }
    protected override void GetInputs() { } //remove normal way of getting inputs to implement delayed ones

    Vector2 newMoveInput;
    Vector2 newAttackInput;

    protected override void Update()
    {
        newMoveInput = move.ReadValue<Vector2>();
        newAttackInput = attack.ReadValue<Vector2>();
        
        moveQueue.Enqueue(newMoveInput);
        attackQueue.Enqueue(newAttackInput);
        timeQueue.Enqueue(Time.time);

        processDelayedInputs();

        base.Update();
    }
    
    private void processDelayedInputs()
    {
        // GPT coocked this shit up so ask him (it gives inputs a timestamp and it uses them after the input delay is over)
        while (timeQueue.Count > 0 && Time.time - timeQueue.Peek() >= inputDelay)
        {
            moveDirection = moveQueue.Dequeue();
            attackDirrection = attackQueue.Dequeue();
            timeQueue.Dequeue();
        }
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
}
