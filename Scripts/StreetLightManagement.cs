using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StreetLightManagement : MonoBehaviour
{
    [SerializeField] private GameObject spotLight;

    List<GameObject> allStreetLightObjects = new List<GameObject>();
    LightingManager lightingManager;
    Dictionary<float, string> timeMap;
    float currentTimeOfDay;
    string currentTimeString = "9:00 AM";
    [SerializeField] Text clock;
    void Start() {
        // KillAllSpotLights();
        SpawnSpotLights();
        lightingManager = transform.gameObject.GetComponent<LightingManager>();
        currentTimeOfDay = 0f;  
        
    }
    // Update is called once per frame
    void Update()
    {
        currentTimeOfDay = lightingManager.GetTimeOfDay();

        if (currentTimeOfDay < 10f || currentTimeOfDay >= 22f) {
            TurnOnStreetLights();
        }
        else {
            TurnOffStreetLights();
        }
        convertTimeToString();

        // 11 = 6am
        // 14 = 9am
        // 17 = 12pm
        // 21 = 3pm
        // 24 = 6pm
        // 2 = 9pm
        // 5 = 12am
        // 8 = 3am



    }

    void convertTimeToString() {

        if (currentTimeOfDay >= 24 || currentTimeOfDay < 2) {
            currentTimeString = "6:00 PM";
        }
        else if (currentTimeOfDay >= 2 && currentTimeOfDay < 5) {
            currentTimeString = "9:00 PM";
        }
        else if (currentTimeOfDay >= 5 && currentTimeOfDay < 8) {
            currentTimeString = "12:00 AM";
        }
        else if (currentTimeOfDay >= 8 && currentTimeOfDay < 11) {
            currentTimeString = "3:00 AM";
        }
        else if (currentTimeOfDay >= 11 && currentTimeOfDay < 14) {
            currentTimeString = "6:00 AM";
        }
        else if (currentTimeOfDay >= 14 && currentTimeOfDay < 17) {
            currentTimeString = "9:00 AM";
        }
        else if (currentTimeOfDay >= 17 && currentTimeOfDay < 20) {
            currentTimeString = "12:00 PM";
        }
        else if (currentTimeOfDay >= 20 && currentTimeOfDay < 24) {
            currentTimeString = "3:00 PM";
        }
        Debug.Log(currentTimeOfDay + ": " + currentTimeString);
        clock.text = currentTimeString;
    }
    private void SpawnSpotLights() {
        var gameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        for (var i=0; i < gameObjects.Length; i++){
            if(gameObjects[i].name.Contains("Lights-Three")){
                var spawnPosition = gameObjects[i].transform.position;
                var newSpotLight = (GameObject) GameObject.Instantiate(spotLight, gameObjects[i].transform);
                newSpotLight.transform.localPosition = new Vector3(5f, 8.19f, -25f);
                newSpotLight.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0, 0));
                                                
                var newSpotLight2 = (GameObject) GameObject.Instantiate(spotLight, gameObjects[i].transform);
                newSpotLight2.transform.localPosition = new Vector3(5f, 8.19f, -0f);
                newSpotLight2.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0, 0));                             
                
                var newSpotLight3 = (GameObject) GameObject.Instantiate(spotLight, gameObjects[i].transform);
                newSpotLight3.transform.localPosition = new Vector3(5f, 8.19f, 25f);
                newSpotLight3.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0, 0));                           
                allStreetLightObjects.Add(newSpotLight);
                allStreetLightObjects.Add(newSpotLight2);
                allStreetLightObjects.Add(newSpotLight3);

                
            }
        }
    }
    private void KillAllSpotLights() {
        var gameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        for (var i=0; i < gameObjects.Length; i++){
            if(gameObjects[i].name.Contains("Spot Light(Clone)")){
                GameObject.Destroy(gameObjects[i]);          

            }
        }
    }
    private void TurnOffStreetLights() {
        for (var i=0; i < allStreetLightObjects.Count; i++){
            Light currentspotLight = allStreetLightObjects[i].GetComponent<Light>();
            currentspotLight.enabled = false;
        }
    }
    private void TurnOnStreetLights() {
        for (var i=0; i < allStreetLightObjects.Count; i++){
            Light currentspotLight = allStreetLightObjects[i].GetComponent<Light>();
            currentspotLight.enabled = true;
        }
    }
}
