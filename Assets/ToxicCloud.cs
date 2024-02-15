using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicCloud : MonoBehaviour
{
    private List<EnemyManager> listOfHiddenEnemies = new List<EnemyManager>();
    public float timeToDie = 4f;

    void Start()
    {
        Invoke(nameof(DestroyOnTimer), timeToDie);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ENEMY")
        {
            EnemyManager en = collision.GetComponent<EnemyManager>();
            en.enemyIsHidden = true;
            listOfHiddenEnemies.Add(en);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "ENEMY")
        {
            EnemyManager en = collision.GetComponent<EnemyManager>();
            foreach (EnemyManager enemy in listOfHiddenEnemies)
            {
                if (en == enemy)
                {
                    listOfHiddenEnemies.Remove(en);
                    en.enemyIsHidden = false;
                    return;
                }
            }
        }
    }

    public void DestroyOnTimer()
    {
        foreach(EnemyManager enemy in listOfHiddenEnemies)
        {
            enemy.enemyIsHidden = false;
        }
        Destroy(gameObject);
    }
}