using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeObstacle : MonoBehaviour
{
    private Transform rotater;
    private void Awake()
    {
        rotater = transform.GetChild(0);
    }
    public void Position(PipeCreate pipe, float CurveRotation, float ringRotation)
    {
        transform.SetParent(pipe.transform, false);
        transform.localRotation = Quaternion.Euler(0f, 0f, -CurveRotation);
        rotater.localPosition = new Vector3(0f, pipe.curveRad);
        rotater.localRotation = Quaternion.Euler(ringRotation, 0f, 0f);
    }
}
