using System;
using System.Collections.Generic;
using UnityEngine;

public class GPUSkeletonAnimation : MonoBehaviour
{
    public float MaxX;
    public float MinX;
    public float MaxY;
    public float MinY;
    public int FrameCount;
    public int FramePerSecond;
    public Texture2D AnimaTexture;

    public List<Vector3> Vertices = new();

    private MeshFilter _meshFilter;
    private int _nowFrame;
    private float _frameDuration;
    private float _frameDown = -1f;

    private void OnEnable()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _nowFrame = 0;
        _frameDown = 0;
        _frameDuration = 1f / FramePerSecond;
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
        // 第0帧顶点位置（绝对坐标）
        var baseColor = AnimaTexture.GetPixel(index, 0);
        var baseX = baseColor.r * (MaxX - MinX) + MinX;
        var baseY = baseColor.g * (MaxY - MinY) + MinY;
        Vector3 basePos = new Vector3(baseX, baseY, 0);

        if (frame == 0)
            return basePos;

        // 相对第0帧的偏移量
        var deltaColor = AnimaTexture.GetPixel(index, frame);
        var dx = deltaColor.r * (MaxX - MinX) + MinX;
        var dy = deltaColor.g * (MaxY - MinY) + MinY;
        Vector3 offset = new Vector3(dx, dy, 0);

        return basePos + offset;
    }
}