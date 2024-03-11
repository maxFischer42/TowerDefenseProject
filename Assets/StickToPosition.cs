using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToPosition : MonoBehaviour
{
    public Transform transformToStick;

    // Update is called once per frame
    void Update()
    {
        transform.position = transformToStick.position;
    }
}
