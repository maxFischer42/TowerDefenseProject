using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePuchaseCallback : MonoBehaviour
{
    public GameManager manager;
    public int id;
    public void Setup(int _id, GameManager _manager)
    {
        manager = _manager;
        id = _id;
    }

    public void OnSelectCallback()
    {
        //manager.OnPurchaseHandle(id);
    }

    public void OnHoverCallback()
    {
    }
}
