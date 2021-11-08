using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianWalking : MonoBehaviour
{
    
    Animator m_Animator; 
    CapsuleCollider m_Collider;
    float m_ScaleX, m_ScaleY, m_ScaleZ;
    bool shouldCheckSideWalk = true;
    bool shouldCheckFront = true;
    bool shouldCheckCollision = true;

    string leftObject;
    string rightObject;

    string currentState;

    List<string> supportedActions;

    float lastCheckTime = 0f;
    Vector3 lastCheckPos;
    float xSeconds = 5.0f;
    float yMuch = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Collider from the GameObject
        m_Collider = GetComponent<CapsuleCollider>();
        lastCheckPos = transform.position;

        //Get the Animator attached to the GameObject you are intending to animate.
        m_Animator = gameObject.GetComponent<Animator>();

        var randNum = Random.Range(5, 10);

        var initialState = "run";
        m_Animator.SetTrigger(initialState);
        currentState = initialState;

        leftObject = "";
        rightObject = "";

        supportedActions = new List<string>() {
            "laugh",
            "scream",
            "fall1",
            "fall2",
            "scream",
            "jump",
            "telloff",
            "land",
            "wave"
        };

        randNum = Random.Range(5, 10);

        Invoke("randomActions", randNum);

    }

    // Update is called once per frame
    void Update()
    {

        if (shouldCheckSideWalk) {
            CheckSideWalk();
        }
        // if (shouldCheckFront) {
        //     CheckFront();
        // }
        CheckStuck();

    }

    void FixedUpdate() {
        
    }
    void randomActions() {
        int r = Random.Range(0, supportedActions.Count);
        m_Animator.SetTrigger(supportedActions[r]);
        var randNum = Random.Range(10, 30);

        Invoke("randomActions", randNum);
    }

    void CheckSideWalk() {
        Vector3 topOfPerson = transform.position + new Vector3(0f, m_Collider.height+ 0.25f, 0f);
        var verticalOffset = new Vector3(0f, -0.1f, 0f);

        var right45 = (transform.forward + transform.right - transform.up).normalized + verticalOffset;
        // since transform.left doesn't exist, you can use -transform.right instead
        var left45 = (transform.forward - transform.right - transform.up).normalized + verticalOffset;
        // var left45 = new Vector3(1f, -1f, 1f);
        Debug.DrawRay(topOfPerson, right45, Color.green);
        Debug.DrawRay(topOfPerson, left45, Color.green);

        // Debug.Log((transform.forward - transform.right - transform.up).normalized);
        RaycastHit leftHit;
        RaycastHit rightHit;

       

        if (Physics.Raycast(topOfPerson, left45, out leftHit, 30f)) {
            if (leftHit.collider.gameObject.layer == 3) {
                var colliderName = leftHit.collider.gameObject.name;
                Debug.Log("left: " + colliderName);
                leftObject = colliderName;
                
            }
        }

         
        if (Physics.Raycast(topOfPerson, right45, out rightHit, 30f)) {
            if (rightHit.collider.gameObject.layer == 3) {
                var colliderName = rightHit.collider.gameObject.name;
                Debug.Log("right: " + rightHit.collider.gameObject.name);
                rightObject = colliderName;

                
            }
        }
        
        if (leftObject == rightObject) { //same on sidewalk, or road
            if (currentState == "walk") {
                m_Animator.ResetTrigger("turnRight_Walk");
                m_Animator.ResetTrigger("turnLeft_Walk");
                m_Animator.SetTrigger("walk");
            }
            else {
                m_Animator.ResetTrigger("turnRight");
                m_Animator.ResetTrigger("turnLeft");
                m_Animator.SetTrigger("run");
            }


        }
        else {
            if (leftObject.Contains("Road")) { //should turn right
               TurnRight();

            }
            else if (rightObject.Contains("Road")) {
                Debug.Log("turning left");
               TurnLeft();
            }
        }

    }

    void CheckFront() {
        Vector3 centerOfPerson = transform.position + new Vector3(0f, m_Collider.height*0.5f, 0f);
        var frontĐir = transform.forward;
        Debug.DrawRay(centerOfPerson, frontĐir, Color.green);

        RaycastHit frontHit;
        var frontObject = "";
        if (Physics.Raycast(centerOfPerson, frontĐir, out frontHit, 3f)) {
            if (frontHit.collider.gameObject.layer == 3) {
                var colliderName = frontHit.collider.gameObject.name;
                Debug.Log("front: " + colliderName);
                frontObject = colliderName;
                TurnRight();
                shouldCheckFront = false;
                Invoke("ResetFrontChecking", 2.0f);

            }
        }
    }
    void TurnRight() {
        if (currentState == "walk") {
             //Reset the "Jump" trigger
            m_Animator.ResetTrigger("walk");

            //Send the message to the Animator to activate the trigger parameter named "Crouch"
            m_Animator.SetTrigger("turnRight_Walk");
        }
        else if (currentState == "run") {
             //Reset the "Jump" trigger
            m_Animator.ResetTrigger("run");

            //Send the message to the Animator to activate the trigger parameter named "Crouch"
            m_Animator.SetTrigger("turnRight");
        }
        
        shouldCheckSideWalk = false;
        Invoke("ResetSideWalkChecking", 2.0f);
    }
    void TurnLeft() {
        if (currentState == "walk") {
             //Reset the "Jump" trigger
            m_Animator.ResetTrigger("walk");

            //Send the message to the Animator to activate the trigger parameter named "Crouch"
            m_Animator.SetTrigger("turnLeft_Walk");
        }
        else if (currentState == "run") {
             //Reset the "Jump" trigger
            m_Animator.ResetTrigger("run");

            //Send the message to the Animator to activate the trigger parameter named "Crouch"
            m_Animator.SetTrigger("turnLeft");
        }
        shouldCheckSideWalk = false;
        Invoke("ResetSideWalkChecking", 2.0f);
    }
    void TurnAround() {
        Debug.Log("turning around");
        if (currentState == "walk") {
            m_Animator.SetTrigger("turnAround_Walk");
        }
        else if (currentState == "run") {
            m_Animator.SetTrigger("turnAround");
        }
        // shouldCheckSideWalk = false;
        // Invoke("ResetSideWalkChecking", 2.0f);
    }

    void ResetSideWalkChecking() {
        Debug.Log("reset check sidewalk");
        shouldCheckSideWalk = true;
        if (currentState == "walk")
            m_Animator.SetTrigger("walk");
        else if (currentState == "run") {
            m_Animator.SetTrigger("run");

        }
    }

    void ResetFrontChecking() {
        Debug.Log("reset check front");
        shouldCheckFront = true;
        if (currentState == "walk")
            m_Animator.SetTrigger("walk");
        else if (currentState == "run") {
            m_Animator.SetTrigger("run");

        }


    }
    void ResetCollision() {
        Debug.Log("reset check collision");

        shouldCheckCollision = true;
       if (currentState == "walk")
            m_Animator.SetTrigger("walk");
        else if (currentState == "run") {
            m_Animator.SetTrigger("run");

        }

    }
    void OnCollisionStay(Collision collision)
    {
        if (shouldCheckCollision) {
            if (collision.gameObject.layer == 3  ) {
                if (collision.gameObject.name != "Colliders") {
                    Debug.Log(collision.gameObject.name);
                    var colliderName = collision.gameObject.name;
                    var randNum = Random.Range(0, 10);
                    if (leftObject != colliderName && rightObject != colliderName) {
                         if (randNum > 5) {

                            Debug.Log("turning leftttt");
                            TurnLeft();
                        } 
                        else {
                            TurnRight();
                        }
                    }
                    
                    if (leftObject == colliderName) {
                        if (leftObject == rightObject) {                      
                            TurnAround();                           
                        }
                        else
                            TurnRight();
                    }
                    else if (rightObject == colliderName) {
                        if (leftObject == rightObject) {                      
                            TurnAround();                           
                        }
                        else
                            TurnLeft();
                    }


                   
                    shouldCheckCollision = false;
                    Invoke("ResetCollision", 4.0f);

                }
            }
        }   
    }
    void CheckStuck() {
        if ((Time.time - lastCheckTime) > xSeconds)
        {
            Vector3 distance = transform.position - lastCheckPos;
            if (distance.magnitude < yMuch) {
                TurnAround();
            }
            
            lastCheckPos = transform.position;
            lastCheckTime = Time.time;
        }
    }
}
