using UnityEngine;

public class ADHDMainScript : EnemyScript
{
    [SerializeField] private ADHDMiniScript[] MiniGuys;
    private float originalSize = 1;
    [SerializeField] private float maxSize;
    private bool isGrowing = false;
    private float GrowingTime = 10;


    protected override void Awake()
    {
        base.Awake();
        originalSize = transform.localScale.x;
    }

    protected override void Start()
    {
        base.Start();
        foreach (ADHDMiniScript guy in MiniGuys)
        {
            guy.MiniGuyHit += ResetADHDTimer;
        }
        ResetADHDTimer();
    }
    protected override void EnemySetup()
    {
        base.EnemySetup();
        invincibleByAbility = true;
    }
    private void ResetADHDTimer()
    {
        invincibleByAbility = true;
        transform.localScale = new Vector3(originalSize, originalSize, originalSize);
        isGrowing = false;
        CancelInvoke(nameof(StartGrowth));
        Invoke(nameof(StartGrowth), 10f);
    }
    public override void OnRoomEnter()
    {
        base.OnRoomEnter();
        ResetADHDTimer();
    }
    public override void OnRoomExit()
    {
        base.OnRoomExit();
        ResetADHDTimer();
    }
    private void StartGrowth()
    {
        isGrowing = true;
    }
    protected override void Update()
    {
        base.Update();
        if(isGrowing)
        {
            if (transform.localScale.x >= maxSize)
            {
                BecomeVulnerable();
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x + ((maxSize - originalSize) * Time.deltaTime / GrowingTime), transform.localScale.y + ((maxSize - originalSize) * Time.deltaTime / GrowingTime), originalSize);
            }
        }
    }
    private void BecomeVulnerable()
    {
        invincibleByAbility = false;
    }
}
