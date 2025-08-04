using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class FrameVerticesData
{
    public List<Vector3> Vertices = new();
}

public class TestData : MonoBehaviour
{
    public List<FrameVerticesData> Data = new();
    
    private MeshFilter _meshFilter;
    private int _nowFrame;
    private float _frameDuration = 1 / 30f;
    
    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
    }

    private void OnEnable()
    {
        _nowFrame = 0;
    }

    private void LateUpdate()
    {
        if (_nowFrame >= Data.Count)
            _nowFrame = 0;
        var data = Data[_nowFrame];
        if (_meshFilter != null && _meshFilter.sharedMesh != null)
        {
            var vertices = _meshFilter.sharedMesh.vertices;
            for (int i = 0; i < vertices.Length && i < data.Vertices.Count; i++)
            {
                vertices[i] = data.Vertices[i];
            }
            _meshFilter.sharedMesh.vertices = vertices;
        }
        _nowFrame++;
    }
}