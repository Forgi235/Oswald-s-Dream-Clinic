using Pathfinding;
using UnityEngine;

public class MuscularDystrophyScript : EnemyScript
{
    [SerializeField] private AIDestinationSetter AIDSetter;
    [SerializeField] private AIPath AIPath_;
    
    [SerializeField] private float SineDivider;
    private float _Time = 0;
    private float originalSpeed;

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
            AIDSetter.target = PlayerMovement.PM.transform;
        }
    }
    public override void OnRoomEnter()
    {
        base.OnRoomEnter();
        if (!alive) return;
        AIDSetter.target = PlayerMovement.PM.transform;
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        AIPath_.maxSpeed = Mathf.Sin(_Time / SineDivider) * originalSpeed;
        _Time += Time.fixedDeltaTime;
        _Time %= Mathf.PI * SineDivider;
    }
}
