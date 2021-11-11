using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TrackClick : MonoBehaviour
{
    public GameObject[] emotionEffects;
    public GameObject currentTarget;
    public GameObject popupWindow;
    public RawImage avatar;
    RectTransform rectTransform;
    // public List<GameObject> allPeopleImages;
    public Texture2D[] allPeopleImagesArray;
    GameObject currentEffect = null;
    // Start is called before the first frame update
    void Start()
    {
        // var allImages = LoadAllPeopleImages();
        // if (allImages != null) {
        //     allPeopleImagesArray = allImages;
        // }
        popupWindow.SetActive(false);
        // Debug.Log(allPeopleImagesArray.Length);
        avatar = popupWindow.transform.Find("RawImage").gameObject.GetComponent<RawImage>();
        rectTransform = popupWindow.transform.Find("RawImage").gameObject.GetComponent (typeof (RectTransform)) as RectTransform;
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
                    string targetName = hit.transform.gameObject.name.ToLower();
                    Debug.Log(hit.transform.gameObject.name);
                    currentTarget = hit.transform.gameObject;
                    Texture2D targetImage = AssignPersonImage("random");

                    //Assigns a photo//
                    if (targetName.Contains("female")){
                        
                        targetImage = AssignPersonImage("female");
                    }
                    else if (targetName.Contains("male")) {
                        targetImage = AssignPersonImage("male");
                    }
                    Debug.Log(targetName.Contains("female"));
                    
                     
                    float scaleXtoY = (float) targetImage.width/(float) targetImage.height;
                    
                    avatar.texture = targetImage;
                    rectTransform.localScale  = new Vector3(scaleXtoY * 2f, 2f, 0f );
                    // Debug.Log(rectTransform.sizeDelta);
                    // Debug.Log(targetImage.width + "/" + targetImage.height + ": " + targetImage.width/targetImage.height);

                    //Random emotion for now//
                    TurnOnPopup();
                    
                }
                else {
                   Debug.Log(hit.transform.gameObject.name);
                }
            }
                
        }
    }
    // Texture2D[] LoadAllPeopleImages() {
    //     Texture2D[] imgL = (Texture2D[]) Resources.LoadAll("person");
    //     Debug.Log(imgL.Length);
    //     return imgL;
    // }
    Texture2D AssignPersonImage(string gender) {
        Texture2D imageToAssign = allPeopleImagesArray[Random.Range (0, allPeopleImagesArray.Length)];
        string[] imageInfo = imageToAssign.name.Split('_');
        Debug.Log(gender);
        
        if (gender == "female") {
            Debug.Log("female");
            Debug.Log(imageInfo[1]);

            while (imageInfo[1] != "1" ) {
                imageToAssign = allPeopleImagesArray[Random.Range(0, allPeopleImagesArray.Length)];
                imageInfo = imageToAssign.name.Split('_');

            }  
        }
        else if (gender == "male") {

            while (imageInfo[1] != "0" ) {
                imageToAssign = allPeopleImagesArray[Random.Range (0, allPeopleImagesArray.Length)];
                imageInfo = imageToAssign.name.Split('_');

            }
        }
        Debug.Log(imageToAssign.name);
        return imageToAssign;
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
            randomEmotion = "angry";
        }
        else if (r == 3) {
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
        else if (emotion.ToLower() == "angry") {
            effectToAssign = emotionEffects[2];
        }
        else if (emotion.ToLower() == "neutral") {
            effectToAssign = emotionEffects[3];
        }
        // effectToAssign = emotionEffects[1];
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
