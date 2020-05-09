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

        genIndex = transform.Find("genIndex_v").gameObject.GetComponent<Text>();
        carsPerGen = transform.Find("carsPerGen_v").gameObject.GetComponent<Text>();
        carIndex = transform.Find("carIndex_v").gameObject.GetComponent<Text>();

        genIndex.text = (managerScript.generation+1) + "/" + (EvolutionManager.MAX_GENERATION + 1);
        carsPerGen.text = EvolutionManager.GENERATION_SIZE.ToString();
        carIndex.text = (managerScript.currentCarIndex+1) + "/" + EvolutionManager.GENERATION_SIZE;
    }

    // Update is called once per frame
    void Update()
    {
        genIndex.text = (managerScript.generation+1) + "/" + (EvolutionManager.MAX_GENERATION + 1);
        carsPerGen.text = EvolutionManager.GENERATION_SIZE.ToString();
        carIndex.text = (managerScript.currentCarIndex+1) + "/" + EvolutionManager.GENERATION_SIZE;
    }
}