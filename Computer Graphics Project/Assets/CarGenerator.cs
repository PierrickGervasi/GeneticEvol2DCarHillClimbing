using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGenerator : MonoBehaviour
{

    public GameObject frontWheel;
    public GameObject backWheel;
    public GameObject carBody;

    private GameObject other;
    
    
    // Start is called before the first frame update
    void Start()
    {

        // WHEELS AND BODY SCALES
        float frontWheelScale = Random.Range(0.5f, 1.5f);
        float backWheelScale = Random.Range(0.5f, 1.5f);
        float carBodyScaleX = Random.Range(0.5f, 1.5f);
        float carBodyScaleY = Random.Range(0.5f, 1.5f);

        
        frontWheel.transform.localScale = new Vector3(frontWheelScale, frontWheelScale, 1);
        backWheel.transform.localScale = new Vector3(backWheelScale, backWheelScale, 1);
        carBody.transform.localScale = new Vector3(carBodyScaleX, carBodyScaleY, 1);

        
        // PUT THE WHEELS RANDOMLY
        var bodySize = carBody.GetComponent<SpriteRenderer>().bounds.size;
        float bodyWidth = bodySize.x;
        float bodyHeight = bodySize.y;

        WheelJoint2D frontWheelJoint = frontWheel.GetComponent<WheelJoint2D>();
        WheelJoint2D backWheelJoint = frontWheel.GetComponent<WheelJoint2D>();
        
        float frontWheelPositionX = Random.Range(-bodyWidth, bodyWidth);
        float frontWheelPositionY = Random.Range(-bodyHeight, bodyHeight);
        float backWheelPositionX = Random.Range(-bodyWidth, bodyWidth);
        float backWheelPositionY = Random.Range(-bodyHeight, bodyHeight);


        frontWheel.transform.localPosition = new Vector3(frontWheelPositionX, frontWheelPositionY, 0);
        backWheel.transform.localPosition = new Vector3(backWheelPositionX, backWheelPositionY, 0);
        
        frontWheelJoint.connectedAnchor = new Vector2(frontWheelPositionX, frontWheelPositionY);
        backWheelJoint.connectedAnchor = new Vector2(backWheelPositionX, backWheelPositionY);
        
    }
}
