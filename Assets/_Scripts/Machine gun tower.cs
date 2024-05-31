using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.ProBuilder;
using UnityEngine.Serialization;

public class Machineguntower : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float rateOfFire = 5f;
    [SerializeField] Transform projectileSpawnPoint;
    
    Transform target;
    Enemy currentTargetEnemy;

    private List<Transform> enemiesInRange = new();
    IEnumerator attackCoroutine;
    bool attacking = false;
    bool cooldownDone = true;

    float firingInterval, currentCooldown;
    // Start is called before the first frame update
    private void Awake()
    {
        firingInterval = 1f / rateOfFire;
    }

    private void Update()
    {
        if (cooldownDone && attacking)
        {
            StartCoroutine(StartAttack());
            //attacking = true;
            //if (attackCoroutine == null)
            //{
            //    attackCoroutine = StartAttack();
            //}
            //if (attacking) return;
        }
    }

    IEnumerator StartAttack() {
        if (currentTargetEnemy == null)
        {
            target = FindClosestAliveEnemy();
            currentTargetEnemy = target.gameObject.GetComponent<Enemy>();
        }
        if (currentTargetEnemy.GetCurrentHP() <= 0)
        {

        }

        cooldownDone = false;
        currentCooldown = firingInterval;
        while (currentCooldown > 0)
        {
            currentCooldown -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        Projectile newProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, transform.rotation).GetComponent<Projectile>();
        //Debug.Log($"Intended spawn position: {transform.position}");
        // Get the rigidbody component of the projectile
        Rigidbody rb = newProjectile.ProjectileRB;

        Vector3 targetDirection = target.transform.position - rb.position;
        targetDirection.Normalize();
        Quaternion projectileRotation = Quaternion.LookRotation(targetDirection);

        // Set the velocity of the projectile to the enemy's forward direction multiplied by the speed factor
        rb.velocity = targetDirection * newProjectile.ProjectileSpeed;
        rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, projectileRotation, 0.1f);
        cooldownDone = true;
    }

    Transform FindClosestAliveEnemy()
    {
        // Use LINQ to compare the distances between the enemies and the current class position
        // and return the one with the smallest distance
        enemiesInRange.RemoveAll(enemy => enemy == null);

        return enemiesInRange.Aggregate((minItem, nextItem) =>
            Vector3.Distance(minItem.position, transform.position) < Vector3.Distance(nextItem.position, transform.position) ? minItem : nextItem);
    }
     
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringConstants.Tags.enemyTag))
        {
            enemiesInRange.Add(other.transform);
            attacking = true;
            //target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(StringConstants.Tags.enemyTag))
        {
            enemiesInRange.Remove(other.transform);
            enemiesInRange.TrimExcess();
            attacking = enemiesInRange.Count > 0;
        }
    }
}
