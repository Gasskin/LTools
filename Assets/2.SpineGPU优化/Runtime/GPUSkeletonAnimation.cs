using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OneFrameVertices
{
    public List<Vector3> Vertices = new();
}

public class GPUSkeletonAnimation : MonoBehaviour
{
    public bool UseColor;
    public float MaxX;
    public float MinX;
    public float MaxY;
    public float MinY;
    public int FrameCount;
    public int FramePerSecond;
    public Texture2D AnimaTexture;

    public List<OneFrameVertices> FrameVertices = new();

    private MeshFilter _meshFilter;
    private int _nowFrame;
    private float _frameDuration;
    private float _frameDown = -1f;
    private Color32[] _color32Array;

    private void OnEnable()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _nowFrame = 0;
        _frameDown = 0;
        _frameDuration = 1f / FramePerSecond;
        _color32Array = AnimaTexture.GetPixels32();
        // for (int i = 0; i < FrameVertices.Count; i++)
        // {
        //     for (int j = 0; j < FrameVertices[i].Vertices.Count; j++)
        //     {
        //         Debug.LogError($"{FrameVertices[i].Vertices[j]}===={AnimaTexture.GetPixel(i, j)}");
        //     }
        // }
    }

    private void Update()
    {
        _frameDown -= Time.deltaTime;
        if (_frameDown <= 0)
        {
            _frameDown = _frameDuration;
            if (_meshFilter != null && _meshFilter.sharedMesh != null)
            {
                var vertices = _meshFilter.sharedMesh.vertices;
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] = GetVertex(_nowFrame, i);
                }
                _meshFilter.sharedMesh.vertices = vertices;
            }
            _nowFrame++;
            if (_nowFrame >= FrameCount)
            {
                _nowFrame = 0;
            }
        }
    }

    private Vector3 GetVertex(int frame, int index)
    {
        if (UseColor)
        {
            var color32 = _color32Array[frame * AnimaTexture.width + index];
            var vertex = new Vector3();
            var x = UnpackBit88ToFloat(color32.r, color32.g);
            var y = UnpackBit88ToFloat(color32.b, color32.a);
            vertex.x = x * (MaxX - MinX) + MinX;
            vertex.y = y * (MaxY - MinY) + MinY;
            return vertex;
        }
        else
        {
            var vertex = FrameVertices[frame].Vertices[index];
            vertex.x = vertex.x * (MaxX - MinX) + MinX;
            vertex.y = vertex.y * (MaxY - MinY) + MinY;
            return vertex;
        }
    }
    
    private float UnpackBit88ToFloat(byte high, byte low)
    {
        int intVal = (high << 8) | low;         
        return intVal / 65535f;                
    }
}