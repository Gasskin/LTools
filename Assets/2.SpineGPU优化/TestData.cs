using System;
using System.Collections.Generic;
using Spine.Unity;
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
    private SkeletonAnimation _skeletonAnimation;

    private int _nowFrame;
    private float _frameDuration = 1 / 60f;
    private float _frameDown = -1f;
    
    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    private void OnEnable()
    {
        _nowFrame = 0;
    }

    private void Update()
    {
        var needUpdate = false;
        var time = Time.deltaTime;
        _frameDown -= time;
        if (_frameDown <= 0)
        {
            needUpdate = true;
        }
        if (needUpdate)
        {
            if (_meshFilter != null && _meshFilter.sharedMesh != null)
            {
                var vertices = _meshFilter.sharedMesh.vertices;
                var data = Data[_nowFrame];
                for (int i = 0; i < vertices.Length && i < data.Vertices.Count; i++)
                {
                    vertices[i] = data.Vertices[i];
                }
                _meshFilter.sharedMesh.vertices = vertices;
            }
            _nowFrame++;
            if (_nowFrame >= Data.Count)
            {
                _nowFrame = 0;
            }
        }
    }
}