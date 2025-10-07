using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class EnemySO :  ScriptableObject
{
    public string enemyType;
    public int enemyHealth;
    public int enemyDamageAmount;
}
