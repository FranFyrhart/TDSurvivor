using DG.Tweening;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Unity.Loading;
using UnityEngine;
using UnityEngine.Analytics;

public class TowerDetector : MonoBehaviour
{
    [SerializeField] List<Collider> activatedTowers, targetCollidersHit;
    [SerializeField] LayerMask enemyLayer;

    RaycastHit[] targetsHit;
    List<Collider> targetCollidersHitCopy = new();
    List<Task> laserDamageTicks = new();
    
    private void Awake()
    {
        activatedTowers = new();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Tower")) return;
        {
            //Debug.Log("Collided with tower");
            activatedTowers.Add(other);
            Tower towerToActivate = other.GetComponent<Tower>(); 
            towerToActivate.TowerDeactivated = false;
            //towerToActivate.attackEnemies();
            towerToActivate.Player = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Tower")) return;
        //Debug.Log("Removing tower");

        Tower towerToDeactivate = other.GetComponent<Tower>();
        towerToDeactivate.TowerDeactivated = true;
        towerToDeactivate.Player = null; 
        activatedTowers.Remove(other);
    }
}
