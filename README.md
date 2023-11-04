# MetaLens
“Metaverse”, a new form of social life via virtual reality, is a new ambitious vision set by Meta (formerly known as Facebook) that allows users to socialize in a virtual world. It is a groundbreaking concept in social networking in which we will perform many activities such as working, playing, studying, and interacting with each other in an immersive way. Thus, our project, MetaLens, plans to focus primarily on a vision in the future where daily lives are enhanced with technology and Artificial Intelligence (AI). Our lives will be surrounded by robots, drones, and other automatic systems that make our tasks a lot more convenient. Via AI, Deep Learning, Sentiment Analysis, and Object Recognition, MetaLens aims to help not only the citizens but also the government be aware of their citizen’s well-being and the city’s overall performance to derive appropriate solutions.
# Datasets

## UTK Face Dataset:

https://susanqq.github.io/UTKFace/
~ 20,000 face images with gender, age, and ethnicity


Google Street View API https://developers.google.com/maps/documentation/streetview/overview ~ Houses, Utility Poles + Street Lights

COCO Dataset (80 objects)

https://cocodataset.org
Vehicles

Road Damage Dataset

https://github.com/sekilab/RoadDamageDetector
Road

# Approach
## Data Research/Collection

UTK has 2 versions:
- Cropped + Grayscaled 
- Uncropped + Colored
  
## Data Curation
- Data simplification
- Data Extraction
- Data Structuring

## Model Training (2 Phases):

### Initial (naïve)
- Close-up Face version (limited vision cues)
- Simple < 10-layer CNN
- Low accuracy for each category (~50-60%)
### Revised
- Colored Portrait version (more hints about clothes, skin tone, … )
- Transfer Learning with ResNet-50 pretrained on ImageNet and Early Stopping
- Increased accuracy to 70-90%
  
### Model Inference and Deployment
- Input: Image
- Output: Age, Gender, Ethnicity, and Sentiment (Emotion)

###API endpoint: https://hackaroo.ngrok.io/uploader
## Application Deployment

Unity: PC version, Web, and VR

# Model Predictions: 
![image](https://github.com/duyhho/HackARoo-Fall2021-Assets/assets/17374092/16f58234-fa11-4649-9a61-ee0d763511c5)

```
{
  "Age": "Middle-Aged",
  "Age Estimate": 55,
  "Ethnicity": "Indian",
  "Gender": "Male",
  "Sentiment": "Neutral"
}
```
# Front-end Design (Unity):

![image](https://github.com/duyhho/HackARoo-Fall2021-Assets/assets/17374092/025276fd-37d9-46d3-a97f-ffc50dece7d1)
![image](https://github.com/duyhho/HackARoo-Fall2021-Assets/assets/17374092/bff3fa1d-df55-466e-975f-fd3ffacb886b)
![image](https://github.com/duyhho/HackARoo-Fall2021-Assets/assets/17374092/b8d8c83e-06e8-4f5a-983d-d981bb797dad)
![gif1](https://github.com/duyhho/HackARoo-Fall2021-Assets/blob/main/Assets/Predict%20Woman.gif)
![gif1](https://github.com/duyhho/HackARoo-Fall2021-Assets/blob/main/Assets/PredictMan.gif)

# Video
Video: https://youtu.be/wus7FLhRER4

Colab: https://colab.research.google.com/drive/1E7223DY3RbS-OVM6cZ3VO7MKQxmZ5XZe?usp=sharing

GitHub (VR Version): https://github.com/benamreview/HackARoo-Fall2021-VR-Assets

GitHub (PC Version): https://github.com/benamreview/HackARoo-Fall2021-Assets

PPT Slides (with demos): https://mailmissouri-my.sharepoint.com/:p:/g/personal/dhh3hb_umsystem_edu/ESgtQ36AxjpGjiy9yIZMPxEBW0nTIMS14dP1CtJY8M9EEA?e=Dresha
