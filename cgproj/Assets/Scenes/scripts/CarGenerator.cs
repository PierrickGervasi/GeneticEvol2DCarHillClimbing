using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGenerator : MonoBehaviour
{
    public CarParameters GenerateRandomCar()
    {
        var carParams = ScriptableObject.CreateInstance<CarParameters>();
        
        PopulateCarBodyParams(carParams);
        PopulateCarWheelParams(carParams);
        
        return carParams;
    }
    
    private void PopulateCarBodyParams(CarParameters carParams)
    {
        float carBodyWidth = Random.Range(CarBody.MINIMUM, CarBody.MAXIMUM);
        float carBodyHeight = Random.Range(CarBody.MINIMUM, CarBody.MAXIMUM);

        carParams.SetCarBody(carBodyWidth, carBodyHeight);        
    }

    private void PopulateCarWheelParams(CarParameters carParams)
    {
        float Wheel0Diameter = Random.Range(CarWheel.DIAMETER_MINIMUM, CarWheel.DIAMETER_MAXIMUM);
        float Wheel1Diameter = Random.Range(CarWheel.DIAMETER_MINIMUM, CarWheel.DIAMETER_MAXIMUM);

        float Wheel0RatioX = Random.Range(CarWheel.RATIO_MINIMUM, CarWheel.RATIO_MAXIMUM);
        float Wheel0RatioY = Random.Range(CarWheel.RATIO_MINIMUM, CarWheel.RATIO_MAXIMUM);
        float Wheel1RatioX = Random.Range(CarWheel.RATIO_MINIMUM, CarWheel.RATIO_MAXIMUM);
        float Wheel1RatioY = Random.Range(CarWheel.RATIO_MINIMUM, CarWheel.RATIO_MAXIMUM);

        bool wheel0motor = Random.Range(0.0f,1.0f) > 0.5f;
        bool wheel1motor = Random.Range(0.0f,1.0f) > 0.5f;

        if ((wheel0motor & wheel1motor) == false)
        {
            if(Random.Range(0.0f, 1.0f) > 0.5f)
            {
                wheel0motor = true;
            }
            else
            {
                wheel1motor = true;
            }
        }

        carParams.SetCarWheel(0, Wheel0RatioX, Wheel0RatioY, Wheel0Diameter, wheel0motor);
        carParams.SetCarWheel(1, Wheel1RatioX, Wheel1RatioY, Wheel1Diameter, wheel1motor);
    }
}
