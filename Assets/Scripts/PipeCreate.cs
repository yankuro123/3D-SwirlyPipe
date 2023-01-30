using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeCreate : MonoBehaviour
{
    public float PipeRadius;
    public int PipeSegCount;
    public float ringDistance;
    public float minCurveRad, maxCurveRad;
    public int minCurveSegCount, maxCurveSegCount;
    public ObstacleGenerator[] generators;

    public float curveSegCount
    {
        get
        {
            return CurveSegCount;
        }
    }
    public float curveRad { 
        get
        {
            return CurveRadius;
        } 
    }
    public float CurveAngle
    {
        get
        {
            return curveAngle;
        }
    }
    public float RelativeRotation
    {
        get
        {
            return relativeRotation;
        }
    }

    private Vector2[] UV;
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private int CurveSegCount;
    private float curveAngle;
    private float CurveRadius;
    private float relativeRotation;

    // Start is called before the first frame update
    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Pipe";
    }

    private void SetVertices()
    {
        vertices = new Vector3[PipeSegCount * CurveSegCount * 4];
        float uStep = /*(2f * Mathf.PI)*/ringDistance / CurveSegCount;
        curveAngle = uStep * CurveSegCount * (360f / (2f * Mathf.PI));
        CreateFrstQuadRing(uStep);
        int iDelta = PipeSegCount * 4;
        for(int u = 2, i = iDelta; u <= CurveSegCount; u++, i += iDelta)
        {
            CreateQuadRing(u * uStep, i);
        }
        mesh.vertices = vertices;
    }

    private void CreateQuadRing(float u, int i)
    {
        float vStep = (2f * Mathf.PI) / PipeSegCount;
        int RingOffset = PipeSegCount * 4;
        Vector3 Vertex = GetPointTorus(u, 0f);
        //Vector3 VertexA = GetPointTorus(0f, 0f);
        //Vector3 VertexB = GetPointTorus(u, 0f);
        for (int o = 1; o <= PipeSegCount; o++, i +=4)
        {
            vertices[i] = vertices[i - RingOffset + 2];
            vertices[i + 1] = vertices[i - RingOffset + 3];
            vertices[i + 2] = Vertex;
            vertices[i + 3] = Vertex = GetPointTorus(u, o * vStep);
        }
    }

    private void SetTriangles()
    {
        triangles = new int[PipeSegCount * CurveSegCount * 6];
        for(int i = 0, u = 0; i < triangles.Length; i += 6, u += 4)
        {
            triangles[i] = u;
            triangles[i + 1] = triangles[i + 4] = u + 2;
            triangles[i + 2] = triangles[i + 3] = u + 1;
            triangles[i + 5] = u + 3;
        }
        mesh.triangles = triangles;
    }
    private void CreateFrstQuadRing (float u)
    {
        float vStep = (2f * Mathf.PI) / PipeSegCount;
        Vector3 VertexA = GetPointTorus(0f, 0f);
        Vector3 VertexB = GetPointTorus(u, 0f);
        for (int i = 1, o = 0; i <= PipeSegCount; i++, o += 4)
        {
            vertices[o] = VertexA;
            vertices[o + 1] = VertexA = GetPointTorus(0f, i * vStep);
            vertices[o + 2] = VertexB;
            vertices[o + 3] = VertexB = GetPointTorus(u, i * vStep);
            //VertexA = GetPointTorus(0f, i * vStep);
            //VertexB = GetPointTorus(u, i * vStep);
        }
    }
    private Vector3 GetPointTorus(float u, float v)
    {
        Vector3 p;
        float r = (CurveRadius + PipeRadius * Mathf.Cos(v));
        p.x = r * Mathf.Sin(u);
        p.y = r * Mathf.Cos(u);
        p.z = PipeRadius * Mathf.Sin(v);
        return p;
    }
    public void AlignWith(PipeCreate pipe)
    {
        relativeRotation = UnityEngine.Random.Range(0, CurveSegCount) * 360f / PipeSegCount;
        transform.SetParent(pipe.transform, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0f, 0f, -pipe.curveAngle);
        transform.Translate(0f, pipe.CurveRadius, 0f);
        transform.Rotate(relativeRotation, 0f, 0f);
        transform.Translate(0f, -CurveRadius, 0f);
        transform.SetParent(pipe.transform.parent);
        transform.localScale = Vector3.one;
    }
    public void Generate()
    {
        CurveRadius = UnityEngine.Random.Range(minCurveRad, maxCurveRad);
        CurveSegCount = UnityEngine.Random.Range(minCurveSegCount, maxCurveSegCount + 1);
        mesh.Clear();
        SetVertices();
        SetUV();
        SetTriangles();
        mesh.RecalculateNormals();
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        generators[UnityEngine.Random.Range(0, generators.Length)].GenerateItems(this);
    }

    private void SetUV()
    {
        UV = new Vector2[vertices.Length];
        for(int i = 0; i < vertices.Length; i += 4)
        {
            UV[i] = Vector2.zero;
            UV[i + 1] = Vector2.right;
            UV[i + 2] = Vector2.up;
            UV[i + 3] = Vector2.down;
        }
        mesh.uv = UV;
    }
    //private void OnDrawGizmos()
    //{
    //    float uStep = (2f * Mathf.PI) / CurveSegCount;
    //    float vStep = (2f * Mathf.PI) / PipeSegCount;
    //    for(int z = 0; z < CurveSegCount; z++)
    //    {
    //        for (int i = 0; i < PipeSegCount; i++)
    //        {
    //            Vector3 point = GetPointTorus(z * uStep, i * vStep);
    //            Gizmos.color = new Color(1f, (float) i / PipeSegCount, (float) z / CurveSegCount);
    //            Gizmos.DrawSphere(point, 0.1f);
    //        }
    //    }
    //}
}
