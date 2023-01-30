using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PipeSystem PipeSystem;
    public float velocity;
    public float rotationVelocity;

    private PipeCreate currPipe;
    private float Traveled;
    private float deltaToRotation;
    private float systemRotation;
    private Transform World;
    private float worldRotation, avatarRotation;
    private Transform world, rotater;

    // Start is called before the first frame update
    void Start()
    {
        World = PipeSystem.transform.parent;
        rotater = transform.GetChild(0);
        currPipe = PipeSystem.SetupFirstPipe();
        SetupCurrPipe();
    }



    // Update is called once per frame
    void Update()
    {
        if (velocity < 7f)
        {
            velocity += Time.deltaTime / 50;
        }
        else
            velocity = 7f;
        float delta = velocity * Time.deltaTime;
        Traveled += delta;
        systemRotation += delta * deltaToRotation;
        if(systemRotation >= currPipe.CurveAngle)
        {
            delta = (systemRotation - currPipe.CurveAngle) / deltaToRotation;
            currPipe = PipeSystem.SetupNextPipe();
            SetupCurrPipe();
            systemRotation = delta * deltaToRotation;
        }
        PipeSystem.transform.localRotation = Quaternion.Euler(0f, 0f, systemRotation);
        UpdateAvatarRotation();
    }

    private void UpdateAvatarRotation()
    {
        avatarRotation += rotationVelocity * Time.deltaTime * Input.GetAxis("Horizontal");
        if(avatarRotation < 0f) 
            {
                avatarRotation += 360f;
            }
        else if(avatarRotation >= 360f)
        {
            avatarRotation -= 360f;
        }
        rotater.localRotation = Quaternion.Euler(avatarRotation, 0f, 0f);
    }

    private void SetupCurrPipe()
    {
        deltaToRotation = 360f / (2f * Mathf.PI * currPipe.curveRad);
        worldRotation += currPipe.RelativeRotation;
        if(worldRotation < 0f)
        {
            worldRotation += 360f;
        }
        else if (worldRotation >= 360f)
        {
            worldRotation -= 360f;
        }
        World.localRotation = Quaternion.Euler(worldRotation, 0f, 0f);
    }
}
