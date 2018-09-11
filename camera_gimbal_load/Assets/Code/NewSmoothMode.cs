using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class NewSmoothMode : MonoBehaviour 
{
    public GameObject CameraGimbal;
    public float MoveSpeed;
    public GameObject motor1RotationAxis;
    public GameObject motor2RotationAxis;
    public GameObject motor3RotationAxis;
    Vector3 GimbalPosition, PreviousPosition, VelocityPosition, GimbalRotation, VelocityRotation, PreviousRotation;
    float Angle1, Angle2, Angle3, AngleGimbalX, AngleGimbalZ;
    double NormDist,NormDist1;
    double sigma,sigma1;
    double V_0,V_1;
    double Exp = 2.718;
    double Input = -2,Input1;
    double VelocityObject;
    double V_th = 20;
    double AngularVelocity,V_minus,V_back;
	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {

        MoveCameraGimbal();
        IMU();
        MotorLimit();
		
	}

    //IMU measurement 
    void IMU()
    {
        GimbalPosition = CameraGimbal.transform.position;
        VelocityPosition = GimbalPosition - PreviousPosition;
        PreviousPosition = GimbalPosition;
        GimbalRotation = CameraGimbal.transform.eulerAngles;
        VelocityRotation = GimbalRotation - PreviousRotation;
        PreviousRotation = GimbalRotation;
        Angle1 = motor1RotationAxis.transform.eulerAngles.y;
        Angle2 = motor3RotationAxis.transform.eulerAngles.z;
        Angle3 = motor2RotationAxis.transform.eulerAngles.x;
        AngleGimbalX = CameraGimbal.transform.eulerAngles.x;
        AngleGimbalZ = CameraGimbal.transform.eulerAngles.z;
    }

    void MoveCameraGimbal()
    {
        Input = Input + 0.02;
        if (Input < 3)
        {
            V_0 = 100;
            sigma = 0.8;
            NormDist = (1 / ((Mathf.Sqrt(2 * Mathf.PI)) * sigma)) * (Math.Pow(Exp, -0.5 * (Input / sigma) * (Input / sigma)));
            VelocityObject = V_0 * NormDist;
        }
        else
        {
            VelocityObject = 0;
        }
        CameraGimbal.transform.Translate(-Vector3.up * (float)VelocityObject * Time.deltaTime);
    }

    //Move the camera gimbal 

    void Smooth()
    {
        if (VelocityObject > V_th)
        {
            V_minus = VelocityObject - V_th;
            motor3RotationAxis.transform.Rotate(Vector3.forward * (float)V_minus * (float)NormDist * Time.deltaTime);
        }

        if (VelocityObject < V_th && Angle2 > 0)
        {
            V_1 = 5;
            Input1 = Input1 + 0.002;
            sigma = 0.8;
            NormDist = (1 / ((Mathf.Sqrt(2 * Mathf.PI)) * sigma)) * (Math.Pow(Exp, -0.5 * (Input1 / sigma) * (Input1 / sigma)));
            V_back = V_1 * NormDist;
            
            motor3RotationAxis.transform.Rotate(-Vector3.forward * (float)V_back*  Time.deltaTime);
        }
    }

    void MotorLimit()
    {
        if (Angle2 >-1 && Angle2 < 60)
        {
            Smooth();
        }
        else
        {
            print("Warning: Motor Limit");    
        }
    }
}
