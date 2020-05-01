using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTransformer : MonoBehaviour
{

    public GameObject wheel0;
    public GameObject wheel1;
    public GameObject carBody;

    public CarParameters carParams;


    public void TransformByCarParams()
    {   
        ApplyCarBodyParams();
        ApplyCarWheelParams();
    }

    private void ApplyCarBodyParams()
    {
        var body = carParams.GetCarBody();
        
        carBody.transform.localScale = new Vector3(body.width, body.height, 1);
    }

    private void ApplyCarWheelParams()
    {
        var wheel0 = carParams.GetWheel(0);
        var wheel1 = carParams.GetWheel(1);
        
        this.wheel0.transform.localScale = new Vector3(wheel0.diameter, wheel0.diameter, 1);
        this.wheel1.transform.localScale = new Vector3(wheel1.diameter, wheel1.diameter, 1);

        this.wheel0.transform.localPosition = new Vector3(wheel0.xPosition, wheel0.yPosition, 0);
        this.wheel1.transform.localPosition = new Vector3(wheel1.xPosition, wheel1.yPosition, 0);

        WheelJoint2D frontWheelJoint = this.wheel0.GetComponent<WheelJoint2D>();
        WheelJoint2D backWheelJoint = this.wheel1.GetComponent<WheelJoint2D>();
        frontWheelJoint.connectedAnchor = new Vector2(wheel0.xPosition, wheel0.yPosition);
        backWheelJoint.connectedAnchor = new Vector2(wheel1.xPosition, wheel1.yPosition);
    }
}
