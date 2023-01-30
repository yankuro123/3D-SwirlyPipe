using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSystem : MonoBehaviour
{
    public PipeCreate PipePrefab;
    public int PipeCount;
    private PipeCreate[] pipes;
    private void Awake()
    {
        pipes = new PipeCreate[PipeCount];
        for(int i = 0; i < pipes.Length; i++)
        {
            PipeCreate pipe = pipes[i] = Instantiate<PipeCreate>(PipePrefab);
            pipe.transform.SetParent(transform, false);
            pipe.Generate();
            if(i > 0)
            {
                pipe.AlignWith(pipes[i - 1]);
            }
        }
        AlignNextPipeWithOrigin();
    }
    public PipeCreate SetupFirstPipe()
    {
        transform.localPosition = new Vector3(0f, -pipes[1].curveRad);
        return pipes[1];
    }

    internal PipeCreate SetupNextPipe()
    {
        ShiftPipes();
        AlignNextPipeWithOrigin();
        pipes[pipes.Length - 1].Generate();
        pipes[pipes.Length - 1].AlignWith(pipes[pipes.Length - 2]);
        transform.localPosition = new Vector3(0f, -pipes[1].curveRad);
        return pipes[1];
    }

    private void AlignNextPipeWithOrigin()
    {
        Transform toAlign = pipes[1].transform;
        for (int i = 0; i < pipes.Length; i++)
        {
            if(i != 1)
            {
                pipes[i].transform.SetParent(toAlign);
            }
        }
        toAlign.localPosition = Vector3.zero;
        toAlign.localRotation = Quaternion.identity;
        for(int i = 0; i < pipes.Length; i++)
        {
            if(i != 1)
            {
                pipes[i].transform.SetParent(transform);
            }
        }
    }

    private void ShiftPipes()
    {
        PipeCreate temp = pipes[0];
        for(int i = 1; i < pipes.Length; i++)
        {
            pipes[i - 1] = pipes[i];
        }
        pipes[pipes.Length - 1] = temp;
    }
}
