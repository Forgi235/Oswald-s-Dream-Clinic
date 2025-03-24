using UnityEngine;
using Pathfinding;

public class OCD_Script : EnemyScript
{
    [SerializeField] private AIDestinationSetter AIDSetter;
    [SerializeField] private AIPath AIPath_;
    [SerializeField] private Transform[] Path;
    [SerializeField] private float NormalSpeed;
    [SerializeField] private float EnragedSpeed;

    private int ArrayIndex = 0;
    private bool Enraged = false;

    protected override void EnemySetup()
    {
        base.EnemySetup();
        invincibleByAbility = true;
        Enraged = false;
        AIPath_.maxSpeed = NormalSpeed;
    }
    protected override void Start()
    {
        base.Start();
        if(Path.Length > 0)
        {
            AIDSetter.target = Path[ArrayIndex];
        }
        else
        {
            Debug.Log("OCD Enemy has no path");
        }
    }
    protected override void Update()
    {
        base.Update();
        if (!Enraged)
        {
            if (Vector3.Distance(Path[ArrayIndex].position, transform.position) < 0.3f)
            {
                NextTargetPoint();
            }
        }
    }
    private void NextTargetPoint()
    {
        ArrayIndex++;
        if(ArrayIndex >= Path.Length) ArrayIndex = 0;
        AIDSetter.target = Path[ArrayIndex];
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D (collision);
        if (collision.CompareTag("MoveableObject")) Invoke(nameof(Enrage), 0.75f);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("MoveableObject")) CancelInvoke(nameof(Enrage));
    }
    private void Enrage()
    {
        Enraged = true;
        AIDSetter.target = PlayerMovement.PM.transform;
        AIPath_.maxSpeed = EnragedSpeed;
        invincibleByAbility = false;
    }
    public override void OnRoomEnter()
    {
        base.OnRoomEnter();
        Enraged = false;
        ArrayIndex = 0;
        AIDSetter.target = Path[ArrayIndex];
        AIPath_.maxSpeed = NormalSpeed;
        invincibleByAbility = true;
    }
}
