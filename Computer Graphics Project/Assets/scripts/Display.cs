using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class Display : MonoBehaviour
{
    
    private Text time;
    private float distance;
    private float fitness;
    
    // Start is called before the first frame update
    void Start()
    {
        time = this.transform.Find("Time_v").gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        time.text = ((float)((int) (Time.time * 10)) / 10).ToString();
    }
}
