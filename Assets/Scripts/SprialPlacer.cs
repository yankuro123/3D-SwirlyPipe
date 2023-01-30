using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprialPlacer : ObstacleGenerator
{
    public PipeObstacle[] Prefab;

    public override void GenerateItems(PipeCreate pipe)
    {
        float start = (Random.Range(0, pipe.PipeSegCount) + 0.5f);
        float direction = Random.value < 0.5f ? 1f : -1f;

        float angleStep = pipe.CurveAngle / pipe.curveSegCount;
        for (int i = 0; i < pipe.curveSegCount; i++)
        {
            PipeObstacle item = Instantiate<PipeObstacle>(Prefab[Random.Range(0, Prefab.Length)]);
            float pipeRotation = (start + i * direction) * 360 / pipe.PipeSegCount;                            
            item.Position(pipe, i * angleStep, pipeRotation);
        }
    }
}
