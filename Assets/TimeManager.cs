using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            Time.timeScale = Time.timeScale + 0.01f;
            print(Time.timeScale);
        } else if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = Time.timeScale - 0.01f;
            print(Time.timeScale);
        }
    }
}
