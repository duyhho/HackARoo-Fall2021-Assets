using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PredictButton : MonoBehaviour
{
    public Button predictButton;
    public GameObject webcamPlane;

	void Start () {
		Button btn = predictButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick(){
		Debug.Log ("You have clicked the button!");
        StartCoroutine(webcamPlane.GetComponent<WebCamTextureToCloudVision>().CaptureOneTime());
	}
    public void ToggleActive() {
        transform.gameObject.SetActive(!gameObject.activeSelf);
    }
}
