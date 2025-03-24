using Pathfinding;
using System.Collections;
using UnityEngine;

public class BorderlinePersonalityDisorderScript : EnemyScript
{
    [SerializeField] private AIDestinationSetter AIDSetter;
    [SerializeField] private AIPath AIPath_;

    [SerializeField] private GameObject EnemyFolder;
    [SerializeField] private Transform SadFormTarget;

    [SerializeField] private float AngryFormSpeed;
    [SerializeField] private float SadFormSpeed;
    [SerializeField] private float AbbandonedFormSpeed;

    [SerializeField] private SpriteRenderer[] spriteRenderers;

    private Vector3 VectorDirection;

    private int a;
    private int a2;
    private int FormSwitchCounter = 0;
    private float b;

    private float shortestDistance = 0;
    private Transform shortestDistanceTransform;

    protected override void EnemySetup()
    {
        base.EnemySetup();
        invincibleByAbility = true;
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void Start()
    {
        FormSwitch();
        FormSwitchCounter = 0;
        if (EnemyFolder == null) Debug.LogError("Enemy folder on BPD enemy");
    }
    private void FormSwitch()
    {
        CancelInvoke(nameof(FormSwitch));
        invincibleByAbility = true;
        if (FormSwitchCounter >= 7)
        {
            BecomeVulnerable();
            return;
        }
        a2 = a;
        while (a2 == a)
        {
            a2 = Random.Range(0, 3);
        }
        a = a2;
        if (a == 0)
        {
            AngryForm();
        }
        else if (a == 1)
        {
            SadForm();
        }
        else
        {
            AbbandonedForm();
        }
        FormSwitchCounter++;
        Invoke(nameof(FormSwitch), 2f);
    }
    private void AngryForm()
    {
        AIPath_.maxSpeed = AngryFormSpeed;
        AIDSetter.target = PlayerMovement.PM.transform;
    }
    private void SadForm()
    {
        SadFormTarget.RotateAround(transform.position ,new Vector3(0,0,1), Random.Range(0, 360));
        AIPath_.maxSpeed = SadFormSpeed;
        AIDSetter.target = SadFormTarget;
    }
    private void AbbandonedForm()
    {
        AIPath_.maxSpeed = AbbandonedFormSpeed;
        if (EnemyFolder.GetComponentsInChildren<EnemyScript>().Length <= 1)
        {
            shortestDistance = Vector3.Distance(this.transform.position, PlayerMovement.PM.transform.position);
            shortestDistanceTransform = PlayerMovement.PM.transform;
        }
        else
        {
            shortestDistance = 1000000000;
        }
        foreach (EnemyScript ES in EnemyFolder.GetComponentsInChildren<EnemyScript>())
        {
            if (ES != null && ES != this)
            {
                b = Vector3.Distance(this.transform.position, ES.transform.position);
                if (b < shortestDistance)
                {
                    shortestDistance = b;
                    shortestDistanceTransform = ES.transform;
                }
            }
        }
        AIDSetter.target = shortestDistanceTransform;
    }
    private void BecomeVulnerable()
    {
        invincibleByAbility = false;
        AIPath_.maxSpeed = 0;
        FormSwitchCounter = 0;
        Invoke(nameof(FormSwitch), 2.5f);
    }
    public override void OnRoomEnter()
    {
        base.OnRoomEnter();
        FormSwitch();
        FormSwitchCounter = 0;
        invincibleByAbility = true;
    }
    protected override IEnumerator DamageIndicator()
    {
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.color = Color.red;
        }
        yield return new WaitForSeconds(0.2f);
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.color = Color.white;
        }
    }
}
