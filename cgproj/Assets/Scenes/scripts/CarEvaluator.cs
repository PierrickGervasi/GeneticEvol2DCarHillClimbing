using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEvaluator : MonoBehaviour
{
    public Display performanceDisplay;
    private EvolutionManager evolutionManager;

    private bool evaluationTerminated;

    void Start()
    {
        evaluationTerminated = false;
        var go = GameObject.Find("EvolutionManager");
        evolutionManager = go.GetComponent<EvolutionManager>();
    }

    public void TerminateEvaluation(bool reachedTrackEnding = false)
    {
        if (evolutionManager != null && evaluationTerminated == false)
        {
            evaluationTerminated = true;
            
            var totalTime = Time.time - performanceDisplay.initialTime;
            var distance = performanceDisplay.maxDistance - performanceDisplay.initialPosition;

            var performance = ScriptableObject.CreateInstance<CarPerformance>();
            performance.distance = distance;
            performance.time = totalTime;
            performance.finishedTrack = reachedTrackEnding;
            performance.averageSpeed = distance / totalTime;

            evolutionManager.EvaluationFinished(performance);
        }
    }
}
