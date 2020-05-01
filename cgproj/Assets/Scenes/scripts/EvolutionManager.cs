using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EvolutionManager : MonoBehaviour
{
    public CarGenerator generator;

    public static readonly int GENERATION_SIZE = 8;

    private CarParameters[] cars = new CarParameters[GENERATION_SIZE];
    private CarPerformance[] carsPerformance = new CarPerformance[GENERATION_SIZE];
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
        currentCarIndex = 0;

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
        carsPerformance[currentCarIndex] = performance;
        ++currentCarIndex;

        if (currentCarIndex >= GENERATION_SIZE)
        {
            Debug.Log("Generation done evaluating!");
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
}
