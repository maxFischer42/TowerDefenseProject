using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerifyPurchase : MonoBehaviour
{
    public int _id;
    public bool isHero = false;
    public void OnButton()
    {
        GameManager.Instance.VerifyPurchase(_id);
    }
}
