using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackClick : MonoBehaviour
{
    public GameObject[] emotionEffects;
    public GameObject currentTarget;
    public GameObject popupWindow;

    GameObject currentEffect = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit  hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit))  {
                if (hit.transform.gameObject.layer == 6 ) {
                    Debug.Log( "My person is clicked by mouse");
                    Debug.Log(hit.transform.gameObject.name);
                    currentTarget = hit.transform.gameObject;


                    //Random emotion for now//
                    TurnOnPopup();
                    
                }
                else {
                   Debug.Log(hit.transform.gameObject.name);
                }
            }
                
        }
    }
    public void AssignRandomEmotion() {
        int r = Random.Range(0, emotionEffects.Length);
        string randomEmotion = "happy";
        if (r == 0) {
            randomEmotion = "happy";
        }
        else if (r == 1) {
            randomEmotion = "sad";
        }
        else if (r == 2) {
            randomEmotion = "neutral";
        }
        AssignEmotion(currentTarget, randomEmotion);
        TurnOffPopup();

    }
    void AssignEmotion(GameObject go, string emotion) {
        GameObject effectToAssign = new GameObject();
        Debug.Log(go.name + " is " + emotion );
        
        if (emotion.ToLower() == "happy") {
            effectToAssign = emotionEffects[0];
        }
        else if (emotion.ToLower() == "sad") {
            effectToAssign = emotionEffects[1];
        }
        else if (emotion.ToLower() == "neutral") {
            effectToAssign = emotionEffects[2];
        }
        if (currentEffect != null) {
            GameObject.Destroy(currentEffect);
            currentEffect = null;
        }
        currentEffect = (GameObject) GameObject.Instantiate(effectToAssign, go.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
        currentEffect.transform.parent = go.transform;



    }
    public void TurnOffPopup() {
        if (popupWindow) 
        {
            popupWindow.SetActive(false);
        }
    }
    public void TurnOnPopup() {
        if (popupWindow) 
        {
            popupWindow.SetActive(true);
        }
    }
}
