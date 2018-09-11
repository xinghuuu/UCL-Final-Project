using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraBased : MonoBehaviour
{
    public GameObject motor1RotationAxis;
    public GameObject motor2RotationAxis;
    public GameObject motor3RotationAxis;
    public GameObject Cube;
    public GameObject CameraGimbal;
    float rotationSpeedw, rotationSpeeda, rotationSpeeds, rotationSpeedd;
    public float MoveSpeed = 10;
    public Vector2 Velocity_;
    Vector2 PreviousPosition;
    public Camera GoPro;
    public Vector2 ObjectPosition;
    float RotationSpeedMotor1;
    float RotationSpeedMotor2;
    float RotationSpeedMotor3;
    float rotationSpeedM2,rotationSpeedM3;
    Vector2 DistancePreviousFrame;
    Vector2 DistancePPreviousFrame;
    public Vector2 DistanceInitialFrame;
    Vector2 InitialPosition;
    Vector2 PminusPP;
    public double NormDist;
    float AngleX;
    float AngleY;
    float AngleZ;
    public double translationSpeed = 10;
    Vector3 GimbalRotation;
    Vector3 PreviousRotation;
    Vector3 VelocityRotation;
    float AngleGimbalX;
    float AngleGimbalZ;


    // Use this for initialization
    void Start()
    {
        InitialPosition = GoPro.WorldToScreenPoint(Cube.transform.position);
        //NormDist = 1;
    }
    // Update is called once per frame
    void Update()
    {
        MoveObject();
        velocity();
        position();
        MoveGimbal();
        TestLimit();
        IMU();
    }



    // Get current 3D position of the camera
    void position()
    {
        AngleZ = motor3RotationAxis.transform.eulerAngles.z;
        AngleY = motor1RotationAxis.transform.eulerAngles.y;
        AngleX = motor2RotationAxis.transform.eulerAngles.x;
    }
    void velocity()
    {
        ObjectPosition = GoPro.WorldToScreenPoint(Cube.transform.position);
        DistanceInitialFrame = (ObjectPosition - InitialPosition) / Time.deltaTime;
        DistancePreviousFrame = ObjectPosition - PreviousPosition;
        Velocity_ = DistancePreviousFrame / Time.deltaTime;
        PminusPP = DistancePreviousFrame - DistancePPreviousFrame;
        DistancePPreviousFrame = DistancePreviousFrame;
        PreviousPosition = ObjectPosition;
        print(DistancePreviousFrame);
    }

    void IMU()
    {
        GimbalRotation = CameraGimbal.transform.eulerAngles;
        VelocityRotation = GimbalRotation - PreviousRotation;
        PreviousRotation = GimbalRotation;
        AngleGimbalX = CameraGimbal.transform.eulerAngles.x;
        AngleGimbalZ = CameraGimbal.transform.eulerAngles.z;
    }

    void GetRotationSpeed()
    {
        // cube moves up
        float V_0;
        double sigma;
        //double NormDist;
        //if (AngleZ > 0 && AngleZ < 90)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.J))
        {
            if (AngleZ < 0 && AngleZ > -1)
            {
                motor3RotationAxis.transform.Rotate(Vector3.forward * 10 * Time.deltaTime);
            }
            if (AngleZ > 270 && AngleZ < 360)
            {
                V_0 = 10;
                sigma = 0.8;
                double Input = (-(double)AngleZ + 360) / 60;
                double Exp = 2.718;
                NormDist = (1 / ((Mathf.Sqrt(2 * Mathf.PI)) * sigma)) * (Math.Pow(Exp, -0.5 * (Input / sigma) * (Input / sigma)));
                rotationSpeeds = (V_0 + (float)1 * PminusPP.y * Time.deltaTime) * (float)NormDist;
                motor3RotationAxis.transform.Rotate(Vector3.forward * rotationSpeeds * Time.deltaTime);
            }
            else
            {
                V_0 = 10;
                sigma = 0.65;
                double Input = (double)AngleZ / 60;
                double Exp = 2.718;
                NormDist = 0.8 * (1 / ((Mathf.Sqrt(2 * Mathf.PI)) * sigma)) * (Math.Pow(Exp, -0.5 * (Input / sigma) * (Input / sigma)));
                rotationSpeedw = (V_0 + (float)1 * PminusPP.y * Time.deltaTime) * (float)NormDist;
                motor3RotationAxis.transform.Rotate(Vector3.forward * rotationSpeedw * Time.deltaTime);
            }

        }

        // cube moves down 
        // if (AngleZ > 270 && AngleZ < 361)
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.H))
        {
            if (AngleZ > 0 && AngleZ < 90)
            {
                V_0 = 10;
                sigma = 0.65;
                double Input = (double)AngleZ / 60;
                double Exp = 2.718;
                NormDist = 1.1 * (1 / ((Mathf.Sqrt(2 * Mathf.PI)) * sigma)) * (Math.Pow(Exp, -0.5 * (Input / sigma) * (Input / sigma)));
                rotationSpeedw = (V_0 + (float)1 * PminusPP.y * Time.deltaTime) * (float)NormDist;
                motor3RotationAxis.transform.Rotate(-Vector3.forward * rotationSpeedw * Time.deltaTime);

            }
            if (AngleZ < 0 && AngleZ > -1)
            {
                motor3RotationAxis.transform.Rotate(-Vector3.forward * 10 * Time.deltaTime);
            }
            else
            {
                V_0 = 10;
                sigma = 0.8;
                double Input = (-(double)AngleZ + 360) / 60;
                double Exp = 2.718;
                NormDist = (1 / ((Mathf.Sqrt(2 * Mathf.PI)) * sigma)) * (Math.Pow(Exp, -0.5 * (Input / sigma) * (Input / sigma)));
                rotationSpeeds = (V_0 + (float)1 * PminusPP.y * Time.deltaTime) * (float)NormDist;
                motor3RotationAxis.transform.Rotate(-Vector3.forward * rotationSpeeds * Time.deltaTime);
            }
        }

        // cube moves left 
        //if (AngleY > -1 && AngleY <90)
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.O))
        {
            if (AngleY > -1 && AngleY < 90)
            {
                V_0 = 10;
                sigma = 0.7;
                double Input = (double)AngleY / 55; ;
                double Exp = 2.718;
                NormDist = 0.7 * (1 / ((Mathf.Sqrt(2 * Mathf.PI)) * sigma)) * (Math.Pow(Exp, -0.5 * (Input / sigma) * (Input / sigma)));
                rotationSpeedd = (V_0 + (float)1 * PminusPP.x * Time.deltaTime) * (float)NormDist;
                motor1RotationAxis.transform.Rotate(-Vector3.up * rotationSpeedd * Time.deltaTime);
            }
            else
            {
                V_0 = 10;
                sigma = 0.8;
                double Input = (-(double)AngleY + 360) / 55;
                double Exp = 2.718;
                NormDist =0.7* (1 / ((Mathf.Sqrt(2 * Mathf.PI)) * sigma)) * (Math.Pow(Exp, -0.5 * (Input / sigma) * (Input / sigma)));
                rotationSpeeda = (V_0 + (float)1 * PminusPP.x * Time.deltaTime) * (float)NormDist;
                motor1RotationAxis.transform.Rotate(-Vector3.up * rotationSpeeda * Time.deltaTime);
            }
        }

        //if (AngleY > 270 && AngleY < 361)
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.P))
        {
            if (AngleY > 270 && AngleY < 360)
            {
                V_0 = 10;
                sigma = 0.8;
                double Input = (-(double)AngleY + 360) / 55;
                double Exp = 2.718;
                NormDist = 0.9 * (1 / ((Mathf.Sqrt(2 * Mathf.PI)) * sigma)) * (Math.Pow(Exp, -0.5 * (Input / sigma) * (Input / sigma)));
                rotationSpeeda = (V_0 + (float)1 * PminusPP.x * Time.deltaTime) * (float)NormDist;
                motor1RotationAxis.transform.Rotate(Vector3.up * rotationSpeeda * Time.deltaTime);
            }
            if (AngleY < 0 && AngleY > -1)
            {
                motor1RotationAxis.transform.Rotate(Vector3.up * 10 * Time.deltaTime);
            }
            else
            {
                V_0 = 10;
                sigma = 0.8;
                double Input = (double)AngleY / 55; ;
                double Exp = 2.718;
                NormDist = 0.8 * (1 / ((Mathf.Sqrt(2 * Mathf.PI)) * sigma)) * (Math.Pow(Exp, -0.5 * (Input / sigma) * (Input / sigma)));
                rotationSpeedd = (V_0 + (float)1 * PminusPP.x * Time.deltaTime) * (float)NormDist;
                motor1RotationAxis.transform.Rotate(Vector3.up * rotationSpeedd * Time.deltaTime);
            }
        }



        ////////
        /// 
        /// 

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
                motor3RotationAxis.transform.Rotate(Vector3.forward * rotationSpeedM2 / 5 * Time.deltaTime);
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
            }

        }


        if (VelocityRotation.z > 0.1 || VelocityRotation.z < -0.1)
        {
            if (AngleZ > 0 && AngleZ < 90)
            {
                sigma = 1;
                double Input = (double)AngleZ / 60;
                double Exp = 2.718;
                NormDist = 2 * (1 / ((Mathf.Sqrt(2 * Mathf.PI)) * sigma)) * (Math.Pow(Exp, -0.5 * (Input / sigma) * (Input / sigma)));
                rotationSpeedM3 = VelocityRotation.z * (float)NormDist;
                motor3RotationAxis.transform.Rotate(-Vector3.forward * rotationSpeedM3 * 100 * Time.deltaTime);
            }
            //if (AngleZ <= 0 && AngleZ > -1)
            //{
            //motor3RotationAxis.transform.Rotate(-Vector3.forward * rotationSpeedM3 * 100 * Time.deltaTime);
            //print("cccc"); 
        }
        if (AngleZ < 359 && AngleZ > 270)
        {
            sigma = 1;
            double Input = (-(double)AngleZ + 360) / 60;
            double Exp = 2.718;
            NormDist = 2.5 * (1 / ((Mathf.Sqrt(2 * Mathf.PI)) * sigma)) * (Math.Pow(Exp, -0.5 * (Input / sigma) * (Input / sigma)));
            rotationSpeedM3 = VelocityRotation.z * (float)NormDist;
            motor3RotationAxis.transform.Rotate(-Vector3.forward * rotationSpeedM3 * 100 * Time.deltaTime);
        }
    }




    //Calculate the Distance between the camera and one single object 

    //Move the Object
    void MoveObject()
    {
        //Left and Right movement of the camera gimbal 
        if (Input.GetKey(KeyCode.A))
        {
            Cube.transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            Cube.transform.Translate(-Vector3.forward * MoveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.W))
        {
            Cube.transform.Translate(Vector3.up * MoveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            Cube.transform.Translate(-Vector3.up * MoveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            Cube.transform.Translate(Vector3.left * MoveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            Cube.transform.Translate(-Vector3.left * MoveSpeed * Time.deltaTime);
        }

    }

    // Translate and rotate the camera gimbal 
    void MoveGimbal()
    {
        if (Input.GetKey(KeyCode.P))
        {
            CameraGimbal.transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.O))
        {
            CameraGimbal.transform.Translate(-Vector3.forward * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.L))
        {
            CameraGimbal.transform.Translate(-Vector3.left * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.K))
        {
            CameraGimbal.transform.Translate(Vector3.left * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.H))
        {
            CameraGimbal.transform.Translate(Vector3.up * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.J))
        {
            CameraGimbal.transform.Translate(-Vector3.up * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.R))
        {
            CameraGimbal.transform.Rotate(Vector3.forward * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.T))
        {
            CameraGimbal.transform.Rotate(-Vector3.forward * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Y))
        {
            CameraGimbal.transform.Rotate(Vector3.left * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.U))
        {
            CameraGimbal.transform.Rotate(-Vector3.left * MoveSpeed * Time.deltaTime);
        }


    }
    void TestLimit()
    {
        
        if ((AngleZ < 300 && AngleZ > 60) || (AngleY < 300 && AngleY > 60) ||(AngleGimbalX < 300 && AngleGimbalX > 60))
        {
            print("Warning: Gimbal Exceeds Limit");
        }

        else
        {
            GetRotationSpeed();
        }

    }

}
