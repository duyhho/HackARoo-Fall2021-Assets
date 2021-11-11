using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RegularButton : MonoBehaviour
{
    public Button cancelButton;
    // public GameObject webcamPlane;

	void Start () {
		Button btn = cancelButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick(){
		Debug.Log ("You have clicked the button!");
        // StartCoroutine(webcamPlane.GetComponent<WebCamTextureToCloudVision>().CaptureOneTime());
	}
    public void ToggleActive() {
        transform.gameObject.SetActive(!gameObject.activeSelf);
    }
}
