using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
public class SmoothMode : MonoBehaviour
{
    public GameObject CameraGimbal;
    public float MoveSpeed;
    public GameObject motor1RotationAxis;
    public GameObject motor2RotationAxis;
    public GameObject motor3RotationAxis;
    Vector3 GimbalPosition;
    Vector3 PreviousPosition;
    Vector3 VelocityPosition;
    Vector3 GimbalRotation;
    Vector3 PreviousRotation;
    Vector3 VelocityRotation;
    float Angle1;
    float Angle2;
    float Angle3;
    float AngleGimbalX;
    float AngleGimbalZ;
    double sigma;
    float rotationSpeedM3up,rotationSpeedM1,rotationSpeedM2,rotationSpeedM3;
    double NormDist;
    double Exp = 2.718;

    // Use this for initialization
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        MoveCameraGimbal();
        IMU();
        GimbalLock();
    }

    void MoveCameraGimbal()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            CameraGimbal.transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.X))
        {
            CameraGimbal.transform.Translate(-Vector3.forward * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            CameraGimbal.transform.Translate(-Vector3.left * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            CameraGimbal.transform.Translate(Vector3.left * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            CameraGimbal.transform.Translate(Vector3.up * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            CameraGimbal.transform.Translate(-Vector3.up * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.O))
        {
            CameraGimbal.transform.Rotate(Vector3.forward * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.P))
        {
            CameraGimbal.transform.Rotate(-Vector3.forward * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.K))
        {
            CameraGimbal.transform.Rotate(Vector3.up * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.L))
        {
            CameraGimbal.transform.Rotate(-Vector3.up * MoveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.M))
        {
            CameraGimbal.transform.Rotate(Vector3.left * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.N))
        {
            CameraGimbal.transform.Rotate(-Vector3.left * MoveSpeed * Time.deltaTime);
        }
    }

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

    void Smooth()
    {
        if (VelocityPosition.y > 0.1 || VelocityPosition.y < -0.1)
        {
            if (Angle2 > 0 && Angle2 < 90)
            {
                sigma = 1.3;
                double Input = (double)Angle2 / 60;
                double Exp = 2.718;
                NormDist = (1 / ((Mathf.Sqrt(2 * Mathf.PI)) * sigma)) * (Math.Pow(Exp, -0.5 * (Input / sigma) * (Input / sigma)));
                rotationSpeedM3up = 100 * VelocityPosition.y * (float)NormDist;
                motor3RotationAxis.transform.Rotate(-Vector3.forward * rotationSpeedM3up * Time.deltaTime);

            }
            if (Angle2 < 0 && Angle2 > -1)
            {
                motor3RotationAxis.transform.Rotate(-Vector3.forward * 50 * VelocityPosition.y * Time.deltaTime);
            }
            else
            {
                sigma = 1.3;
                double Input = (-(double)Angle2 + 360) / 60;
                double Exp = 2.718;
                NormDist = (1 / ((Mathf.Sqrt(2 * Mathf.PI)) * sigma)) * (Math.Pow(Exp, -0.5 * (Input / sigma) * (Input / sigma)));
                rotationSpeedM3up = 100 * VelocityPosition.y * (float)NormDist;
                motor3RotationAxis.transform.Rotate(-Vector3.forward * rotationSpeedM3up * Time.deltaTime);
            }
        }


        if (VelocityPosition.z > 0.1 || VelocityPosition.z < -0.1)
        {
            if (Angle1 > 0 && Angle1 < 90)
            {
                sigma = 1.8;
                double Input = (double)Angle1 / 60;
                double Exp = 2.718;
                NormDist = (1 / ((Mathf.Sqrt(2 * Mathf.PI)) * sigma)) * (Math.Pow(Exp, -0.5 * (Input / sigma) * (Input / sigma)));
                rotationSpeedM1 = 100 * VelocityPosition.z * (float)NormDist;
                motor1RotationAxis.transform.Rotate(Vector3.up * rotationSpeedM1 * Time.deltaTime);

            }

            if (Angle1 < 0 && Angle1 > -1)
            {
                motor1RotationAxis.transform.Rotate(Vector3.up * 50 * VelocityPosition.z * Time.deltaTime);
            }

            else
            {
                sigma = 1.8;
                double Input = (-(double)Angle1 + 360) / 60;
                double Exp = 2.718;
                NormDist = (1 / ((Mathf.Sqrt(2 * Mathf.PI)) * sigma)) * (Math.Pow(Exp, -0.5 * (Input / sigma) * (Input / sigma)));
                rotationSpeedM1 = 100 * VelocityPosition.z * (float)NormDist;
                motor1RotationAxis.transform.Rotate(Vector3.up * rotationSpeedM1 * Time.deltaTime);
            }
        }

        if (VelocityRotation.x > 0.1 || VelocityRotation.x < -0.1)
        {
            if (AngleGimbalX > 0 && AngleGimbalX < 90)
            {
                sigma = 1;
                double Input = (double)AngleGimbalX / 60;
                double Exp = 2.718;
                NormDist = 2 * (1 / ((Mathf.Sqrt(2 * Mathf.PI)) * sigma)) * (Math.Pow(Exp, -0.5 * (Input / sigma) * (Input / sigma)));
                rotationSpeedM2 = 100 * VelocityRotation.x * (float)NormDist;
                motor2RotationAxis.transform.Rotate(Vector3.left * rotationSpeedM2 * Time.deltaTime);
                motor3RotationAxis.transform.Rotate(Vector3.forward * rotationSpeedM2 / 8 * Time.deltaTime);
                motor1RotationAxis.transform.Rotate(Vector3.up * rotationSpeedM2 /3 * Time.deltaTime);

            }
            //if (AngleGimbalX <= 0 && AngleGimbalX > -1)
            //{
            //    motor2RotationAxis.transform.Rotate(Vector3.left * 1 * Time.deltaTime);
            //}
            if (AngleGimbalX < 359 && AngleGimbalX > 270)
            {
                sigma = 1;
                double Input = (-(double)AngleGimbalX + 360) / 60;
                double Exp = 2.718;
                NormDist = 2 * (1 / ((Mathf.Sqrt(2 * Mathf.PI)) * sigma)) * (Math.Pow(Exp, -0.5 * (Input / sigma) * (Input / sigma)));
                rotationSpeedM2 = 100 * VelocityRotation.x * (float)NormDist;
                motor2RotationAxis.transform.Rotate(Vector3.left * rotationSpeedM2 * Time.deltaTime);
                motor1RotationAxis.transform.Rotate(Vector3.up * rotationSpeedM2 /3 * Time.deltaTime);

            }

        }


        if (VelocityRotation.z > 0.1 || VelocityRotation.z < -0.1)
        {
            if (Angle2 > 0 && Angle2 < 90)
            {
                sigma = 1;
                double Input = (double)Angle2 / 60;
                double Exp = 2.718;
                NormDist = 2 * (1 / ((Mathf.Sqrt(2 * Mathf.PI)) * sigma)) * (Math.Pow(Exp, -0.5 * (Input / sigma) * (Input / sigma)));
                rotationSpeedM3 = VelocityRotation.z * (float)NormDist;
                motor3RotationAxis.transform.Rotate(-Vector3.forward * rotationSpeedM3 * 100 * Time.deltaTime);

            }
            //if (Angle2 <= 0 && Angle2 > -1)
            //{
            //motor3RotationAxis.transform.Rotate(-Vector3.forward * rotationSpeedM3 * 100 * Time.deltaTime);
            //print("cccc"); 

            if (Angle2 < 359 && Angle2 > 270)
            {
                sigma = 1;
                double Input = (-(double)Angle2 + 360) / 60;
                double Exp = 2.718;
                NormDist = 2.5 * (1 / ((Mathf.Sqrt(2 * Mathf.PI)) * sigma)) * (Math.Pow(Exp, -0.5 * (Input / sigma) * (Input / sigma)));
                rotationSpeedM3 = VelocityRotation.z * (float)NormDist;
                motor3RotationAxis.transform.Rotate(-Vector3.forward * rotationSpeedM3 * 100 * Time.deltaTime);
            }
        }
    }

    void GimbalLock()
    {
        if ((Angle1 < 300 && Angle1 > 60)||(Angle2 < 300 && Angle2 > 60)||(Angle3 < 300 && Angle3 > 60))
        {
            print("Warning: Motor Exceeds the Limit");
        }
        else
        {
            Smooth();
        }

    }
}
