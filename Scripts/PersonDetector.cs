using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
        // scannedPeople = new List<int>();
        scanInfo = new Dictionary<string, int>(){
            {"happy", 0},
            {"sad", 0},
            {"neutral", 0}
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
            // if (Physics.Raycast(centerOfPerson, frontĐir, out frontHit, 30f)) {
            //     if (frontHit.collider.gameObject.layer == 6) {
            //         var colliderName = frontHit.collider.gameObject.name;
            //         Debug.Log("front: " + colliderName);
            //         frontObject = colliderName;
            //         currentTarget = frontHit.transform.gameObject;
            //         AssignRandomEmotion();
            //         // scannedPeople.Add(currentTarget.GetInstanceID(), currentTarget);

            //         // TurnRight();
            //         // shouldCheckFront = false;
            //         // Invoke("ResetFrontChecking", 2.0f);

            //     }
            // }

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

            // if (leftObject == rightObject) { //same on sidewalk, or road
            //     if (currentState == "walk") {
            //         m_Animator.ResetTrigger("turnRight_Walk");
            //         m_Animator.ResetTrigger("turnLeft_Walk");
            //         m_Animator.SetTrigger("walk");
            //     }
            //     else {
            //         m_Animator.ResetTrigger("turnRight");
            //         m_Animator.ResetTrigger("turnLeft");
            //         m_Animator.SetTrigger("run");
            //     }


            // }
            // else {
            //     if (leftObject.Contains("Road")) { //should turn right
            //        TurnRight();

            //     }
            //     else if (rightObject.Contains("Road")) {
            //         Debug.Log("turning left");
            //        TurnLeft();
            //     }
            // }

    }
    // void CheckFront() {
    //     Vector3 centerOfPerson = transform.position + new Vector3(0f, m_Collider.height*0.5f, 0f);
    //     var frontĐir = transform.forward;
    //     Debug.DrawRay(centerOfPerson, frontĐir, Color.green);

    //     RaycastHit frontHit;
    //     var frontObject = "";
    //     if (Physics.Raycast(centerOfPerson, frontĐir, out frontHit, 3f)) {
    //         if (frontHit.collider.gameObject.layer == 3) {
    //             var colliderName = frontHit.collider.gameObject.name;
    //             Debug.Log("front: " + colliderName);
    //             frontObject = colliderName;
    //             TurnRight();
    //             shouldCheckFront = false;
    //             Invoke("ResetFrontChecking", 2.0f);

    //         }
    //     }
    // }
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
    // void UploadTexture(Texture2D tex)
    // {
    //     StartCoroutine(UploadTextureRoutine(tex));
    // }

    // private IEnumerator UploadTextureRoutine(Texture2D tex)
    // {
    //     var bytes = tex.EncodeToPNG();
    //     var form = new WWWForm();
    //     form.AddField("id", "Image01");
    //     form.AddBinaryData("image", bytes, $"{tex.name}.png", "image/png");

    //     using(var unityWebRequest = UnityWebRequest.Post("https://vision.googleapis.com/v1/images:annotate", form))
    //     {
    //         // unityWebRequest.SetRequestHeader("Authorization", "Token 555myToken555");

    //         yield return unityWebRequest.SendWebRequest();

    //         if (unityWebRequest.result != UnityWebRequest.Result.Success)
    //         {
    //             print($"Failed to upload {tex.name}: {unityWebRequest.result} - {unityWebRequest.error}");
    //         }
    //         else
    //         {
    //             print($"Finished Uploading {tex.name}");
    //         }
    //     }
    // }
}
