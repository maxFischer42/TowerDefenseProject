using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieOnTimer : MonoBehaviour
{
    public float timeToDie = 2f;
    public bool dieOnCollision = false;

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
            Destroy(gameObject);
        }
    }

}
