﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class EvolutionManager : MonoBehaviour
{
    public CarGenerator generator;

    public static readonly int GENERATION_SIZE = 10;
    public static readonly float crossoverRate = 0.9f;
    public static readonly float mutationRate = 0.06f;

    private CarParameters[] cars = new CarParameters[GENERATION_SIZE];
    private CarPerformance[] carsPerformance = new CarPerformance[GENERATION_SIZE];
    private int bestCar;
    private int secondBestCar;
    private int currentCarIndex;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }
 
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    // Start is called before the first frame update
    void Start()
    {
        NewGeneration();

        for(int i = 0; i < GENERATION_SIZE; ++i)
        {
            var car = generator.GenerateRandomCar();
            cars[i] = car;
        }

        SceneManager.LoadScene("EvaluationScene", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EvaluationFinished(CarPerformance performance) {
        Debug.Log($"Evaluation Finished! distance - {performance.distance}");
        if (currentCarIndex != 0)
        {
            if (performance.TotalScore() > carsPerformance[bestCar].TotalScore())
            {
                secondBestCar = bestCar;
                bestCar = currentCarIndex;
            }
            else if(performance.TotalScore() > carsPerformance[secondBestCar].TotalScore())
            {
                secondBestCar = currentCarIndex;
            }
        }

        carsPerformance[currentCarIndex] = performance;
        ++currentCarIndex;

        if (currentCarIndex >= GENERATION_SIZE)
        {
            Debug.Log("Generation done evaluating!");
            GenerateNextGeneration();
            return;
        }

        StartCoroutine(ReloadEvaluationScene());
    }

    private IEnumerator ReloadEvaluationScene()
    {
        var unload = SceneManager.UnloadSceneAsync("EvaluationScene");
        yield return unload;
        SceneManager.LoadScene("EvaluationScene", LoadSceneMode.Additive);
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainScene")
        {
            if (currentCarIndex >= GENERATION_SIZE)
            {
                Debug.Log("Generation done evaluating!");
                return;
            }

            var car = cars[currentCarIndex];
            
            // Load car
            var carGameObj = GameObject.Find("Car");
            if (carGameObj)
            {
                var carTransformer = carGameObj.GetComponent<CarTransformer>();
                carTransformer.carParams = car;
                carTransformer.TransformByCarParams();

                // Start car
                var carController =  carGameObj.GetComponent<CarController>();
                StartCoroutine(carController.StartCarMotorsDelayed());
            }
            else
            {
                Debug.LogError("Could not get Car game object!");
            }
        }
    }

    private void GenerateNextGeneration()
    {
        var bestParent = cars[bestCar];
        var secondBestParent = cars[secondBestCar];

        Debug.LogWarning($"New Generation from {carsPerformance[bestCar].TotalScore()} and {carsPerformance[secondBestCar].TotalScore()}");

        NewGeneration();
        
        for (int i = 0; i < GENERATION_SIZE; ++i)
        {
            int crossoverPoint = CarParameters.GENE_COUNT;
            var shouldCrossOver = Random.Range(0.0f,1.0f) <= crossoverRate;
            var car = ScriptableObject.CreateInstance<CarParameters>();
            if (shouldCrossOver)
            {
                crossoverPoint = Convert.ToInt32(Random.Range(0.0f, 1.0f) * (CarParameters.GENE_COUNT - 1));
            }
            Debug.Log($"crossover at {crossoverPoint}");
            for (int j = 0; j < CarParameters.GENE_COUNT; ++j)
            {
                if (j >= crossoverPoint)
                {
                    car.SetGene(j, secondBestParent.GetGene(j));
                }
                else
                {
                    car.SetGene(j, bestParent.GetGene(j));
                }

                if (Random.Range(0.0f, 1.0f) <= mutationRate)
                {
                    car.SetGene(j, car.GetGene(j).MutatedCopy());
                }
            }

            cars[i] = car;
        }
    }

    private void NewGeneration()
    {
        currentCarIndex = 0;
        bestCar = 0;
        secondBestCar = 0;
    }
}