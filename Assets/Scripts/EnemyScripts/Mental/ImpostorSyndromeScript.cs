using System;
using System.Collections;
using UnityEngine;

public class ImpostorSyndromeScript : EnemyScript
{
    protected override IEnumerator DamageIndicator()
    {
        yield return new WaitForSeconds(0.01f);
    }
}
