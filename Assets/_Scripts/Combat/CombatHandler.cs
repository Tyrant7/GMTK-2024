using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatHandler : MonoBehaviour
{
    private List<Enemy> enemies;

    public void StartCombat(List<SpawnRequest> requests, int score)
    {
        enemies = new List<Enemy>();
        foreach (SpawnRequest request in requests)
        {
            GameObject spawned = Instantiate(request.prefab, request.position, Quaternion.identity);
            if (request.isPlayer)
            {
                PlayerController player = spawned.GetComponent<PlayerController>();
                player.Initialize(score);
                Camera.main.GetComponent<CameraFollow>().EnableFollow(player.transform);
                continue;
            }
            Enemy enemy = spawned.GetComponent<Enemy>();
            enemy.OnDeath += EnemyDeath;
            enemies.Add(enemy);
        }
    }

    public void EnemyDeath(Enemy enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count <= 0)
        {
            GameManager.Instance.EndCombatPhase();
        }
    }
}
