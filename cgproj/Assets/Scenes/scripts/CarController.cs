﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public WheelJoint2D wheel0;
    public WheelJoint2D wheel1;
    
    public CarTransformer transformer;

    
    // Start is called before the first frame update
    void Start()
    {
        wheel0.useMotor = true;
        var backWheelmotor = new JointMotor2D {motorSpeed = 0, maxMotorTorque = 80};
        wheel0.motor = backWheelmotor;
        
        wheel1.useMotor = true;
        var frontWheelMotor = new JointMotor2D {motorSpeed = 0, maxMotorTorque = 80};
        wheel1.motor = frontWheelMotor;
        
        
    }
    
    public IEnumerator StartCarMotorsDelayed()
    {
        yield return new WaitForSeconds(2);    //Wait one frame

        var carParams = transformer.carParams;

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