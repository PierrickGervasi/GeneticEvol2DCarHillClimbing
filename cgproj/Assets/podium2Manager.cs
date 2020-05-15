using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class podium2Manager : MonoBehaviour
{
    private EvolutionManager managerScript;
    
    public List<CarData> bestCars;

    public GameObject podium2Cars;
    public GameObject podium2Array;

    private List<CarData> podiumCars = new List<CarData>();

    private List<Text> fitnessText = new List<Text>();
    private List<Text> distanceText = new List<Text>();
    private List<Text> finishedText = new List<Text>();
    private List<Text> timeText = new List<Text>();
    private List<Text> avgSpeedText = new List<Text>();
    
    public Dropdown dropdown;

    // Start is called before the first frame update
    void Start()
    {
        GameObject evolutionManager = GameObject.Find("EvolutionManager");
        managerScript = evolutionManager.GetComponent<EvolutionManager>();
        bestCars = managerScript.bestCars;

        podium2Cars.SetActive(false);

        addOptionsToDropDownMenu();
    }

    private void addOptionsToDropDownMenu()
    {
        List<String> options = new List<string>();
        
        options.Add("Choose a generation...");

        for (int i = 0; i < EvolutionManager.MAX_GENERATION+1; i++)
        {
            options.Add("Generation "+i);
        }

        dropdown.AddOptions(options);
    }

    public void onDropdownIndexChanged(int index)
    {
        if (index == 0)
        {
            podium2Cars.SetActive(false);
            initializeTextObjects();
        }
        else
        {
            podium2Cars.SetActive(false);
            podium2Cars.SetActive(true);
            
            podiumCars = findPodiumCars(index);
            transformPodiumCars();

            findTextObjects();
            transformTextObjects();

        }
    }
    

    private List<CarData> findPodiumCars(int generation)
    {
        List<CarData> podiumCars = new List<CarData>();
        
        podiumCars.Add(bestCars[(generation-1)*2]);
        podiumCars.Add(bestCars[(generation-1)*2+1]);
        
        return podiumCars;
    }
    
    private void transformPodiumCars()
    {
        for (int i = 0; i < podium2Cars.transform.childCount; i++)
        {
            var carTransformer = podium2Cars.transform.GetChild(i).gameObject.GetComponent<CarTransformer>();
            carTransformer.carParams = podiumCars.ElementAt(i).parameters;
            carTransformer.TransformByCarParams();
            
            var carController =  podium2Cars.transform.GetChild(i).gameObject.GetComponent<CarController>();
            carController.StartCarMotors();
        }
    }

    private void findTextObjects()
    {
        fitnessText.Add(podium2Array.transform.Find("Fitness (1)").gameObject.GetComponent<Text>());
        fitnessText.Add(podium2Array.transform.Find("Fitness (2)").gameObject.GetComponent<Text>());
        
        distanceText.Add(podium2Array.transform.Find("Distance (1)").gameObject.GetComponent<Text>());
        distanceText.Add(podium2Array.transform.Find("Distance (2)").gameObject.GetComponent<Text>());
        
        finishedText.Add(podium2Array.transform.Find("Finished (1)").gameObject.GetComponent<Text>());
        finishedText.Add(podium2Array.transform.Find("Finished (2)").gameObject.GetComponent<Text>());
        
        timeText.Add(podium2Array.transform.Find("Time (1)").gameObject.GetComponent<Text>());
        timeText.Add(podium2Array.transform.Find("Time (2)").gameObject.GetComponent<Text>());
        
        avgSpeedText.Add(podium2Array.transform.Find("Avg speed (1)").gameObject.GetComponent<Text>());
        avgSpeedText.Add(podium2Array.transform.Find("Avg speed (2)").gameObject.GetComponent<Text>());
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
    
    private void initializeTextObjects()
    {
        for (int i = 0; i < podiumCars.Count; i++)
        {
            fitnessText[i].text = "0";
            distanceText[i].text = "0";
            timeText[i].text ="0";
            avgSpeedText[i].text = "0";
            finishedText[i].text = "No";
        }

    }

}
