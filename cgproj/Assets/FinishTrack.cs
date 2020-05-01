using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishTrack : MonoBehaviour
{
    public CarEvaluator evaluator;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("Car"))
        {
            evaluator.TerminateEvaluation(true);
        }
    }
}
