using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.UI;

public class Enemy : CombatHandler
{
    //[SerializeField] int maxhp = 1000;
    [SerializeField] Transform playerTransform;
    [SerializeField] private GameObject projectile;
    [SerializeField, Tooltip("Attacks per second")]
    float attackSpeed, attackRange;
    [SerializeField] private Transform projectileSpawnPoint; 

    float currentCooldown;
    bool attackCooldownDone = true;

    //Material material;
    Tween colorTween;
    NavMeshAgent meshAgent;
    IEnumerator attackCoroutine;

    private void Awake()
    {
        currentHP = baseHP;
        meshAgent = gameObject.GetComponent<NavMeshAgent>();
        playerTransform = FindObjectOfType<PlayerMovement>().transform;
    }

    private void Update()
    {
        if(playerTransform == null) 
            return;

        meshAgent.SetDestination(playerTransform.position);
        if (meshAgent.remainingDistance < attackRange && attackCooldownDone)
        {
            //meshAgent.isStopped = true;
            StartCoroutine(Attack());
        }
    }
    private IEnumerator Attack() 
    {
        attackCooldownDone  = false;
        currentCooldown = 1f / attackSpeed;

        Projectile newProjectile = Instantiate(projectile, projectileSpawnPoint.position, transform.rotation).GetComponent<Projectile>();
        Rigidbody rb = newProjectile.ProjectileRB;

        // Set the velocity of the projectile to the enemy's forward direction multiplied by the speed factor
        rb.velocity = projectileSpawnPoint.forward * newProjectile.ProjectileSpeed;

        while (currentCooldown > 0)
        {
            currentCooldown -= 0.01f; 
            yield return new WaitForSeconds(0.01f);
        }
        attackCooldownDone = true;
    }
}
