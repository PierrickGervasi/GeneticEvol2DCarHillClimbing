using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class podium3Manager : MonoBehaviour
{
    private EvolutionManager managerScript;

    public List<CarData> bestCars;

    public GameObject podium3Cars;
    public GameObject podium3Array;

    private List<CarData> podiumCars = new List<CarData>();

    private List<Text> fitnessText = new List<Text>();
    private List<Text> distanceText = new List<Text>();
    private List<Text> finishedText = new List<Text>();
    private List<Text> timeText = new List<Text>();
    private List<Text> avgSpeedText = new List<Text>();
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject evolutionManager = GameObject.Find("EvolutionManager");
        managerScript = evolutionManager.GetComponent<EvolutionManager>();
        bestCars = managerScript.bestCars;

        podiumCars = findPodiumCars();
        transformPodiumCars();

        findTextObjects();
        transformTextObjects();
    }
    
    private List<CarData> findPodiumCars()
    {
        List<CarData> podiumCars = new List<CarData>();

        var sortedCars = bestCars.OrderByDescending(car => car.performance.TotalScore());

        podiumCars.Add(sortedCars.First());
        podiumCars.Add(sortedCars.ElementAt(1));
        podiumCars.Add(sortedCars.ElementAt(2));

        return podiumCars;
    }
    
    private void transformPodiumCars()
    {
        for (int i = 0; i < podium3Cars.transform.childCount; i++)
        {
            var carTransformer = podium3Cars.transform.GetChild(i).gameObject.GetComponent<CarTransformer>();
            carTransformer.carParams = podiumCars.ElementAt(i).parameters;
            carTransformer.TransformByCarParams();
            
            var carController =  podium3Cars.transform.GetChild(i).gameObject.GetComponent<CarController>();
            carController.StartCarMotors();
        }
        
        
    }

    private void findTextObjects()
    {
        fitnessText.Add(podium3Array.transform.Find("Fitness (1)").gameObject.GetComponent<Text>());
        fitnessText.Add(podium3Array.transform.Find("Fitness (2)").gameObject.GetComponent<Text>());
        fitnessText.Add(podium3Array.transform.Find("Fitness (3)").gameObject.GetComponent<Text>());
        
        distanceText.Add(podium3Array.transform.Find("Distance (1)").gameObject.GetComponent<Text>());
        distanceText.Add(podium3Array.transform.Find("Distance (2)").gameObject.GetComponent<Text>());
        distanceText.Add(podium3Array.transform.Find("Distance (3)").gameObject.GetComponent<Text>());
        
        finishedText.Add(podium3Array.transform.Find("Finished (1)").gameObject.GetComponent<Text>());
        finishedText.Add(podium3Array.transform.Find("Finished (2)").gameObject.GetComponent<Text>());
        finishedText.Add(podium3Array.transform.Find("Finished (3)").gameObject.GetComponent<Text>());
        
        timeText.Add(podium3Array.transform.Find("Time (1)").gameObject.GetComponent<Text>());
        timeText.Add(podium3Array.transform.Find("Time (2)").gameObject.GetComponent<Text>());
        timeText.Add(podium3Array.transform.Find("Time (3)").gameObject.GetComponent<Text>());
        
        avgSpeedText.Add(podium3Array.transform.Find("Avg speed (1)").gameObject.GetComponent<Text>());
        avgSpeedText.Add(podium3Array.transform.Find("Avg speed (2)").gameObject.GetComponent<Text>());
        avgSpeedText.Add(podium3Array.transform.Find("Avg speed (3)").gameObject.GetComponent<Text>());
    }

    private void transformTextObjects()
    {
        for (int i = 0; i < podiumCars.Count; i++)
        {
            fitnessText[i].text = ((float)((int) ((podiumCars[i].performance.TotalScore()) * 10)) / 10).ToString();
            distanceText[i].text = ((float)((int) ((podiumCars[i].performance.distance) * 10)) / 10).ToString();
            timeText[i].text = ((float)((int) ((podiumCars[i].performance.time) * 10)) / 10).ToString();
            avgSpeedText[i].text = ((float)((int) ((podiumCars[i].performance.averageSpeed) * 10)) / 10).ToString();
            
            if (podiumCars[i].performance.finishedTrack)
            {
                finishedText[i].text = "Yes";
            }
            else
            {
                finishedText[i].text = "No";
            }

        }
    }

}