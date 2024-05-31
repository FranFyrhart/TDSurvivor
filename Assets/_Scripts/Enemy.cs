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
        //Transform playerTransform = FindObjectOfType<PlayerCharacter>().transform;
        currentHP = baseHP;
        meshAgent = gameObject.GetComponent<NavMeshAgent>();
        //material = GetComponent<MeshRenderer>().material;
        playerTransform = FindObjectOfType<PlayerMovement>().transform;
    }

    private void Update()
    {
        meshAgent.SetDestination(playerTransform.position);
        if (meshAgent.remainingDistance < attackRange && attackCooldownDone)
        {
            //meshAgent.isStopped = true;
            StartAttack();
        }
    }

    private void StartAttack() {
        //if (attackCoroutine != null)
        //{
        //    Debug.Log("Starting attack coroutine");
        //    //attackCoroutine.
        //    StopCoroutine(attackCoroutine);
        //    StartCoroutine(attackCoroutine);
        //    return;
        //}
        //attackCoroutine = Attack();
        StartCoroutine(Attack());
    }

    //public override void TakeDamage(int damage) {
    //    Debug.Log("Taking damage");
    //    currentHP -= damage;

    //    if (currentHP <= 0)
    //    {
    //        gameObject.SetActive(false);
    //        return;
    //    }

    //    #region visual feedback 

    //    if (colorTween != null)
    //    {
    //        if (!colorTween.IsComplete()) {
                
    //            Debug.Log("Color tween not yet completed");
    //            return;
    //        }

    //        Debug.Log($"playing tween {colorTween.IsActive()}");
    //        colorTween.Restart();
    //        return;
    //    }

    //    colorTween = material.DOColor(Color.black, "_BaseColor", 0.1f).From().SetLoops(1).SetAutoKill(false);
    //    #endregion
    //}

    private IEnumerator Attack() 
    {
        attackCooldownDone  = false;
        currentCooldown = 1f / attackSpeed;

        Projectile newProjectile = Instantiate(projectile, projectileSpawnPoint.position, transform.rotation).GetComponent<Projectile>();
        //Debug.Log($"Intended spawn position: {transform.position}");
        // Get the rigidbody component of the projectile
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
