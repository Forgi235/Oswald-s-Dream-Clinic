using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public abstract class PlayerMovement : MonoBehaviour
{
    [SerializeField] DamageDetection DMGDet;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected GameObject AttackObjectPrefab;

    public static PlayerMovement PM;

    protected SpriteRenderer spriteRenderer;

    protected float baseMoveSpeed;
    [SerializeField] protected float moveSpeed;
    
    protected InputSystem_Actions PlayerControlls;
    protected Vector2 moveDirection = Vector2.zero;
    protected Vector2 attackDirrection = Vector2.zero;
    //made to fix issues if "attackDirrection" changes during the "AttackTimer()" coroutine
    protected Vector2 attackDirrectionSave = Vector2.zero;
    //making the sizes circular instead of square
    protected Vector2 circularAttackDirrection;

    protected InputAction move;
    protected InputAction attack;

    protected bool isAttacking = false;
    protected IEnumerator AttackCoroutine = null;

    //shots per second (frequency)
    protected float AttackFrequency;
    protected float AttackPeriod => 1f / AttackFrequency;
    protected float attackInstanceDuration;
    protected bool isAttackingDiagonally = false;

    [SerializeField] protected float MaxHP = 3;
    [SerializeField] protected float HP;

    protected float damage = 1;

    protected bool invincible = false;
    protected float IFramesDuration = 1f;
    protected IEnumerator IFrameCoroutine;
    protected IEnumerator DamageIndicatorCoroutine;
    protected int IsGettingDamaged = 0;

    public virtual float GetDamageStat()
    {
        return damage;
    }

    public virtual void Heal(float HealAmount)
    {
        HP += HealAmount;
        if(HP > MaxHP) HP = MaxHP;
    }

    protected void OnEnable()
    {
        move = PlayerControlls.Player.Move;
        move.Enable();

        attack = PlayerControlls.Player.Look;
        attack.Enable();
    }
    protected void OnDisable()
    {
        if(move != null) move.Disable();
        if(attack != null) attack.Disable();
    }
    protected virtual void Awake()
    {
        PlayerControlls = new InputSystem_Actions();
        spriteRenderer = GetComponent<SpriteRenderer>();
        HP = MaxHP;
    }
    protected void Start()
    {
        DMGDet.TookDamage += TakeDamage;
        moveSpeed = baseMoveSpeed;
    }
    protected virtual void GetInputs()
    {
        moveDirection = move.ReadValue<Vector2>();
        attackDirrection = attack.ReadValue<Vector2>();
    }
    
    protected virtual void Update()
    {
        GetInputs();
        if(attackDirrection.x != 0 || attackDirrection.y != 0)
        {
            if(attackDirrection.x != 0 && attackDirrection.y != 0)
            {
                //0.707 is roughlly the square root of 0.5 making the diagonal distace using this nuber to equal "1" making it circular
                circularAttackDirrection = new Vector2(attackDirrection.x * 0.707f, attackDirrection.y * 0.707f);
                isAttackingDiagonally = true;
            }
            else
            {
                circularAttackDirrection = attackDirrection;
                isAttackingDiagonally = false;
            }
            //if he was not attacking previously
            if(!isAttacking)
            {
                
                if (AttackCoroutine == null)
                {
                    isAttacking = true;
                    AttackCoroutine = AttackTimer();
                    StartCoroutine(AttackCoroutine);
                }
            }
        }
        else
        {
            isAttacking = false;
        }
    }
    protected virtual void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
    float AttackTime1;
    float AttackTime2;
    protected virtual IEnumerator AttackTimer()
    {
        while(isAttacking)
        {
            //yes i know i can put the float in "WaitForSeconds" in a variable and change it before the while loop
            //however it could also very easilly break if the firerate changes while attacking
            if (AttackPeriod / 6 - 0.15f > 0)
            {
                AttackTime1 = 0.15f;
                AttackTime2 = AttackPeriod - 0.15f;
            }
            else
            {
                AttackTime1 = AttackPeriod / 6;
                AttackTime2 = (AttackPeriod / 6) * 5;
            }
            //The firerate is split because the keyboard inputs for diagonal fire are twoseparate buttons
            //so the timing is slightly offset to make diagonal attacks not near frame perfect. That hovewher also
            //causes "attackDirrection" to sometimes be zero if you let go of the key fast enough, hence the "attackDirrectionSave" nonsence 
            attackDirrectionSave = new Vector2(attackDirrection.x, attackDirrection.y);
            yield return new WaitForSeconds(AttackTime1);
            if (attackDirrection != Vector2.zero)
            {
                attackDirrectionSave = attackDirrection;
            }
            Attack();
            yield return new WaitForSeconds(AttackTime2 /*seconds per attack*/);
        }
        StopCoroutine(AttackCoroutine);
        AttackCoroutine = null;
    }
    protected abstract void Attack();
    protected void TakeDamage()
    {
        if (!invincible)
        {
            if (IFrameCoroutine != null) StopCoroutine(IFrameCoroutine);
            if (IFrameCoroutine != InvincibilityFrames()) IFrameCoroutine = InvincibilityFrames();
            StartCoroutine(IFrameCoroutine);

            if (DamageIndicatorCoroutine != null) StopCoroutine(DamageIndicatorCoroutine);
            if (DamageIndicatorCoroutine != DamageIndicator()) DamageIndicatorCoroutine = DamageIndicator();
            StartCoroutine(DamageIndicatorCoroutine);

            HP--;
            if (HP <= 0) 
            {
                Die();
            }
        }
    }
    protected void Die()
    {
        SceneManager.LoadSceneAsync(1);
    }
    private IEnumerator InvincibilityFrames()
    {
        invincible = true;
        yield return new WaitForSeconds(IFramesDuration);
        invincible = false;
        if (IsGettingDamaged > 0) { TakeDamage(); }
    }
    private IEnumerator DamageIndicator()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(IFramesDuration / 5);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(IFramesDuration / 5);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(IFramesDuration / 5);
        spriteRenderer.color = Color.white;
    }
}
