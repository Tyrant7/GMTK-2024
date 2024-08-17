using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleEnemy : Enemy
{
    protected override void Behave()
    {
        
    }

    public override void TakeDamage(int damage)
    {
        Debug.Log("ouch! " + damage);
        base.TakeDamage(damage);
    }
}
