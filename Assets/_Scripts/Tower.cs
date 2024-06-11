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
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform connectionPoint;

    [Space]
    [SerializeField] protected Transform projectileSpawnPoint;
    [SerializeField] protected Transform turretPivot;
    
    private List<Collider> targetCollidersHit;
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
        if (targetsInRange.Count == 0)
            return;

        if (currentTarget == null)
        { 
            Debug.LogWarning("Current target is null");
            return;
        }

        Vector3 targetPosition = currentTarget.transform.position;
        Debug.Log("Looking at target");
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - turretPivot.position, Vector3.up);
        turretPivot.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
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
        currentTarget = targetsInRange.OrderBy(target => Vector3.Distance(target.position, transform.position)).FirstOrDefault();
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
