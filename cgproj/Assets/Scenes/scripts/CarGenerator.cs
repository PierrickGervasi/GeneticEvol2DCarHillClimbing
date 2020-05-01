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

        var carBodyParams = carParams.GetCarBody();
        float bodyWidth = carBodyParams.width / 2;
        float bodyHeight = carBodyParams.height / 2;

        float Wheel0PositionX = Random.Range(-bodyWidth, bodyWidth);
        float Wheel0PositionY = Random.Range(-bodyHeight, bodyHeight);
        float Wheel1PositionX = Random.Range(-bodyWidth, bodyWidth);
        float Wheel1PositionY = Random.Range(-bodyHeight, bodyHeight);

        carParams.SetCarWheel(0, Wheel0PositionX, Wheel0PositionY, Wheel0Diameter, true);
        carParams.SetCarWheel(1, Wheel1PositionX, Wheel1PositionY, Wheel1Diameter, true);
    }
}
