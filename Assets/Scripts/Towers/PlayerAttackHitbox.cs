using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    private int damage;

    public void SetDamage(int value)
    {
        damage = value;
    }
}
