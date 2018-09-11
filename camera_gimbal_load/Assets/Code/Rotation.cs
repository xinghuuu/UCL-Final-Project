using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public GameObject motor1RotationAxis;
    public GameObject motor2RotationAxis;
    public GameObject motor3RotationAxis;
    public GameObject CameraGimbal;
    public GameObject ObjectCapture;
    public GameObject CameraPosition;
    public float rotationSpeed;
    public float translationSpeed = 10;
    public float Distance_;
    public Vector3 Position_;
    public Vector3 Velocity_;
    Vector3 PreviousPosition;
    private Vector3 screenPoint;
    private Vector3 offset;




    // Use this for initialization
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        translation();
        GetDistance();
        position();
        velocity();
    }

	void OnMouseDown()
	{
        screenPoint = Camera.main.WorldToScreenPoint(CameraGimbal.transform.position);
        offset = CameraGimbal.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        print("Mouse Down");
	}

    void OnMouseDrag()
    {
        Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorScreenPoint) + offset;
        CameraGimbal.transform.position = cursorPosition;
        print(cursorPosition);
    }

    void MouseControl()
    {
        screenPoint = Camera.main.WorldToScreenPoint(CameraGimbal.transform.position);
        offset = CameraGimbal.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorScreenPoint) + offset;
        CameraGimbal.transform.position = cursorPosition;   
    }

	// Get current 3D position of the camera
	void position()
    {
        Position_ = CameraPosition.transform.position;
    }
    void velocity()
    {
        Velocity_ = (Position_ - PreviousPosition) / Time.deltaTime;
        PreviousPosition = Position_;
    }

    //Calculate the Distance between the camera and one single object 
    void GetDistance()
    {
        Distance_ = Vector3.Distance(CameraPosition.transform.position, ObjectCapture.transform.position);
    }

    //Move the camera gimbal(Translation) 
    void translation()
    {

        //Left and Right movement of the camera gimbal 
        if (Input.GetKey(KeyCode.A))
        {
            float SpeedRadians_Left;
            SpeedRadians_Left = Mathf.Asin(translationSpeed / Distance_);
            rotationSpeed = SpeedRadians_Left * (180 / Mathf.PI);
            CameraGimbal.transform.Translate(Vector3.forward * translationSpeed * Time.deltaTime);
            motor1RotationAxis.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            float SpeedRadians_Right;
            SpeedRadians_Right = Mathf.Asin(translationSpeed / Distance_);
            rotationSpeed = SpeedRadians_Right * (180 / Mathf.PI);
            CameraGimbal.transform.Translate(-Vector3.forward * translationSpeed * Time.deltaTime);
            motor1RotationAxis.transform.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime);
        }


        // Forward and Backward movement of the camera gimbal 
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //float SpeedRadians_Left;
            CameraGimbal.transform.Translate(Vector3.left * translationSpeed * Time.deltaTime);
            //SpeedRadians_Left = Mathf.Tan(translationSpeed / Distance_);
            //rotationSpeed = SpeedRadians_Left * (180 / Mathf.PI);
            //motor1RotationAxis.transform.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            //float SpeedRadians_Left;
            CameraGimbal.transform.Translate(-Vector3.left * translationSpeed * Time.deltaTime);
            //SpeedRadians_Left = Mathf.Tan(translationSpeed / Distance_);
            //rotationSpeed = SpeedRadians_Left * (180 / Mathf.PI);
            //motor1RotationAxis.transform.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime);
        }

        // Vertical movment of the camera gimbal 
        if (Input.GetKey(KeyCode.UpArrow))
        {
            float SpeedRadians_Up;
            SpeedRadians_Up = Mathf.Asin(translationSpeed / Distance_);
            rotationSpeed = SpeedRadians_Up * (180 / Mathf.PI);
            CameraGimbal.transform.Translate(Vector3.up * translationSpeed * Time.deltaTime);
            motor3RotationAxis.transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.DownArrow))
        {
            float SpeedRadians_Down;
            SpeedRadians_Down = Mathf.Asin(translationSpeed / Distance_);
            rotationSpeed = SpeedRadians_Down * (180 / Mathf.PI);
            CameraGimbal.transform.Translate(-Vector3.up * translationSpeed * Time.deltaTime);
            motor3RotationAxis.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        
    }

}
