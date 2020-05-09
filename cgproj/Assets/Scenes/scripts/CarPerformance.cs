using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPerformance : ScriptableObject
{
    public float distance;
    public float averageSpeed;
    public float topSpeed;
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
