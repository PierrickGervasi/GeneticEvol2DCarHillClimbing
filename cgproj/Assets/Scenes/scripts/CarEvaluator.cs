using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEvaluator : MonoBehaviour
{
    private EvolutionManager evolutionManager;

    void Start()
    {
        var go = GameObject.Find("EvolutionManager");
        evolutionManager = go.GetComponent<EvolutionManager>();
    }

    public void TerminateEvaluation()
    {
        if (evolutionManager != null)
        {
            evolutionManager.EvaluationFinished(null);
        }
    }
}
