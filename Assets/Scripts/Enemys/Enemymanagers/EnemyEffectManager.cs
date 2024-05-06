using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffectManager : MonoBehaviour
{
 EnemyManager enemyManager;

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
    }

    public void DamageEnemy(int damage)
    {

        enemyManager.enemyHealthManager.DealDamage(damage);

    }

}
