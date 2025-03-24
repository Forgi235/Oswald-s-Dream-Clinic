using Pathfinding;
using UnityEngine;

public class BipolarDisorderScript : EnemyScript
{

    [SerializeField] private AIDestinationSetter AIDSetter;
    [SerializeField] private AIPath AIPath_;

    [SerializeField] private Transform[] Path;
    private int ArrayIndex = 0;

    [SerializeField] private float HappyFormSpeed;
    [SerializeField] private float SadFormSpeed;

    [SerializeField] private float SadAuraDistance;
    [SerializeField] private float SadAuraStrenght;
    private Vector3 VectorDirection;

    private int hitCounter = 0; 
    private int a;
    private int FormSwitchCounter = 0;

    private bool SadForm_ = false;
    private bool HappyForm_ = false;

    protected override void EnemySetup()
    {
        base.EnemySetup();
        invincibleByAbility = true;
    }
    protected override void Update()
    {
        base.Update();
        if (SadForm_)
        {
            if (Vector3.Distance(PlayerMovement.PM.transform.position, transform.position) < SadAuraDistance)
            {
                VectorDirection = (transform.position - PlayerMovement.PM.transform.position).normalized;
                PlayerMovement.PM.transform.position = PlayerMovement.PM.transform.position + (VectorDirection * SadAuraStrenght * 0.001f);
            }
        }
        else if (HappyForm_)
        {
             if (Vector3.Distance(Path[ArrayIndex].position, transform.position) < 0.5f)
             {
                NextTargetPoint();
             }
        }
    }
    private void NextTargetPoint()
    {
        ArrayIndex = Random.Range(0, Path.Length);
        AIDSetter.target = Path[ArrayIndex];
    }
    protected override void Start()
    {
        FormSwitch();
        FormSwitchCounter = 0;
    }
    private void RandomForm()
    {
        invincibleByAbility = true;
        a = Random.Range(0, 2);
        if (a == 0) HappyForm();
        else SadForm();
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
                hitCounter++;
                if(hitCounter >= 3)
                {
                    FormSwitch();
                    hitCounter = 0;
                }
            }
        }
    }
    private void FormSwitch()
    {
        invincibleByAbility = true;
        if (FormSwitchCounter >= 3)
        {
            BecomeVulnerable();
            return;
        }
        if(HappyForm_)
        {
            SadForm();
        }
        else if ( SadForm_)
        {
            HappyForm();
        }
        else
        {
            RandomForm();
        }
        FormSwitchCounter++;
    }
    private void SadForm()
    {
        SadForm_ = true;
        HappyForm_ = false;
        AIPath_.maxSpeed = SadFormSpeed;
        AIDSetter.target = PlayerMovement.PM.transform;
    }
    private void HappyForm()
    {
        SadForm_ = false;
        HappyForm_ = true;
        NextTargetPoint();
        AIPath_.maxSpeed = HappyFormSpeed;
    }
    
    private void BecomeVulnerable()
    {
        SadForm_ = false;
        HappyForm_ = false;
        invincibleByAbility = false;
        AIPath_.maxSpeed = 0;
        FormSwitchCounter = 0;
        Invoke(nameof(RandomForm), 3);
    }
    public override void OnRoomEnter()
    {
        base.OnRoomEnter();
        FormSwitch();
        FormSwitchCounter = 0;
        invincibleByAbility = true;
    }
}
