using UnityEngine;
using System;

public class SchizophreniaGhostScript : EnemyScript
{
    private Vector3 DirectionVector;
    [SerializeField] private SchizophreniaMainScript MainScript;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] private Transform target;
    Rigidbody2D rb;
    private bool Started = false;

    [SerializeField] private float speed;

    protected override void Start()
    {
        Started = true;
        base.Start();
        CancelInvoke(nameof(targetParent));
        MainScript.EnemyDied += DieLater;
        if (PlayerMovement.PM.transform != null)
        {
            target = PlayerMovement.PM.transform;
        }
    }
    public void LateStart()
    {
        if (!Started)
        {
            Start();
        }
    }

    public void DieLater()
    {
        Invoke(nameof(Die), 0.01f);
    }
    
    public void targetParent()
    {
        target = MainScript.transform;
        rb.excludeLayers += enemyLayer;
    }
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }
    protected override void EnemySetup()
    {
        base.EnemySetup();
        invincibleByAbility = true;
    }

    protected override void Update()
    {
        if (Vector3.Distance(transform.position, target.position) > 0.1f)
        {
            DirectionVector = (target.position - transform.position).normalized;
            rb.linearVelocity = DirectionVector * speed;
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
    }
}
