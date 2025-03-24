using Pathfinding;
using UnityEngine;

public class NarcissisticPersonalityDisorderScript : EnemyScript
{
    [SerializeField] private AIDestinationSetter AIDSetter;
    [SerializeField] private AIPath AIPath_;

    [SerializeField] private float speed;

    protected override void Start()
    {
        AIDSetter.target = PlayerMovement.PM.transform;
    }
    protected override void EnemySetup()
    {
        base.EnemySetup();
        invincibleByAbility = true;
    }
    public override void OnRoomEnter()
    {
        base.OnRoomEnter();
        CancelInvoke(nameof(NarcisismStageOne));
        CancelInvoke(nameof(NarcisismStageTwo));
        CancelInvoke(nameof(Die));
        NarcisismStageOne();
    }
    private void NarcisismStageOne()
    {
        AIPath_.maxSpeed = 0;
        Invoke(nameof(NarcisismStageTwo), 12.5f);
    }
    private void NarcisismStageTwo()
    {
        AIPath_.maxSpeed = speed;
        Invoke(nameof(Die), 12.5f);
    }
    public override void OnRoomExit()
    {
        base.OnRoomExit();
        CancelInvoke(nameof(NarcisismStageOne));
        CancelInvoke(nameof(NarcisismStageTwo));
        CancelInvoke(nameof(Die));
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
                CancelInvoke(nameof(NarcisismStageOne));
                CancelInvoke(nameof(NarcisismStageTwo));
                CancelInvoke(nameof(Die));
                NarcisismStageOne();
            }
        }
    }
}
