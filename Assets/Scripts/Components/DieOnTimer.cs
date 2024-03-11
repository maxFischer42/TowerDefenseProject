using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieOnTimer : MonoBehaviour
{
    public float timeToDie = 2f;
    public bool dieOnCollision = false;
    public int pierceCount = 2;

    void Awake()
    {
        Invoke(nameof(Despawn), timeToDie);
    }

    void Despawn()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(dieOnCollision && collision.gameObject.tag == "ENEMY")
        {
            Despawn();
        } else if(dieOnCollision == false && collision.gameObject.tag == "ENEMY")
        {
            if(pierceCount > 0)
            {
                pierceCount--;
            }
            else
            {
                Despawn();
            }
        }
    }

}
