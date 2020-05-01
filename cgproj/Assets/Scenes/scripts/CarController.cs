using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public WheelJoint2D wheel0;
    public WheelJoint2D wheel1;
    
    public CarGenerator generator;

    
    // Start is called before the first frame update
    void Start()
    {
        generator.GenerateRandomCar();

        wheel0.useMotor = true;
        var backWheelmotor = new JointMotor2D {motorSpeed = 0, maxMotorTorque = 80};
        wheel0.motor = backWheelmotor;
        
        wheel1.useMotor = true;
        var frontWheelMotor = new JointMotor2D {motorSpeed = 0, maxMotorTorque = 80};
        wheel1.motor = frontWheelMotor;
        
        StartCoroutine(MyCoroutine());
    }
    
    IEnumerator MyCoroutine()
    {
        yield return new WaitForSeconds(2);    //Wait one frame

        var carParams = generator.carParams;

        if (carParams.GetWheel(0).hasMotor)
        {
            wheel0.useMotor = true;
            var backWheelmotor = new JointMotor2D {motorSpeed = 1000, maxMotorTorque = 70};
            wheel0.motor = backWheelmotor;
        }

        if (carParams.GetWheel(1).hasMotor)
        {
            wheel1.useMotor = true;
            var backWheelmotor = new JointMotor2D {motorSpeed = 1000, maxMotorTorque = 70};
            wheel1.motor = backWheelmotor;
        }
    }
    
}
