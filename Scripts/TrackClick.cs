using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;
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

    Texture2D currentImage;
    [Serializable]
    public class PredictionResult {
        [JsonProperty(PropertyName = "Sentiment")]
        public string sentiment;
        [JsonProperty(PropertyName = "Age")]
        public string age;
        [JsonProperty(PropertyName = "Ethnicity")]
        public string ethnicity;
        [JsonProperty(PropertyName = "Gender")]
        public string gender;
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
                    currentImage = AssignPersonImage("random");

                    //Assigns a photo//
                    if (targetName.Contains("female")){
                        
                        currentImage = AssignPersonImage("female");
                    }
                    else if (targetName.Contains("male")) {
                        currentImage = AssignPersonImage("male");
                    }
                    Debug.Log(targetName.Contains("female"));
                    
                     
                    float scaleXtoY = (float) currentImage.width/(float) currentImage.height;
                    
                    avatar.texture = currentImage;
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
        Texture2D imageToAssign = allPeopleImagesArray[UnityEngine.Random.Range (0, allPeopleImagesArray.Length)];
        string[] imageInfo = imageToAssign.name.Split('_');
        Debug.Log(gender);
        
        if (gender == "female") {
            Debug.Log("female");
            Debug.Log(imageInfo[1]);

            while (imageInfo[1] != "1" ) {
                imageToAssign = allPeopleImagesArray[UnityEngine.Random.Range(0, allPeopleImagesArray.Length)];
                imageInfo = imageToAssign.name.Split('_');
            }  
        }
        else if (gender == "male") {

            while (imageInfo[1] != "0" ) {
                imageToAssign = allPeopleImagesArray[UnityEngine.Random.Range (0, allPeopleImagesArray.Length)];
                imageInfo = imageToAssign.name.Split('_');

            }
        }
        Debug.Log(imageToAssign.name);
        return imageToAssign;
    }
    public void AssignRandomEmotion() {
        int r = UnityEngine.Random.Range(0, emotionEffects.Length);
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

    public void PredictEmotion() {
        StartCoroutine(ProcessRequest("https://hackaroo.ngrok.io/uploader", currentImage));
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

        PredictionResult info = JsonConvert.DeserializeObject<PredictionResult>(jsonResponse);
        // Debug.Log(info.payload.scannedCount);
        return info;
    }
    private IEnumerator ProcessRequest(string apiURL, Texture2D image)
    {

			// texture2D.Apply(false); // Not required. Because we do not need to be uploaded it to GPU
		Texture2D decompressed = DeCompress(image);
        byte[] jpg = decompressed.EncodeToJPG();
		List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        // formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        formData.Add(new MultipartFormFileSection("file", jpg, "upload.jpg" , "image/jpg"));

        using (UnityWebRequest request = UnityWebRequest.Post(apiURL, formData))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log(jsonResponse);
                PredictionResult sentimentInfo = GetPredictionResult(jsonResponse);
                // Debug.Log(sentimentInfo.sentiment);
                // Debug.Log(sentimentInfo.gender);
                AssignEmotion(currentTarget, sentimentInfo.sentiment);
            }
            TurnOffPopup();
        }
    }
    public Texture2D DeCompress(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
}
