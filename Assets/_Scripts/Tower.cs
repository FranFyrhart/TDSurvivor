using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Combat settings")]
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private int towerDamage = 100;
    [SerializeField] private float rateOfFire = 1f;
    [SerializeField] private List<Collider> targetCollidersHit;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform connectionPoint;

    [Space]
    [SerializeField] protected Transform projectileSpawnPoint;
    [SerializeField] protected Transform turretPivot;
    
    private bool _towerDeactivated = true, attackCoroutineRunning = false;
    private Transform _player;
    private Transform currentTarget;
    private IEnumerator attackCoroutine;
    protected List<Transform> targetsInRange = new();

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
        if (_player == null)
            return;

        Debug.DrawRay(transform.position, _player.transform.position - transform.position, Color.green);

        if (targetsInRange.Count == 0)
            return;

        currentTarget = targetsInRange.Aggregate((minItem, nextItem) =>
            Vector3.Distance(minItem.position, transform.position) < Vector3.Distance(nextItem.position, transform.position) ? minItem : nextItem);

        if (currentTarget == null)
            return;

        Vector3 targetPosition = currentTarget.transform.position;
        targetPosition.y = turretPivot.position.y; // Keep the same height as the turretPivot
        turretPivot.LookAt(targetPosition);
        //Attack();
    }

    private void Attack()
    {
        //targetCollidersHit = Physics.OverlapCapsule(transform.position, _player.position, 1f, enemyLayer).ToList();

        targetCollidersHit = Physics.OverlapSphere(transform.position, attackRange, enemyLayer).ToList();

        if (targetCollidersHit.Count == 0) return;

        //for each overlap capsule, flash the material of the target
        //for (int j = 0; j < targetCollidersHit.Count; j++)
        //{
        //    Enemy enemy = targetCollidersHit[j].GetComponent<Enemy>();
        //    if (enemy == null) continue;
        //    enemy.TakeDamage(towerDamage);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Adding target");
        targetsInRange.Add(other.transform);
        //if (other.CompareTag(StringConstants.Tags.enemyTag))
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(StringConstants.Tags.enemyTag))
            targetsInRange.Remove(other.transform);
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
            float currentCooldown = 1f / rateOfFire;
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
