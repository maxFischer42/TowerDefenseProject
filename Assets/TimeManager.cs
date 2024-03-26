using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public float runSpeed = 1.0f;

    private void Start()
    {
        Time.timeScale = runSpeed;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void UnPause()
    {
        Time.timeScale = runSpeed;
    }

}
