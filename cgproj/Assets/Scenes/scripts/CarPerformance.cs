using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPerformance : ScriptableObject
{
    public float distance;
    public float averageSpeed; //Useless, averageSpeed = distance/time
    public float topSpeed; //not really useful criteria i think, what matters is the time it took
    public float time;
    public bool finishedTrack;

    public float TotalScore()
    {
        float perf = distance / time;

        if (finishedTrack)
        {
            perf += 6.86f;
        }
        
        return perf;
    }
}
