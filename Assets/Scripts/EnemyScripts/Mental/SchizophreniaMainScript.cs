using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SchizophreniaMainScript : EnemyScript
{
    [SerializeField] private Transform[] SpawnPoints;
    [SerializeField] private SchizophreniaGhostScript[] ghostScripts;

    private int GhostCounter = 0;
    private bool AllGhostsSpawned = false;

    public override void OnRoomEnter()
    {
        base.OnRoomEnter();
        GhostCounter = 0;
        AllGhostsSpawned = false;
        foreach (SchizophreniaGhostScript ghostScript in ghostScripts)
        {
            ghostScript.gameObject.SetActive(false);
        }
        CancelInvoke(nameof(BecomeVulnerable));
        CancelInvoke(nameof(SpawnGhost));
        Invoke(nameof(BecomeVulnerable), 20f);
        transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Length)].position;
        invincibleByAbility = true;
        SpawnGhost();
    }
    protected override void Start()
    {
        base.Start();
        AllGhostsSpawned = false;
        CancelInvoke(nameof(BecomeVulnerable));
        Invoke(nameof(BecomeVulnerable), 20f);
        transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Length)].position;
        invincibleByAbility = true;
    }
    protected override void Die()
    {
        foreach(SchizophreniaGhostScript script in ghostScripts)
        {
            script.DieLater();
        }
        CancelInvoke(nameof(SpawnGhost));
        base.Die();
    }
    public override void OnRoomExit()
    {
        base.OnRoomExit();
        CancelInvoke(nameof(SpawnGhost));
        foreach (var ghost in GetComponentsInChildren<SchizophreniaGhostScript>())
        {
            ghost.OnRoomExit();
        }
    }
    private void BecomeVulnerable()
    {
        invincibleByAbility = false;
    }
    int a = 0;
    private void SpawnGhost()
    {
        if (AllGhostsSpawned) return;
        if (!alive)
        {
            CancelInvoke(nameof(SpawnGhost));
            return;
        }
        ghostScripts[GhostCounter].gameObject.SetActive(true);
        ghostScripts[GhostCounter].transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Length)].position;
        a = 0;
        while (Vector3.Distance(ghostScripts[GhostCounter].transform.position, PlayerMovement.PM.transform.position) < 4)
        {
            ghostScripts[GhostCounter].transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Length)].position;
            a++;
            if (a > 10) break;
        }
        ghostScripts[GhostCounter].LateStart();
        GhostCounter++;
        if (GhostCounter >= ghostScripts.Length)
        {
            AllGhostsSpawned = true;
            ghostScripts[0].targetParent();
            return;
        }
        Invoke(nameof(SpawnGhost), 7f);
    }
}
