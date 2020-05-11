using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Display : MonoBehaviour
{
    public GameObject car;

    public CarEvaluator evaluator;

    private Rigidbody2D carRigidbody;
    
    public float maxDistance;
    public float initialTime;
    public float initialPosition;

    private Text time;
    private Text maxDistanceText;
    private Text fitness;

    
    // RESTART CONDITION WHEN CAR STUCK IN LOOP
    public float secBeforeRestartWhenLoop = 20.0f;
    private float lastCheckTimeLoop;

    
    // RESTART CONDITION WHEN CAR CAN'T MOVE
    public int secBeforeRestartWhenStuck = 5;
    public float velocityCondition = 0.2f;
    private float[] velocityValues;
    private float lastCheckTimeStuck;
    private int index;
    private int nbLowVelocities;

    // Start is called before the first frame update
    void Start()
    {
        carRigidbody = car.GetComponent<Rigidbody2D>();
        
        time = this.transform.Find("Time_v").gameObject.GetComponent<Text>();
        maxDistanceText = this.transform.Find("Distance_v").gameObject.GetComponent<Text>();

        initialTime = Time.time;
        initialPosition = car.transform.position.x;

        maxDistanceText.text = 0.ToString();
        maxDistance = 0;

        lastCheckTimeLoop = Time.time;
        lastCheckTimeStuck = Time.time;

        velocityValues = new float [secBeforeRestartWhenStuck];
        index = 0;
        nbLowVelocities = 0;

        for (int i = 0; i < secBeforeRestartWhenStuck; i++)
        {
            velocityValues[i] = velocityCondition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        time.text = ((float)((int) ((Time.time - initialTime) * 10)) / 10).ToString();

        
        
        // RESTART CONDITION FOR LOOPING
        float carPositionX = car.transform.position.x;
        
        if (((float) ((int) ((carPositionX) * 10)) / 10) > maxDistance+0.1f)
        {
            maxDistance = ((float) ((int) ((carPositionX) * 10)) / 10);
            maxDistanceText.text = ((float)((int) ((carPositionX - initialPosition) * 10)) / 10).ToString();
            
            lastCheckTimeLoop = Time.time;
        }
        
        if ((Time.time - lastCheckTimeLoop) > secBeforeRestartWhenLoop)
        {
            TerminateEvaluation();
        }
        
        //RESTART CONDITION WHEN STUCK

        if (Time.time - initialTime > 2)
        {
            if (Time.time - lastCheckTimeStuck >= 1)
            {
                velocityValues[index] = carRigidbody.velocity.magnitude;

                if (index >= secBeforeRestartWhenStuck-1)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }

                lastCheckTimeStuck = Time.time;
            }

            foreach (float velocity in velocityValues)
            {
                if (velocity < velocityCondition)
                {
                    nbLowVelocities++;
                }
            }
        
            if (nbLowVelocities == secBeforeRestartWhenStuck)
            {
                TerminateEvaluation();
            }
            else
            {
                nbLowVelocities = 0;
            }
        }
    }

    private void TerminateEvaluation()
    {
        evaluator.TerminateEvaluation();
    }
}
