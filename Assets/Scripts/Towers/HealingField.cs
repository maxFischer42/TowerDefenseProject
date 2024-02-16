using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingField : MonoBehaviour
{
    public List<TowerManager> towersInField = new List<TowerManager>();

    public int healAmount = 1;
    public float timeBetweenHealing = 2f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "HERO_CHARACTER") return;
        foreach(TowerManager t in towersInField)
        {            
            if(t == other.GetComponent<TowerManager>())
            {
                return;
            }
        }
        towersInField.Add(other.GetComponent<TowerManager>());
    }

    public void Start()
    {
        Invoke(nameof(Heal), timeBetweenHealing);
    }

    public void Heal()
    {
        foreach(TowerManager t in towersInField)
        {
            if (t == null) continue;
            t.UpdateHealth(healAmount * -1);
        }
        Invoke(nameof(Heal), timeBetweenHealing);
    }
}
