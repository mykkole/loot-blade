using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiblePlantVisual : MonoBehaviour
{
    [SerializeField] private DestructiblePlant destructiblePlant;
    [SerializeField] private GameObject bushDeathVFXPrefab;

    private void Start()
    {
        destructiblePlant.OnDestructibleTakeDamage += DestructiblePlan_OnDestructibleTakeDamage;
    }

    private void DestructiblePlan_OnDestructibleTakeDamage(object sender, System.EventArgs e)
    {
        ShowDeathVFX();
    }

    private void ShowDeathVFX()
    {
        Instantiate(bushDeathVFXPrefab, destructiblePlant.transform.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        destructiblePlant.OnDestructibleTakeDamage -= DestructiblePlan_OnDestructibleTakeDamage;
    }
}
