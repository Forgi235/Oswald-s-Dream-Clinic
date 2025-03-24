using Pathfinding;
using UnityEngine;

public class FibrodysplasiaOssificansProgressivaScript : EnemyScript
{
    /*
    This and the function is named bleed because of simplicity.
    Lorewise / IRLwise it signifies the main symptom of the disease
    where upon taking ANY damage (even a scratch works) the body, instead
    of healing it with the correct tissue (skin / the organ that was damaged),
    it heals it with bone which after a while impeeds your movement and
    causes eventual death.
    */

    [SerializeField] private AIPath AIPath_;

    private float originalSpeed;
    private bool bleeding;

    protected override void Awake()
    {
        base.Awake();
        originalSpeed = AIPath_.maxSpeed;
    }
    protected override void Start()
    {
        base.Start();
        if (PlayerMovement.PM.transform != null)
        {
            GetComponent<AIDestinationSetter>().target = PlayerMovement.PM.transform;
        }
    }
    protected override void Update()
    {
        base.Update();
        if(HP <= MaxHP / 2 && !bleeding && alive)
        {
            bleeding = true;
            Invoke(nameof(Bleed), 2f);
        }
        //No you cant use else, no I will not tell you why, look at the conditions again, yes even without the "&& alive"
        if (HP > MaxHP / 2 && bleeding && alive)
        {
            bleeding = false;
            CancelInvoke(nameof(Bleed));
        }
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //at max HP the speed is original at zero it goes to half the original
        AIPath_.maxSpeed = originalSpeed * (2 * HP + MaxHP) / (MaxHP * 3);
    }
    protected override void Die()
    {
        CancelInvoke(nameof(Bleed));
        base.Die();
    }
    private void Bleed()
    {
        TakeDamage(1f);
        Invoke(nameof(Bleed), 2f);
    }
    public override void OnRoomExit()
    {
        base.OnRoomExit();
        bleeding = false;
        CancelInvoke(nameof(Bleed));
    }
    public override void OnRoomEnter()
    {
        base.OnRoomEnter();
        bleeding = false;
        CancelInvoke(nameof(Bleed));
    }
}
