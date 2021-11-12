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

    [Serializable]
    public class PredictionResult {
        [JsonProperty(PropertyName = "Sentiment")]
        public string sentiment;
        [JsonProperty(PropertyName = "Age")]
        public string age;
        [JsonProperty(PropertyName = "Ethnicity")]
        public string ethnicity;
        [JsonProperty(PropertyName = "Destination_Classification")]
        public string destinationClassification;
        [JsonProperty(PropertyName = "Source")]
        public string source;
    }
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
        StartCoroutine(ProcessRequest("https://hackaroo.ngrok.io");
        //Continue//
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
    public PredictionResult GetPredictionResult(string jsonResponse)
    {
        //Valid: "https://dl.dropbox.com/s/4z4bzprj1pud3tq/Assets.json?dl=0"
        //Sample: https://dl.dropbox.com/s/fbh6jbyzrf86g0x/Assets-sample.json?dl=0

        PredictionResult info = JsonConvert.DeserializeObject<TrafficInfo>(jsonResponse);
        // Debug.Log(info.payload.scannedCount);
        return info;
    }
    private IEnumerator ProcessRequest(string apiURL, string trafficURL)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(trafficURL))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                // Debug.Log(jsonResponse);
                sentimentInfo = GetPredictionResult(jsonResponse);
                Debug.Log(sentimentInfo.sentiment);
                Debug.Log(sentimentInfo.gender);
            }
        }
        using (UnityWebRequest request = UnityWebRequest.Get(apiURL))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                cityInfo = GetBuildings(jsonResponse);
                subnetGroups = GetSubnetGroups(cityInfo);
                GenerateCustomStreets();
                GenerateCustomBuildings();
                GenerateUI();
                coroutine = this.StartCoroutine(onCoroutine());
            }
        }
    }
}
