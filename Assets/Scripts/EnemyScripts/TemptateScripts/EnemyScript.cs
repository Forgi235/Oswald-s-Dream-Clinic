using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyScript : MonoBehaviour
{

    [SerializeField] protected float MaxHP;
    public event Action EnemyDied;
    private Vector3 StartingPosition;
    protected SpriteRenderer SpriteR;
    protected float HP;
    //RoomBlock determines whether or not the enemy is required to be defeated to leave the room
    [SerializeField] protected bool RoomBlock;
    protected bool alive;
    protected Color originalColor;

    IEnumerator DamageIndicatorCoroutine;
    protected IEnumerator IFrameCoroutine;

    protected bool invincible = false;
    protected bool invincibleByAbility = false;
    protected float IFramesDuration = 0.2f;

    protected virtual void Awake()
    {
        EnemySetup();
        StartingPosition = transform.position;
        SpriteR = GetComponent<SpriteRenderer>();
        originalColor = SpriteR.color;
    }
    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {
        
    }
    protected virtual void FixedUpdate()
    {

    }
    public virtual void ActivateEnemy()
    {
        gameObject.SetActive(alive);
    }
    public virtual void DeactivateEnemy()
    {
        gameObject.SetActive(false);
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            if (!invincible && !invincibleByAbility)
            {
                GotHit();
                if (IFrameCoroutine != null) StopCoroutine(IFrameCoroutine);
                if (IFrameCoroutine != InvincibilityFrames()) IFrameCoroutine = InvincibilityFrames();
                StartCoroutine(IFrameCoroutine);

                TakeDamage(PlayerMovement.PM.GetDamageStat());
            }
        }
    }
    protected virtual void GotHit() { }

    protected virtual void TakeDamage(float damage)
    {
        HP -= damage;

        if (DamageIndicatorCoroutine != null) StopCoroutine(DamageIndicatorCoroutine);
        if (DamageIndicatorCoroutine != DamageIndicator()) DamageIndicatorCoroutine = DamageIndicator();
        StartCoroutine(DamageIndicatorCoroutine);

        if (HP <= 0)
        {
            Invoke(nameof(Die), 0.1f);
        }
    }
    protected virtual void Die()
    {
        alive = false;
        EnemyDied?.Invoke();
        transform.position = StartingPosition;
        gameObject.SetActive(false);
    }
    protected virtual void EnemySetup()
    {
        EnemyStatSetup();
        HP = MaxHP;
    }
    protected virtual void EnemyStatSetup()
    {
        alive = true;
    }
    protected virtual void SpawnAndReload()
    {
        EnemyStatSetup();
        this.gameObject.SetActive(true);
    }
    public virtual void OnRoomEnter()
    {
        transform.position = StartingPosition;
        HP = MaxHP;
    }
    public virtual void OnRoomExit()
    {

    }
    public bool isBlockingRoom()
    {
        if (RoomBlock && alive) return true;
        return false;
    }
    protected virtual IEnumerator DamageIndicator()
    {
        SpriteR.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        SpriteR.color = originalColor;
    }
    protected virtual IEnumerator InvincibilityFrames()
    {
        invincible = true;
        yield return new WaitForSeconds(IFramesDuration);
        invincible = false;
    }
}
