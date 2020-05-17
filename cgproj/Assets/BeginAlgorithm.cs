using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginAlgorithm : MonoBehaviour
{

    public Text nbrOfGenText;
    public Text carsPerGenText;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick()
    {
        int nbrOfGen = Int32.Parse(nbrOfGenText.text);
        int carsPerGen = Int32.Parse(carsPerGenText.text);

        Debug.Log(nbrOfGen);
        Debug.Log(carsPerGen);
    }
}
