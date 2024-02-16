using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePriority : MonoBehaviour
{
    public int direction = 1;
    public void OnClickCallback()
    {
        GameManager.Instance.ChangeHeroPriority(direction);
    }
}
