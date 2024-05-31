using Sirenix.OdinInspector.Editor.GettingStarted;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class TowerBuilder : MonoBehaviour
{
    [SerializeField] private GameObject tower;

    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main; 
    }

    Vector2 mousePosition;
    public void BuildTower(CallbackContext context)
    {
        if (!context.performed) return;

        // Create a ray from the camera
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        RaycastHit hit;


        // Check if the ray hits any collider
        //fire a raycast from the position of the mouse on screen 
        //check if collider beneath mouse cursor is tagged with "tower build platform
        //instantiate a tower prefab if there is a tower build platform is there

        if (!Physics.Raycast(ray, out hit)) return;
        if (!hit.collider.CompareTag("TowerBuildPlatform")) return;

        Vector3 towerSpawnPosition = hit.collider.transform.position;
        Instantiate(tower, towerSpawnPosition, Quaternion.identity);
    }

    public void MoveMouse(CallbackContext context) { 
        mousePosition = context.ReadValue<Vector2>();
        //Debug.Log($"Current mouse positions: {mousePosition}");
    }
}
