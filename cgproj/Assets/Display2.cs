using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Display2 : MonoBehaviour
{
    private Text nbrOfGen;
    private Text genIndex;
    private Text carsPerGen;
    private Text carIndex;

    private EvolutionManager managerScript;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject evolutionManager = GameObject.Find("EvolutionManager");
        managerScript = evolutionManager.GetComponent<EvolutionManager>();

        genIndex = this.transform.Find("genIndex_v").gameObject.GetComponent<Text>();
        carsPerGen = this.transform.Find("carsPerGen_v").gameObject.GetComponent<Text>();
        carIndex = this.transform.Find("carIndex_v").gameObject.GetComponent<Text>();

//        genIndex.text = managerScript.currentGenIndex.ToString() + "/" + managerScript.nbrOfGenerations;
//        carsPerGen.text = managerScript.GENERATION_SIZE.ToString();
        carIndex.text = ((managerScript.currentCarIndex)).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //        genIndex.text = managerScript.currentGenIndex.ToString() + "/" + managerScript.nbrOfGenerations;
//        carsPerGen.text = managerScript.GENERATION_SIZE.ToString();
        carIndex.text = ((managerScript.currentCarIndex)).ToString();
    }
}