using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : MonoBehaviour
{
    public bool autoActivatedAll = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleActivation() {
        autoActivatedAll = !autoActivatedAll;
        if (autoActivatedAll) {
            foreach (Transform child in transform) {
                Debug.Log(child.gameObject.name);
                child.gameObject.GetComponent<PersonDetector>().autoActivated = true;
            }
        }
        else {
            foreach (Transform child in transform) {
                child.gameObject.GetComponent<PersonDetector>().autoActivated = false;
            }
        }

    }
}
