using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;
public class PersonDetector : MonoBehaviour
{
    string leftObject;
    string rightObject;

    public GameObject[] emotionEffects;
    public GameObject currentTarget;
    // List<int> scannedPeople;
    Dictionary<int, GameObject> scannedPeople = new Dictionary<int, GameObject>();
    Dictionary<string, int> scanInfo = new Dictionary<string, int>();
    GameObject currentEffect = null;
    public bool autoActivated = false;

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
        // scannedPeople = new List<int>();
        scanInfo = new Dictionary<string, int>(){
            {"happy", 0},
            {"sad", 0},
            {"neutral", 0},
            {"angry", 0}
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (autoActivated) {
            DetectPerson();
            if (Input.GetKeyDown(KeyCode.T)) {
            DeactivateEffects();
            }
            if (Input.GetKeyDown(KeyCode.Y)) {
                ActivateEffects();
            }
        }

    }
    void DetectPerson() {
            Vector3 topOfPerson = transform.position; //+ new Vector3(0f, m_Collider.height+ 0.25f, 0f)
            Vector3 centerOfPerson = transform.position;
            var verticalOffset = new Vector3(0f, 0.2f, 0f);

            var right45 = 3f * ((transform.forward + transform.right - transform.up).normalized + verticalOffset);
            // since transform.left doesn't exist, you can use -transform.right instead
            var left45 = 3f * ((transform.forward - transform.right - transform.up).normalized + verticalOffset);
            var frontĐir = 3f * transform.forward;
            var lowerFrontDir = 3f* (transform.forward + new Vector3(0f, -1f, 0f));
            Debug.DrawRay(topOfPerson, right45, Color.green);
            Debug.DrawRay(topOfPerson, left45, Color.green);
            // Debug.DrawRay(centerOfPerson, frontĐir, Color.green);
            Debug.DrawRay(centerOfPerson, lowerFrontDir, Color.red);
            float thickness = 10f; //<-- Desired thickness here.
            // Debug.Log((transform.forward - transform.right - transform.up).normalized);
            RaycastHit leftHit;
            RaycastHit rightHit;



            if (Physics.SphereCast(topOfPerson, thickness, left45 , out leftHit)) {
                if (leftHit.collider.gameObject.layer == 6) {
                    var colliderName = leftHit.collider.gameObject.name;
                    // Debug.Log("left: " + colliderName);
                    leftObject = colliderName;
                    currentTarget = leftHit.transform.gameObject;
                    // scannedPeople.Add(currentTarget.GetInstanceID(), currentTarget);
                    AssignRandomEmotion();
                }

            }


            if (Physics.SphereCast(topOfPerson, thickness, right45, out rightHit)) {
                if (rightHit.collider.gameObject.layer == 6) {
                    var colliderName = rightHit.collider.gameObject.name;
                    // Debug.Log("right: " + rightHit.collider.gameObject.name);
                    rightObject = colliderName;
                    currentTarget = rightHit.transform.gameObject;
                    // scannedPeople.Add(currentTarget.GetInstanceID(), currentTarget);
                    AssignRandomEmotion();



                }
            }
            RaycastHit frontHit;
            var frontObject = "";
            if (Physics.SphereCast(centerOfPerson, thickness, lowerFrontDir, out frontHit)) {
                if (frontHit.collider.gameObject.layer == 6) {
                    var colliderName = frontHit.collider.gameObject.name;
                    // Debug.Log("front: " + colliderName);
                    frontObject = colliderName;
                    currentTarget = frontHit.transform.gameObject;
                    AssignRandomEmotion();
                    // scannedPeople.Add(currentTarget.GetInstanceID(), currentTarget);

                    // TurnRight();
                    // shouldCheckFront = false;
                    // Invoke("ResetFrontChecking", 2.0f);

                }
            }
    }
    public void PredictEmotion() {
        StartCoroutine(ProcessRequest("https://hackaroo.ngrok.io/uploader", currentImage));
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
        // TurnOffPopup();

    }
    void AssignEmotion(GameObject go, string emotion) {
        GameObject effectToAssign = null;
        if (!scannedPeople.ContainsKey(go.GetInstanceID())) {
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
            // if (currentEffect != null) {
            //     GameObject.Destroy(currentEffect);
            //     currentEffect = null;
            // }
            // effectToAssign = emotionEffects[2];
            currentEffect = (GameObject) GameObject.Instantiate(effectToAssign, go.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
            currentEffect.transform.parent = go.transform;
            scannedPeople.Add(currentTarget.GetInstanceID(), currentEffect);
            scanInfo[emotion] += 1;
            Debug.Log("Number of Scanned People: " + scannedPeople.Count);
        }
    }
    void DeactivateEffects() {
        foreach(KeyValuePair<int,GameObject> person in scannedPeople)
        {
            //Now you can access the key and value both separately from this attachStat as:
            // Debug.Log(person.Key);
            // Debug.Log(person.Value);
            // GameObject.Destroy(person.Value);
            person.Value.SetActive(false);
        }
    }
    void ActivateEffects() {
        foreach(KeyValuePair<int,GameObject> person in scannedPeople)
        {
            //Now you can access the key and value both separately from this attachStat as:
            // Debug.Log(person.Key);
            // Debug.Log(person.Value);
            // GameObject.Destroy(person.Value);
            person.Value.SetActive(true);
        }
        foreach(KeyValuePair<string,int> item in scanInfo)
        {
            //Now you can access the key and value both separately from this attachStat as:
            // Debug.Log(person.Key);
            // Debug.Log(person.Value);
            // GameObject.Destroy(person.Value);
            Debug.Log(item.Key + ": " + item.Value);
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
            // TurnOffPopup();
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
