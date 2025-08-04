using System;
using Spine.Unity;
using UnityEngine;

public class Test: MonoBehaviour
{
    private SkeletonAnimation _skeletonAnimation;

    private void Awake()
    {
        _skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    private void OnEnable()
    {
        _skeletonAnimation.OnPostProcessVertices -= OnPostProcessVertices;
        _skeletonAnimation.OnPostProcessVertices += OnPostProcessVertices;
    }

    private void OnDisable()
    {
        _skeletonAnimation.OnPostProcessVertices -= OnPostProcessVertices;
    }

    private void OnPostProcessVertices(MeshGeneratorBuffers buffers)
    {
        var vertices = buffers.vertexBuffer;
        if (vertices.Length > 0)
        {
            Debug.LogError(vertices[0]);
        }
    }
}