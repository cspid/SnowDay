﻿using System.Collections.Generic;
using UnityEngine;

public class SnowDayCamera : MonoBehaviour
{
    [Header("Players")]
    public List<Transform> Players;

    [Header("Camera Horizontal Rotation")]
    public int Resolution = 20;
    public int CurrentStep = 1;


    public float CameraYOffset = 10;
    public float Radius = 10;

    //Time to Destination
    public float SmoothTime = .5f;

    //Camera Movement Multipliers
    public float XZMultiplier = 1f;
    public float YMultiplier = 0.5f;

    //Border Buffer
    public float EdgeBorderBuffer = 8;

    private Camera cam;

    private float thetaStep;
    private Vector3 camVelocity;

    private Vector3 nextCameraPos;

    private void Start()
    {
        cam = Camera.main;
        thetaStep = (2f * Mathf.PI) / Resolution;

        CameraDistance();
        PopCamera();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        CameraDistance();
        MoveCamera();
    }

    /// <summary>
    /// Sets the Radius and the Y Offset of the Camera,
    /// based on the Greatest distance from the center of of the camera Gimbel.
    /// </summary>
    private void CameraDistance()
    {
        float greatestDistance = 0;
        for (int i = 0; i < Players.Count; i++)
        {
            if (!Players[i].gameObject.activeSelf)
                continue;

            float distanceBetween = Vector3.Distance(transform.position, Players[i].position);
            if (distanceBetween > greatestDistance)
            {
                greatestDistance = distanceBetween;
            }
        }

        Radius = greatestDistance + EdgeBorderBuffer * XZMultiplier;
        CameraYOffset = greatestDistance + EdgeBorderBuffer * YMultiplier;
    }

    /// <summary>
    /// Instantly Moves Camera to Current Rotation Step
    /// </summary>
    void PopCamera()
    {
        transform.position = FindAveragePosition();
        nextCameraPos = transform.position + new Vector3(Radius * Mathf.Cos(thetaStep * CurrentStep), CameraYOffset, Radius * Mathf.Sin(thetaStep * CurrentStep));//Radius * Mathf.Sin(thetaStep * CurrentStep));
        cam.transform.position = nextCameraPos;
        cam.transform.LookAt(transform.position, Vector3.up);
    }

    /// <summary>
    /// Smoothly moves the camera between rotation steps.
    /// </summary>
    private void MoveCamera()
    {
        transform.position = FindAveragePosition();
        nextCameraPos = transform.position + new Vector3(Radius * Mathf.Cos(thetaStep * CurrentStep), CameraYOffset, Radius * Mathf.Sin(thetaStep * CurrentStep));//Radius * Mathf.Sin(thetaStep * CurrentStep));
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, nextCameraPos, ref camVelocity, SmoothTime);
        cam.transform.LookAt(transform.position, Vector3.up);
    }

    /// <summary>
    /// Finds the Average Position between all the players
    /// </summary>
    /// <returns>Center Position between the players</returns>
    private Vector3 FindAveragePosition()
    {
        if (Players == null)
            return Vector3.zero;

        Vector3 averagePosition = new Vector3();
        int numofPlayers = 0;
        for (int i = 0; i < Players.Count; i++)
        {
            if (!Players[i].gameObject.activeSelf)
                continue;

            averagePosition += Players[i].position;
            numofPlayers++;
        }
        averagePosition = averagePosition / numofPlayers;
        averagePosition.y = transform.position.y;
        return averagePosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 AveragePosition = FindAveragePosition();
        Gizmos.DrawWireSphere(AveragePosition, 1);
        Gizmos.color = Color.red;
        float theta = 0;
        float thetaStep = (2f * Mathf.PI) / Resolution;

        for (int i = 0; i < Resolution; i++)
        {
            Vector3 pos = transform.position + new Vector3(Radius * Mathf.Cos(theta), CameraYOffset, Radius * Mathf.Sin(theta));
            Gizmos.DrawWireSphere(pos, 1);
            theta += thetaStep;
        }
    }
}