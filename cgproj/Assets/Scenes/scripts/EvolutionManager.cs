using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EvolutionManager : MonoBehaviour
{
    private List<CarParameters> cars = new List<CarParameters>();
    private List<CarPerformance> carsPerformance = new List<CarPerformance>();

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
        SceneManager.LoadScene("EvaluationScene", LoadSceneMode.Additive);

        // Generate random cars
        // Evaluation loop
        // Load track
        // Evaluate car
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EvaluationFinished(CarPerformance performance) {
        Debug.Log("Evaluation Finished!");
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
            Debug.Log("Scene: " + scene.name + " Loaded!");
            //player = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
