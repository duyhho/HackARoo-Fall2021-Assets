using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLightManagement : MonoBehaviour
{
    [SerializeField] private GameObject spotLight;

    List<GameObject> allStreetLightObjects = new List<GameObject>();
    LightingManager lightingManager;
    float currentTimeOfDay;
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
