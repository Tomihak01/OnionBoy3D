using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potato : EnemyBase
{
    
    protected override void SetEnemyStats()
    {
        moveSpeed = 3f; 
        
    }

   
    protected override void AttackPlayer()
    {
        
        Debug.Log("Potato hyökkää pelaajaa vastaan!");
    }
}