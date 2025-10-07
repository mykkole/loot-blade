using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapone : MonoBehaviour
{
    public static ActiveWeapone Instance { get; private set; }
    [SerializeField] private Sword sword;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Player.Instance.IsAlive())
        {
            WeaponFacingDirection();
        }

    }
    public Sword GetActiveWeapon()
    {
        return sword;
    }

    private void WeaponFacingDirection()
    {
        if (!Player.Instance.IsFacingRight)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
