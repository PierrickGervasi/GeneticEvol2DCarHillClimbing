using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    public float speed = 1500;

    public WheelJoint2D backWheel;
    public WheelJoint2D frontWheel;
    
    

    private float movement = 0f;
    
    
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        backWheel.useMotor = true;
        frontWheel.useMotor = true;
        
    }
}
