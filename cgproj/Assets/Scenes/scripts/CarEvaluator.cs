using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEvaluator : MonoBehaviour
{
    public Display performanceDisplay;
    private EvolutionManager evolutionManager;

    void Start()
    {
        var go = GameObject.Find("EvolutionManager");
        evolutionManager = go.GetComponent<EvolutionManager>();
    }

    public void TerminateEvaluation(bool reachedTrackEnding = false)
    {
        if (evolutionManager != null)
        {
            var totalTime = Time.time - performanceDisplay.initialTime;
            var distance = performanceDisplay.maxDistance - performanceDisplay.initialPosition;

            var performance = new CarPerformance();
            performance.distance = distance;
            performance.time = totalTime;
            performance.finishedTrack = reachedTrackEnding;

            evolutionManager.EvaluationFinished(performance);
        }
    }
}
