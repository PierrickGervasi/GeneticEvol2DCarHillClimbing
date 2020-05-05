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
        var body = carParams.GetCarBody();
        var wheel0 = carParams.GetWheel(0);
        var wheel1 = carParams.GetWheel(1);
        
        this.wheel0.transform.localScale = new Vector3(wheel0.diameter, wheel0.diameter, 1);
        this.wheel1.transform.localScale = new Vector3(wheel1.diameter, wheel1.diameter, 1);

        this.wheel0.transform.localPosition = WheelPositionFromWheelRatios(wheel0.xRatio, wheel0.yRatio, body);
        this.wheel1.transform.localPosition = WheelPositionFromWheelRatios(wheel1.xRatio, wheel1.yRatio, body);

        WheelJoint2D frontWheelJoint = this.wheel0.GetComponent<WheelJoint2D>();
        WheelJoint2D backWheelJoint = this.wheel1.GetComponent<WheelJoint2D>();
        frontWheelJoint.connectedAnchor = new Vector2(this.wheel0.transform.localPosition.x, this.wheel0.transform.localPosition.y);
        backWheelJoint.connectedAnchor = new Vector2(this.wheel1.transform.localPosition.x, this.wheel1.transform.localPosition.y);
    }

    private Vector3 WheelPositionFromWheelRatios(float xRatio, float yRatio, CarBody body)
    {
        float xPosition = 0.0f;
        float yPosition = 0.0f;

        if (xRatio <= 0.5f)
        {
            xPosition = (0.5f - xRatio) * -body.width; 
        } else
        {
            xPosition = (xRatio - 0.5f) * body.width;
        }

        if (yRatio <= 0.5f)
        {
            yPosition = (0.5f - yRatio) * -body.height; 
        } else
        {
            yPosition = (yRatio - 0.5f) * body.height;
        }

        return new Vector3(xPosition, yPosition, 0);
    }
}
