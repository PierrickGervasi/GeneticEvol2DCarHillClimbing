using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    public bool useBothWheels;
    
    public WheelJoint2D backWheel;
    public WheelJoint2D frontWheel;
    
    private WheelJoint2D other;

    
    // Start is called before the first frame update
    void Start()
    {
        
        backWheel.useMotor = true;
        var backWheelmotor = new JointMotor2D {motorSpeed = 0, maxMotorTorque = 80};
        backWheel.motor = backWheelmotor;
        
        frontWheel.useMotor = true;
        var frontWheelMotor = new JointMotor2D {motorSpeed = 0, maxMotorTorque = 80};
        frontWheel.motor = frontWheelMotor;
        
        StartCoroutine(MyCoroutine());
    }
    
    IEnumerator MyCoroutine()
    {
        yield return new WaitForSeconds(2);    //Wait one frame


        if (useBothWheels)
        {
            frontWheel.useMotor = true;
            var frontWheelMotor = new JointMotor2D {motorSpeed = 1000, maxMotorTorque = 70};
            frontWheel.motor = frontWheelMotor;
            
            backWheel.useMotor = true;
            var backWheelmotor = new JointMotor2D {motorSpeed = 1000, maxMotorTorque = 70};
            backWheel.motor = backWheelmotor;
        }

        else
        {
            frontWheel.useMotor = false;
            backWheel.useMotor = false;

            if (Random.value > 0.5)
            {
                frontWheel.useMotor = true;
                var frontWheelMotor = new JointMotor2D {motorSpeed = 1000, maxMotorTorque = 70};
                frontWheel.motor = frontWheelMotor;
                other = backWheel;
            }
            else
            {
                backWheel.useMotor = true;
                var backWheelmotor = new JointMotor2D {motorSpeed = 1000, maxMotorTorque = 70};
                backWheel.motor = backWheelmotor;
                other = frontWheel;
            }

            if (Random.value > 0.5)
            {
                other.useMotor = true;
                var otherMotor = new JointMotor2D {motorSpeed = 1000, maxMotorTorque = 70};
                other.motor = otherMotor;        
            }
            else
            {
                other.useMotor = false;
            }
        }
    }
    
}
