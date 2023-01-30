using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObstacleGenerator : MonoBehaviour
{
    public abstract void GenerateItems(PipeCreate pipe);
}
