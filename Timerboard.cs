using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timerboard : MonoBehaviour
{

    private bool timeIsFrozen = true;
    private float timer = 0f;
    private void Update()
    {
        if (!timeIsFrozen)
        {
            timer += Time.deltaTime;
        }            
    }

    public void AddToTimer(float _amount)
    {
        timer += _amount;
    }

    public float GetTimer()
    {
        return timer;
    }

    public void SetTimeFreeze()
    {
        if (timeIsFrozen)
        {
            timeIsFrozen = false;
        }
        else
        {
            timeIsFrozen = true;
        }
    }
}
