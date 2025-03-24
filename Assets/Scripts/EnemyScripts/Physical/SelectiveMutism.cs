using Pathfinding;
using UnityEngine;

public class SelectiveMutism : EnemyScript
{
    protected override void Start()
    {
        base.Start();
        if (PlayerMovement.PM.transform != null)
        {
            GetComponent<AIDestinationSetter>().target = PlayerMovement.PM.transform;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        IFramesDuration = 4f;
    }
    protected override void GotHit()
    {
        base.GotHit();
        // even if he got hit durring IFrames the iframe duration resets
        if (IFrameCoroutine != null) StopCoroutine(IFrameCoroutine);
        if (IFrameCoroutine != InvincibilityFrames()) IFrameCoroutine = InvincibilityFrames();
        StartCoroutine(IFrameCoroutine);
    }
}
