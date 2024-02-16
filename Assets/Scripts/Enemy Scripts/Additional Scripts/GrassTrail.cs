using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTrail : MonoBehaviour
{
    public float speedModifier = 1.5f;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "ENEMY")
        {
            collision.GetComponent<Pather>().ModSpeed(speedModifier);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ENEMY")
        {
            collision.GetComponent<Pather>().ModSpeed(-speedModifier);
        }
    }
}
