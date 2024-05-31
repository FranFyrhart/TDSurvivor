using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] float attackInterval = 1f;
    [SerializeField] List<Collider> targetCollidersHit;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] int towerDamage = 100;
    [SerializeField] Transform connectionPoint; 

    Task attackingEnemiesTask;
    bool _towerDeactivated = true, attackCoroutineRunning = false;
    Transform _player;
    IEnumerator attackCoroutine;

    public Transform Player { set { _player = value; } }

    public bool TowerDeactivated {
        set {
            _towerDeactivated = value;

            if (value == true) {
                StopCoroutine(attackCoroutine);
                return;
            }

            if (attackCoroutine == null)
            {
                attackCoroutine = AttackEnemiesPerInterval();
            }

            StartCoroutine(attackCoroutine);
        }
    }

    private void Update()
    {
        if (_towerDeactivated)
            return;

        if (_player == null)
            return;

        Debug.DrawRay(transform.position, _player.transform.position - transform.position, Color.green);
        //Attack();
    }

    private void Attack()
    {
        targetCollidersHit = Physics.OverlapCapsule(connectionPoint.position, _player.position, 1f, enemyLayer).ToList();

        if (targetCollidersHit.Count == 0) return;

        //for each overlap capsule, flash the material of the target
        for (int j = 0; j < targetCollidersHit.Count; j++)
        {
            Enemy enemy = targetCollidersHit[j].GetComponent<Enemy>();
            if (enemy == null) continue;
            enemy.TakeDamage(towerDamage);
        }
    }

    public Transform GetConnectionPoint() {
        return connectionPoint;
    }

    //public void attackEnemies() {
    //}

    public IEnumerator AttackEnemiesPerInterval(/*List<Enemy> enemies*/) {
        //if (enemies == null) return;
        //if (enemies.Count == 0) return;
        while (!_towerDeactivated)
        {
            Debug.Log("Damaging enemies");
            float currentCooldown = attackInterval;
            while (currentCooldown > 0)
            {
                currentCooldown -= 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
            Attack();
        }
    }

    public void StopAttack() {
        _towerDeactivated = true;
        attackCoroutineRunning = false;
        //attackingEnemiesTask
        //attackingEnemiesTask.Cancel();
    }
}
