using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRange : MonoBehaviour
{
    public List<Transform> transformsInRange = new List<Transform>();
    public Transform currentTarget;
    private TowerManager tower;
    public bool canSeeHidden = false;
    public bool canSeeFromSupport = false;

    private void Start()
    {
        tower = transform.parent.GetComponent<TowerManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "ENEMY")
        {
            if(collision.GetComponent<EnemyManager>().enemyIsHidden && !canSeeHidden)
            {
                return;
            }
            transformsInRange.Add(collision.transform);
            RefreshPriority();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "ENEMY")
        {
            transformsInRange.Remove(collision.transform);
            RefreshPriority();
        }
    }

    public void RefreshPriority()
    {
        if (tower == null) return;
        target_priority priority = tower.priority;
        Transform tempTarget = currentTarget;
        Debug.Log("Refreshing priority;; enemies in range: " + transformsInRange.Count);
        if (transformsInRange.Count == 0)
        {
            currentTarget = null;
            return;
        }
        if (priority == target_priority.first)
        {
            tempTarget = transformsInRange[0];
        }
        else if (priority == target_priority.last)
        {
            tempTarget = transformsInRange[transformsInRange.Count - 1];
        }
        else
        {
            foreach (Transform t in transformsInRange)
            {
                if (tempTarget == null)
                {
                    tempTarget = t;
                    continue;
                }         

                switch (priority)
                {
                    case target_priority.highHP:
                        if (t.GetComponent<EnemyManager>().GetHealth() > tempTarget.GetComponent<EnemyManager>().GetHealth())
                        {
                            tempTarget = t;
                        }
                        break;
                    case target_priority.lowHP:
                        if (t.GetComponent<EnemyManager>().GetHealth() < tempTarget.GetComponent<EnemyManager>().GetHealth())
                        {
                            tempTarget = t;
                        }
                        break;
                    case target_priority.fastest:
                        if (t.GetComponent<EnemyManager>().getEnemy().speed > tempTarget.GetComponent<EnemyManager>().getEnemy().speed)
                        {
                            tempTarget = t;
                        }
                        break;
                    case target_priority.slowest:
                        if (t.GetComponent<EnemyManager>().getEnemy().speed < tempTarget.GetComponent<EnemyManager>().getEnemy().speed)
                        {
                            tempTarget = t;
                        }
                        break;
                    case target_priority.danger:
                        if (t.GetComponent<EnemyManager>().getEnemy().dangerLevel > tempTarget.GetComponent<EnemyManager>().getEnemy().dangerLevel)
                        {
                            tempTarget = t;
                        }
                        break;
                }
            }

        }
       currentTarget = tempTarget;
    }

}
