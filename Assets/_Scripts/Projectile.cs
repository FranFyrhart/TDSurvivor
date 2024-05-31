using Sirenix.OdinInspector.Editor.GettingStarted;
using System;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.UI;
using System.Runtime.InteropServices.ComTypes;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private Rigidbody projectileRB;
    [SerializeField] private float projectileSpeed = 2f;
    [SerializeField, Tooltip("Projectile lifetime in seconds")] float lifetime = 1.5f;
    [SerializeField] private string targetTag;

    float currentLifetime;

    private void Start() {
        //Debug.Log($"Projectile Spawn position: {transform.position}");
        currentLifetime = lifetime;
        StartCoroutine(StartLifetimeCountdown());
    }

    IEnumerator StartLifetimeCountdown() { 
        while (currentLifetime > 0)
        {
            currentLifetime -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        gameObject.SetActive(false);
    }

    public float ProjectileSpeed { get { return projectileSpeed; } }
    public Rigidbody ProjectileRB { get {  return projectileRB; } }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.layer == 6) {
            StopCoroutine(StartLifetimeCountdown());
            Destroy(gameObject);
            return;
        }

        if (!other.gameObject.CompareTag(targetTag)) return;
        
        CombatHandler enemyHit = other.gameObject.GetComponent<CombatHandler>();
        enemyHit.TakeDamage(damage); 

        Destroy(gameObject);
    }
}
