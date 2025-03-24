using UnityEngine;
using Pathfinding;

public class TourettesSyndromeScript : EnemyScript
{
    [SerializeField] AIPath AIPath;
    [SerializeField] Transform ChargeTarget;
    [SerializeField] AIDestinationSetter AIDsetter;
    private float baseSpeed;

    protected override void Start()
    {
        base.Start();
        invincibleByAbility = true;
        baseSpeed = AIPath.maxSpeed;
        if (PlayerMovement.PM.transform != null)
        {
            AIDsetter.target = PlayerMovement.PM.transform;
        }
    }
    public override void OnRoomEnter()
    {
        base.OnRoomEnter();
        Normal();
    }
    private void PrepareCharge()
    {
        ChargeTarget.localPosition = (PlayerMovement.PM.transform.position - transform.position).normalized;
        AIPath.maxSpeed = 0f;
        Invoke(nameof(Charge), 0.5f);
    }
    private void Charge()
    {
        AIDsetter.target = ChargeTarget;
        AIPath.maxSpeed = 5f * baseSpeed;
        Invoke(nameof(Stun), 0.8f);
    }
    private void Stun()
    {
        invincibleByAbility = false;
        AIPath.maxSpeed = 0f;
        Invoke(nameof(Normal), 2f);
    }
    private void Normal()
    {
        AIDsetter.target = PlayerMovement.PM.transform;
        invincibleByAbility = true;
        AIPath.maxSpeed = baseSpeed;
        Invoke(nameof(PrepareCharge), 5f);
    }
}
