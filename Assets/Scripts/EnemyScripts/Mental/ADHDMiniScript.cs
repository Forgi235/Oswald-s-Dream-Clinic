using UnityEngine;
using System;

public class ADHDMiniScript : EnemyScript
{
    private Vector3 DirectionVector;
    [SerializeField] private Transform target;
    [SerializeField] private float TurningBS;
    [SerializeField] private ADHDMainScript MainEnemy;
    Rigidbody2D rb;

    public Action MiniGuyHit;

    [SerializeField] private float BaseSpeed;
    private float speed;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }
    protected override void Start()
    {
        base.Start();
        if (PlayerMovement.PM.transform != null)
        {
            target = PlayerMovement.PM.transform;
        }
        MainEnemy.EnemyDied += Die;
        speed = BaseSpeed / 2;
    }
    public override void OnRoomEnter()
    {
        base.OnRoomEnter();
        speed = BaseSpeed / 2;
    }
    protected override void EnemySetup()
    {
        base.EnemySetup();
        invincibleByAbility = true;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("PlayerAttack"))
        {
            if (!invincible)
            {
                if (IFrameCoroutine != null) StopCoroutine(IFrameCoroutine);
                if (IFrameCoroutine != InvincibilityFrames()) IFrameCoroutine = InvincibilityFrames();
                StartCoroutine(IFrameCoroutine);
                MiniGuyHit?.Invoke();
            }
        }
    }
    /*protected override void Update()
    {
        if (speed < BaseSpeed) speed += (BaseSpeed / 2) * Time.deltaTime / 2;
        DirectionVector = (target.position - transform.position).normalized * TurningBS;
        DirectionVector = new Vector3(DirectionVector.x + rb.linearVelocity.normalized.x, DirectionVector.y + rb.linearVelocity.normalized.y, DirectionVector.z).normalized;
        rb.linearVelocity = DirectionVector * speed;
    }*/
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (speed < BaseSpeed) speed += (BaseSpeed / 2) * Time.fixedDeltaTime / 2;
        DirectionVector = (target.position - transform.position).normalized * TurningBS;
        DirectionVector = new Vector3(DirectionVector.x + rb.linearVelocity.normalized.x, DirectionVector.y + rb.linearVelocity.normalized.y, DirectionVector.z).normalized;
        rb.linearVelocity = DirectionVector * speed;
    }
}
