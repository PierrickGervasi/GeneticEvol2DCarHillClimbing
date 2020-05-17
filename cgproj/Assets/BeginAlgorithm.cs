using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginAlgorithm : MonoBehaviour
{

    public Text nbrOfGenText;
    public Text carsPerGenText;
    
    public void onClick()
    {
        int nbrOfGen = Int32.Parse(nbrOfGenText.text);
        int carsPerGen = Int32.Parse(carsPerGenText.text);

        EvolutionManager.MAX_GENERATION = nbrOfGen - 1;
        EvolutionManager.GENERATION_SIZE = carsPerGen;
        EvolutionManager.buttonGoPushed = true;
    }
}
