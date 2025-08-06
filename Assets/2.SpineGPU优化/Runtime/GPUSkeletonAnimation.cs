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
    private static readonly int _frameIndex = Shader.PropertyToID("_FrameIndex");
    
    public float MaxX;
    public float MinX;
    public float MaxY;
    public float MinY;
    public int FrameCount;
    public int FramePerSecond;
    public Texture2D AnimaTexture;

    private int _nowFrame;
    private float _frameDuration;
    private float _frameDown = -1f;
    private Color32[] _color32Array;
    private Material _material;
    
    private void OnEnable()
    {
        _nowFrame = 0;
        _frameDown = 0;
        _frameDuration = 1f / FramePerSecond;
        _color32Array = AnimaTexture.GetPixels32();
        _material = GetComponent<MeshRenderer>().sharedMaterial;
    }

    private void Update()
    {
        _frameDown -= Time.deltaTime;
        if (_frameDown <= 0)
        {
            _frameDown = _frameDuration;
            // if (_meshFilter != null && _meshFilter.sharedMesh != null)
            // {
            //     var vertices = _meshFilter.sharedMesh.vertices;
            //     for (int i = 0; i < vertices.Length; i++)
            //     {
            //         vertices[i] = GetVertex(_nowFrame, i);
            //     }
            //     _meshFilter.sharedMesh.vertices = vertices;
            // }
            _material.SetFloat(_frameIndex, _nowFrame);
            _nowFrame++;
            // if (_nowFrame >= FrameCount)
            // {
            //     _nowFrame = 0;
            // }
        }
    }

    private Vector3 GetVertex(int frame, int index)
    {
        var color32 = _color32Array[frame * AnimaTexture.width + index];
        var vertex = new Vector3();
        var x = UnpackBit88ToFloat(color32.r, color32.g);
        var y = UnpackBit88ToFloat(color32.b, color32.a);
        vertex.x = x * (MaxX - MinX) + MinX;
        vertex.y = y * (MaxY - MinY) + MinY;
        return vertex;
    }
    
    private float UnpackBit88ToFloat(byte high, byte low)
    {
        int intVal = (high << 8) | low;         
        return intVal / 65535f;                
    }
}