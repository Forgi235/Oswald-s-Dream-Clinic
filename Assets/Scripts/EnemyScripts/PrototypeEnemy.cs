using Pathfinding;
using UnityEngine;

public class PrototypeEnemy : EnemyScript
{
    protected override void Start()
    {
        base.Start();
        if (PlayerMovement.PM.transform != null)
        {
            GetComponent<AIDestinationSetter>().target = PlayerMovement.PM.transform;
        }
    }
    protected override void EnemyStatSetup()
    {
        base.EnemyStatSetup();
    }
    protected override void EnemySetup()
    {
        base.EnemySetup();
    }
}
