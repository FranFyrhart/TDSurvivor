using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Tower : MonoBehaviour
{
    [Header("Combat settings")]
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private int towerDamage = 100;
    [SerializeField] private float rateOfFire = 1f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform connectionPoint;
    [SerializeField] private GameObject projectile;

    [Space]
    [SerializeField] protected Transform projectileSpawnPoint;
    [SerializeField] protected Transform turretPivot;
    
    private List<Collider> targetCollidersHit;
    private bool _towerDeactivated = true, attackCoroutineRunning = false;
    private Transform _player;
    private Transform currentTarget;
    private IEnumerator attackCoroutine;
    protected List<Transform> targetsInRange = new();
    private bool attackCooldownDone = true;
    private float currentCooldown;

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
            return;

        Vector3 targetPosition = currentTarget.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - turretPivot.position, Vector3.up);
        turretPivot.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
        
        if (!attackCooldownDone)
        {
            Debug.Log("attack cooldown not done");
            return;
        }

        if (targetsInRange.Count == 0)
        {
            Debug.LogWarning("no targets in range");
            return;
        }

        if (currentTarget == null)
        {
            Debug.LogWarning("No target");
            return;
        }

        Attack();
    }

    private void Attack()
    {
        attackCooldownDone = false;
        currentCooldown = 1f / rateOfFire;

        Projectile newProjectile = Instantiate(projectile, projectileSpawnPoint.position, transform.rotation).GetComponent<Projectile>();
        //Debug.Log($"Intended spawn position: {transform.position}");
        // Get the rigidbody component of the projectile
        Rigidbody rb = newProjectile.ProjectileRB;

        Vector3 targetDirection = currentTarget.transform.position - projectileSpawnPoint.position;
        targetDirection.Normalize();
        Quaternion projectileRotation = Quaternion.LookRotation(targetDirection);

        // Set the velocity of the projectile towards the target
        rb.velocity = targetDirection * newProjectile.ProjectileSpeed;
        rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, projectileRotation, 0.1f);

        StartCoroutine(CooldownAttack());
        
        Debug.Log("Tower Attacking");

        IEnumerator CooldownAttack()
        {
            while (currentCooldown > 0)
            {
                currentCooldown -= 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
            attackCooldownDone = true;
        }
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
            //Attack();
        }
    }

    public void StopAttack() {
        _towerDeactivated = true;
        attackCoroutineRunning = false;
        //attackingEnemiesTask
        //attackingEnemiesTask.Cancel();
    }
}
