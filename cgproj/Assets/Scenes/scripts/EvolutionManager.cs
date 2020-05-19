using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EvolutionManager : MonoBehaviour
{
    public CarGenerator generator;

    public static  int GENERATION_SIZE = 24;
    public static  int MAX_GENERATION = 10;
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

    public List<CarData> bestCars = new List<CarData>();

    private CarData[] cars;
    private CarData bestCar;
    private CarData secondBestCar;
    public int currentCarIndex;
    public int generation;
    public int tournamentCount;
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }
 
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    void Start()
    {
        SceneManager.LoadScene("BeginningScene", LoadSceneMode.Additive);
    }

    public void OnUserClickedGo()
    {
        if (GENERATION_SIZE < 2)
        {
            GENERATION_SIZE = 2;
        }

        if (MAX_GENERATION < 2)
        {
            MAX_GENERATION = 2;
        }

        // Minimum of 4 cars per tournament
        if (GENERATION_SIZE >= 16)
        {
            tournamentCount = 4;
        }
        else
        {
            tournamentCount = 1;
        }

        cars = new CarData[GENERATION_SIZE];
        generation = 0;
        NewGeneration();

        for(int i = 0; i < GENERATION_SIZE; ++i)
        {
            CarParameters carParams = null;
            if(manualCarParams)
            {
                carParams = ScriptableObject.CreateInstance<CarParameters>();
                carParams.SetCarBody(manualBodyWidth, manualBodyHeight);
                carParams.SetCarWheel(0, manualWheel0XRatio, manualWheel0YRatio, manualWheel0Diameter, manualWheel0Motor);
                carParams.SetCarWheel(1, manualWheel1XRatio, manualWheel1YRatio, manualWheel1Diameter, manualWheel1Motor);
            }
            else
            {
                carParams = generator.GenerateRandomCar();
            }
        
            cars[i] = new CarData(carParams, i, generation);
        }

        StartCoroutine(StartEvaluationScene());
    }


    public void EvaluationFinished(CarPerformance performance) {
        Debug.Log($"{generation}-{currentCarIndex} - Evaluation Finished! distance - {performance.distance}");

        cars[currentCarIndex].performance = performance;
        
        ++currentCarIndex;

        if (currentCarIndex >= GENERATION_SIZE)
        {
            SaveBestCars();

            if (GenerateNextGeneration() == false)
            {
                StartCoroutine(EndEvaluationCycle());
                return;
            }
        }

        StartCoroutine(ReloadEvaluationScene());
    }

    
    private IEnumerator StartEvaluationScene()
    {
        var unload = SceneManager.UnloadSceneAsync("BeginningScene");
        yield return unload;
        SceneManager.LoadScene("EvaluationScene", LoadSceneMode.Additive);
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
        if (scene.name == "EvaluationScene")
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
                carTransformer.carParams = car.parameters;
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

        NewGeneration();
        
        Debug.LogWarning($"Crossover Rate - {crossoverRate}, Mutation Rate - {mutationRate}");
        
        int carsPerTournament = GENERATION_SIZE / tournamentCount; 
        var shuffledCars = cars.OrderBy(a => Guid.NewGuid()).ToList();

        for (int tournament = 0; tournament  < tournamentCount; ++tournament)
        {
            int carIndexBase = tournament*carsPerTournament;

            var tournamentCars = shuffledCars.Skip(carIndexBase).Take(carsPerTournament);
            var sortedTournament = tournamentCars.OrderByDescending(car => car.performance.TotalScore());
            var bestParent = sortedTournament.First();
            var secondBestParent = sortedTournament.Skip(1).First();

            Debug.LogWarning($"Tournament #{tournament} winners are - {bestParent.carIndex}-{bestParent.performance.TotalScore()} and {secondBestParent.carIndex}-{secondBestParent.performance.TotalScore()}");
            
            for (int i = 0; i < tournamentCars.Count(); ++i)
            {
                int crossoverPoint = CarParameters.GENE_COUNT;
                var shouldCrossOver = Random.Range(0.0f,1.0f) <= crossoverRate;
                var carParams = ScriptableObject.CreateInstance<CarParameters>();
                if (shouldCrossOver)
                {
                    crossoverPoint = Convert.ToInt32(Random.Range(0.0f, 1.0f) * (CarParameters.GENE_COUNT - 1));
                }
                for (int j = 0; j < CarParameters.GENE_COUNT; ++j)
                {
                    if (j >= crossoverPoint)
                    {
                        carParams.SetGene(j, secondBestParent.parameters.GetGene(j));
                    }
                    else
                    {
                        carParams.SetGene(j, bestParent.parameters.GetGene(j));
                    }

                    if (Random.Range(0.0f, 1.0f) <= mutationRate)
                    {
                        carParams.SetGene(j, carParams.GetGene(j).MutatedCopy());
                    }
                }
                
                cars[carIndexBase + i] = new CarData(carParams, carIndexBase + i, generation);
            }
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
    }

    private void SaveBestCars()
    {
        var sortedCars = cars.OrderByDescending(car => car.performance.TotalScore());

        bestCar = sortedCars.First();
        secondBestCar = sortedCars.ElementAt(1);

        bestCars.Add(bestCar);
        bestCars.Add(secondBestCar);
    }
}
