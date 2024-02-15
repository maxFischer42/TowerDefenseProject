using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeManager : MonoBehaviour
{
    public int baseHealth = 100;
    private int currentHealth;


    private bool active = true;

    void Start()
    {
        currentHealth = baseHealth;
    }

    void HealthUpdate()
    {
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("DEFEAT");
            active = false;
        }
    }

    private void Update()
    {
        if (!active) return;
        HealthUpdate();
    }

    public void ModifyHealth(int change, bool isHarm)
    {
        int sign = isHarm ? -1 : 1;
        currentHealth += (change * sign);
    }
}
