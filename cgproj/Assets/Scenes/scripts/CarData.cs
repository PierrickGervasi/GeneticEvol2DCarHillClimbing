using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarData
{
    public CarData(CarParameters param, int carId, int gen)
    {
        parameters = param;
        carIndex = carId;
        carGeneration = gen;
    }
    
    public CarParameters parameters;
    public CarPerformance performance;
    public int carIndex;
    public int carGeneration;
}
