using UnityEngine;
using System.Collections.Generic;

 using System.Collections;
 using System.IO;
 using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;
 // Screen Recorder will save individual images of active scene in any resolution and of a specific image format
 // including raw, jpg, png, and ppm.  Raw and PPM are the fastest image formats for saving.
 //
 // You can compile these images into a video using ffmpeg:
 // ffmpeg -i screen_3840x2160_%d.ppm -y test.avi
 
 public class ScreenRecorder : MonoBehaviour
 {
     public RawImage avatar;
     string currentPredictionMode = "house";
     [Serializable]
    public class PredictionResult {
        [JsonProperty(PropertyName = "image")]
        public string base64img;
        
    }
     // 4k = 3840 x 2160   1080p = 1920 x 1080
     public int captureWidth = 1920;
     public int captureHeight = 1080;
 
     // optional game object to hide during screenshots (usually your scene canvas hud)
     public GameObject hideGameObject; 
 
     // optimize for many screenshots will not destroy any objects so future screenshots will be fast
     public bool optimizeForManyScreenshots = true;
 
     // configure with raw, jpg, png, or ppm (simple raw format)
     public enum Format { RAW, JPG, PNG, PPM };
     public Format format = Format.PPM;
 
     // folder to write output (defaults to data path)
     public string folder;
 
     // private vars for screenshot
     private Rect rect;
     private RenderTexture renderTexture;
     private Texture2D screenShot;
     private int counter = 0; // image #
 
     // commands
     private bool captureScreenshot = false;
     private bool captureVideo = false;
 
     // create a unique filename using a one-up variable
     private string uniqueFilename(int width, int height)
     {
         // if folder not specified by now use a good default
         if (folder == null || folder.Length == 0)
         {
             folder = Application.dataPath;
             if (Application.isEditor)
             {
                 // put screenshots in folder above asset path so unity doesn't index the files
                 var stringPath = folder + "/..";
                 folder = Path.GetFullPath(stringPath);
             }
             folder += "/screenshots";
 
             // make sure directoroy exists
             System.IO.Directory.CreateDirectory(folder);
 
             // count number of files of specified format in folder
             string mask = string.Format("screen_{0}x{1}*.{2}", width, height, format.ToString().ToLower());
             counter = Directory.GetFiles(folder, mask, SearchOption.TopDirectoryOnly).Length;
         }
 
         // use width, height, and counter for unique file name
         var filename = string.Format("{0}/screen_{1}x{2}_{3}.{4}", folder, width, height, counter, format.ToString().ToLower());
 
         // up counter for next call
         ++counter;
 
         // return unique filename
         return filename;
     }
 
     public void CaptureScreenshot()
     {
         captureScreenshot = true;
     }
    void Start() {
        // avatar = popupWindow.transform.Find("RawImage").gameObject.GetComponent<RawImage>();
        avatar.enabled = false;
    }
     void Update()
     {
         // check keyboard 'k' for one time screenshot capture and holding down 'v' for continious screenshots
        //  captureScreenshot |= Input.GetKeyDown("k");
        //  captureVideo = Input.GetKey("v");
 
         if (captureScreenshot || captureVideo)
         {
             captureScreenshot = false;
 
             // hide optional game object if set
             if (hideGameObject != null) hideGameObject.SetActive(false);
 
             // create screenshot objects if needed
             if (renderTexture == null)
             {
                 // creates off-screen render texture that can rendered into
                 rect = new Rect(0, 0, captureWidth, captureHeight);
                 renderTexture = new RenderTexture(captureWidth, captureHeight, 24);
                 screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
             }
         
             // get main camera and manually render scene into rt
             Camera camera = this.GetComponent<Camera>(); // NOTE: added because there was no reference to camera in original script; must add this script to Camera
             camera.targetTexture = renderTexture;
             camera.Render();
 
             // read pixels will read from the currently active render texture so make our offscreen 
             // render texture active and then read the pixels
             RenderTexture.active = renderTexture;
             screenShot.ReadPixels(rect, 0, 0);
 
             // reset active camera texture and render texture
             camera.targetTexture = null;
             RenderTexture.active = null;
 
             // get our unique filename
             string filename = uniqueFilename((int) rect.width, (int) rect.height);
 
             // pull in our file header/data bytes for the specified image format (has to be done from main thread)
             byte[] fileHeader = null;
             byte[] fileData = null;
             if (format == Format.RAW)
             {
                 fileData = screenShot.GetRawTextureData();
             }
             else if (format == Format.PNG)
             {
                 fileData = screenShot.EncodeToPNG();
             }
             else if (format == Format.JPG)
             {
                fileData = screenShot.EncodeToJPG();
                StartCoroutine(ProcessRequest(String.Format("https://hackaroo.ngrok.io/api/predict/{0}", currentPredictionMode), fileData));


             }
             else // ppm
             {
                 // create a file header for ppm formatted file
                 string headerStr = string.Format("P6\n{0} {1}\n255\n", rect.width, rect.height);
                 fileHeader = System.Text.Encoding.ASCII.GetBytes(headerStr);
                 fileData = screenShot.GetRawTextureData();
             }
 
             // create new thread to save the image to file (only operation that can be done in background)
             new System.Threading.Thread(() =>
             {
                 // create file and write optional header with image bytes
                 var f = System.IO.File.Create(filename);
                 if (fileHeader != null) f.Write(fileHeader, 0, fileHeader.Length);
                 f.Write(fileData, 0, fileData.Length);
                 f.Close();
                 Debug.Log(string.Format("Wrote screenshot {0} of size {1}", filename, fileData.Length));
             }).Start();
 
             // unhide optional game object if set
             if (hideGameObject != null) hideGameObject.SetActive(true);
 
             // cleanup if needed
             if (optimizeForManyScreenshots == false)
             {
                 Destroy(renderTexture);
                 renderTexture = null;
                 screenShot = null;
             }
         }
     }
     private IEnumerator ProcessRequest(string apiURL, byte[] jpg)
    {

			// texture2D.Apply(false); // Not required. Because we do not need to be uploaded it to GPU
		// Texture2D decompressed = DeCompress(image);
        // byte[] jpg = decompressed.EncodeToJPG();
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
                PredictionResult serverPredictionResult = GetPredictionResult(jsonResponse);
                DisplayImage(serverPredictionResult);

                // Debug.Log(sentimentInfo.gender);
                // AssignEmotion(currentTarget, serverPredictionResult.sentiment);
                // AssignEmotion(currentTarget, "sad");

            }
            // TurnOffPopup();
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
    void DisplayImage(PredictionResult result) {
        Debug.Log(result.base64img);
        byte[]  imageBytes = Convert.FromBase64String(result.base64img);
        Texture2D tex = new Texture2D(640, 640);
        tex.LoadImage( imageBytes );
        avatar.texture = tex;
        avatar.enabled = true;
    }
    public void PredictHouse(){
        captureScreenshot = true;
        currentPredictionMode = "house";
        // StartCoroutine(ProcessRequest("https://hackaroo.ngrok.io/api/predict/house", fileData));

    }
    public void  PredictUtility(){
         captureScreenshot = true;
        currentPredictionMode = "utility";
        // StartCoroutine(ProcessRequest("https://hackaroo.ngrok.io/api/predict/utility", fileData));
    }
    public void  PredictRoad(){
         captureScreenshot = true;
        currentPredictionMode = "road";
        // StartCoroutine(ProcessRequest("https://hackaroo.ngrok.io/api/predict/road", fileData));
    }
    public void  PredictVehicle(){
         captureScreenshot = true;
        currentPredictionMode = "vehicle";
        // StartCoroutine(ProcessRequest("https://hackaroo.ngrok.io/api/predict/vehicle", fileData));
    }
     
 }
 