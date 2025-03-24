using Pathfinding;
using UnityEngine;

public class BlindnessScript : EnemyScript
{
    private Vector3 DirectionVector;
    [SerializeField] private Transform target;
    Rigidbody2D rb;

    [SerializeField] private float speed;

    protected override void Start()
    {
        base.Start();
        if (PlayerMovement.PM.transform != null)
        {
            target = PlayerMovement.PM.transform;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void EnemyStatSetup()
    {
        base.EnemyStatSetup();
    }

    protected override void EnemySetup()
    {
        base.EnemySetup();
    }

    protected override void Update()
    {
        DirectionVector = (target.position - transform.position).normalized;
        rb.linearVelocity = DirectionVector * speed;
    }
}
