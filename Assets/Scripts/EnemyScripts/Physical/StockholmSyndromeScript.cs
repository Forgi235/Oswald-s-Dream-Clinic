using UnityEngine;
using Pathfinding;

public class StockholmSyndromeScript : EnemyScript
{
    private int buffState = 0;
    private float baseMaxHP;
    private float baseSpeed;
    [SerializeField] AIPath AIpath;

    protected override void Awake()
    {
        base.Awake();
        baseMaxHP = MaxHP;
        baseSpeed = AIpath.maxSpeed;
    }
    protected override void Start()
    {
        base.Start();
        if (PlayerMovement.PM.transform != null)
        {
            GetComponent<AIDestinationSetter>().target = PlayerMovement.PM.transform;
        }
    }
    public override void OnRoomEnter()
    {
        base.OnRoomEnter();
        ResetStats();
        buffState = 0;
        CancelInvoke(nameof(Buff));
        Invoke(nameof(Buff), 8f);
    }
    private void Buff()
    {
        if (buffState == 0) Buff_1();
        else if (buffState == 1) Buff_2();
        else if (buffState == 2) Buff_3();
        buffState++;
        if(buffState == 3) return;
        Invoke(nameof(Buff), 8f);
    }
    protected override void TakeDamage(float damage)
    {
        CancelInvoke(nameof(Buff));
        Invoke(nameof(Buff), 8f);
        base.TakeDamage(damage);
    }
    private void ResetStats()
    {
        MaxHP = baseMaxHP;
        AIpath.maxSpeed = baseSpeed;
        HP = MaxHP;
    }
    private void Buff_1()
    {
        AIpath.maxSpeed = baseSpeed * 1.333f;
        MaxHP += Mathf.RoundToInt(baseMaxHP * 0.6f);
        HP += baseMaxHP;
        if (HP > MaxHP) HP = MaxHP;
    }
    private void Buff_2()
    {
        AIpath.maxSpeed = baseSpeed * 1.666f;
        MaxHP += Mathf.RoundToInt(baseMaxHP * 0.6f);
        HP += baseMaxHP;
        if (HP > MaxHP) HP = MaxHP;
    }
    private void Buff_3()
    {
        AIpath.maxSpeed = baseSpeed * 2f;
        MaxHP += Mathf.RoundToInt(baseMaxHP * 0.6f);
        HP += baseMaxHP;
        if (HP > MaxHP) HP = MaxHP;
    }
    public override void OnRoomExit()
    {
        CancelInvoke(nameof(Buff));
        buffState = 0;
        ResetStats();
    }
}
