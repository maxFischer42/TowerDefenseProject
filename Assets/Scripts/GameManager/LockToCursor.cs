using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockToCursor : MonoBehaviour
{
    public bool isCollidingBox = false;
    public Image img;
    int numColls = 0;
    
    void Update()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = pos;
    }

    public void TryClick()
    {
        Debug.Log("Try Click...");
        if(isCollidingBox)
        {
            GameManager.Instance.CancelUnitPurchase();
        } else
        {
            GameManager.Instance.OnPurchaseHandle(transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PATH")
        {
            img.color = Color.red;
            numColls++;
            isCollidingBox = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PATH")
        {
            numColls--;
            if(numColls <= 0)
            {
                img.color = Color.white;
                numColls = 0;
                isCollidingBox = false;
            }
            //isCollidingBox = true;
        }
    }
}
