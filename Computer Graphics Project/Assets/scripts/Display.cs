using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Display : MonoBehaviour
{
    public GameObject car;
    
    private Text time;
    private Text maxDistanceText;
    private Text fitness;

    private float initialPosition;
    private float maxDistance;

    private float initialTime;

    private float lastCheckTime;
    private float timeBeforeRestart;

    private float latestMaxDistance;
    
    // Start is called before the first frame update
    void Start()
    {
        time = this.transform.Find("Time_v").gameObject.GetComponent<Text>();
        maxDistanceText = this.transform.Find("Distance_v").gameObject.GetComponent<Text>();

        initialTime = Time.time;
        initialPosition = car.transform.position.x;

        maxDistanceText.text = 0.ToString();
        maxDistance = 0;

        lastCheckTime = Time.time;
        timeBeforeRestart = 5;

    }

    // Update is called once per frame
    void Update()
    {
        time.text = ((float)((int) ((Time.time - initialTime) * 10)) / 10).ToString();

        float carPositionX = car.transform.position.x;
        
        if (((float) ((int) ((carPositionX) * 10)) / 10) > maxDistance)
        {
            Debug.Log("yes");
            maxDistance = ((float) ((int) ((carPositionX) * 10)) / 10);
            maxDistanceText.text = ((float)((int) ((carPositionX - initialPosition) * 10)) / 10).ToString();
            
            lastCheckTime = Time.time;
            latestMaxDistance = maxDistance;
        }
        
        if ((Time.time - lastCheckTime) > timeBeforeRestart)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
    }
}
