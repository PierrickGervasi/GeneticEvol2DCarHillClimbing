using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class EvolutionManager : MonoBehaviour
{
    public CarGenerator generator;

    public static readonly int GENERATION_SIZE = 20;
    public static readonly int MAX_GENERATION = 8;
    public static readonly float STATIC_CROSSOVER_RATE = 0.9f;
    public static readonly float STATIC_MUTATION_RATE = 0.15f;

    public bool dynamicRates = true;

    public float mutationRate = 0.0f;
    public float crossoverRate = 1.0f;

    public bool manualCarParams = false;
    public float manualBodyWidth = 2.5f;
    public float manualBodyHeight = 1.5f;
    public float manualWheel0Diameter = 1.2f;
    public float manualWheel0XRatio = 0.1f;
    public float manualWheel0YRatio = 0.1f;
    public bool manualWheel0Motor = true;
    public float manualWheel1Diameter = 1.2f;
    public float manualWheel1XRatio = 0.9f;
    public float manualWheel1YRatio = 0.1f;
    public bool manualWheel1Motor = false;

    private CarParameters[] cars = new CarParameters[GENERATION_SIZE];
    private CarPerformance[] carsPerformance = new CarPerformance[GENERATION_SIZE];
    private int bestCar;
    private int secondBestCar;
    public int currentCarIndex;
    public int generation;

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
        generation = 0;
        NewGeneration();

        for(int i = 0; i < GENERATION_SIZE; ++i)
        {
            CarParameters car = null;
            if(manualCarParams)
            {
                car = ScriptableObject.CreateInstance<CarParameters>();
                car.SetCarBody(manualBodyWidth, manualBodyHeight);
                car.SetCarWheel(0, manualWheel0XRatio, manualWheel0YRatio, manualWheel0Diameter, manualWheel0Motor);
                car.SetCarWheel(1, manualWheel1XRatio, manualWheel1YRatio, manualWheel1Diameter, manualWheel1Motor);
            }
            else
            {
                car = generator.GenerateRandomCar();
            }
            
            cars[i] = car;
        }

        SceneManager.LoadScene("EvaluationScene", LoadSceneMode.Additive);
    }
    

    public void EvaluationFinished(CarPerformance performance) {
        Debug.Log($"{generation}-{currentCarIndex} - Evaluation Finished! distance - {performance.distance}");

        carsPerformance[currentCarIndex] = performance;

        if (currentCarIndex != 0)
        {
            if (performance.TotalScore() >= carsPerformance[bestCar].TotalScore())
            {
                secondBestCar = bestCar;
                bestCar = currentCarIndex;
            }
            else if(performance.TotalScore() > carsPerformance[secondBestCar].TotalScore())
            {
                secondBestCar = currentCarIndex;
            }
        }
        
        ++currentCarIndex;

        if (currentCarIndex >= GENERATION_SIZE)
        {
            if (GenerateNextGeneration() == false)
            {
                StartCoroutine(EndEvaluationCycle());
                return;
            }
        }

        StartCoroutine(ReloadEvaluationScene());
    }

    private IEnumerator ReloadEvaluationScene()
    {
        var unload = SceneManager.UnloadSceneAsync("EvaluationScene");
        yield return unload;
        SceneManager.LoadScene("EvaluationScene", LoadSceneMode.Additive);
    }

    private IEnumerator EndEvaluationCycle()
    {
        var unload = SceneManager.UnloadSceneAsync("EvaluationScene");
        yield return unload;
        SceneManager.LoadScene("EndScene", LoadSceneMode.Additive);
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

    private bool GenerateNextGeneration()
    {
        ++generation;
        if (generation > MAX_GENERATION)
        {
            // No next generation
            return false;
        }

        var bestParent = cars[bestCar];
        var secondBestParent = cars[secondBestCar];

        Debug.LogWarning($"New Generation from {bestCar}-{carsPerformance[bestCar].TotalScore()} and {secondBestCar}-{carsPerformance[secondBestCar].TotalScore()}");

        NewGeneration();
        
        Debug.LogWarning($"Crossover Rate - {crossoverRate}, Mutation Rate - {mutationRate}");
        
        
        for (int i = 0; i < GENERATION_SIZE; ++i)
        {
            int crossoverPoint = CarParameters.GENE_COUNT;
            var shouldCrossOver = Random.Range(0.0f,1.0f) <= crossoverRate;
            var car = ScriptableObject.CreateInstance<CarParameters>();
            if (shouldCrossOver)
            {
                crossoverPoint = Convert.ToInt32(Random.Range(0.0f, 1.0f) * (CarParameters.GENE_COUNT - 1));
            }
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

        return true;
    }

    private void NewGeneration()
    {
        if (dynamicRates)
        {
            mutationRate = (float) generation / MAX_GENERATION;
            crossoverRate = 1.0f - mutationRate;
        }
        else
        {
            // static crossover rates
            mutationRate = STATIC_MUTATION_RATE;
            crossoverRate = STATIC_CROSSOVER_RATE;
        }
        

        currentCarIndex = 0;
        bestCar = 0;
        secondBestCar = 1;
    }
}
