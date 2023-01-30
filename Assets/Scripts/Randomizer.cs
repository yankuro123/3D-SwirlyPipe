using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomizer : ObstacleGenerator
{
    public PipeObstacle[] Prefab;

    public override void GenerateItems(PipeCreate pipe)
    {
        float angleStep = pipe.CurveAngle / pipe.curveSegCount;
        for(int i = 0; i < pipe.curveSegCount; i++)
        {
            PipeObstacle item = Instantiate<PipeObstacle>(Prefab[Random.Range(0, Prefab.Length)]);
            float pipeRotation = (Random.Range(0, pipe.PipeSegCount) + 0.5f) * 360f / pipe.PipeSegCount;
            item.Position(pipe, i * angleStep, pipeRotation);
        }
    }
}
