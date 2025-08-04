using System.Linq;
using Spine.Unity;
using UnityEditor;
using UnityEngine;

public partial class SpineEditor
{
    [MenuItem("Assets/LTools/SpineGPU优化/Test")]
    public static void CreateAnimationTexture()
    {
        var select = Selection.activeGameObject;
        if (select == null)
            return;
        _meshFilter = select.GetComponent<MeshFilter>();
        _meshRenderer = select.GetComponent<MeshRenderer>();
        _skeletonAnimation = select.GetComponent<SkeletonAnimation>();

        _skeletonAnimation.Initialize(true);
        var animations = _skeletonAnimation.skeletonDataAsset.GetSkeletonData(true).Animations;
        var main = animations.Find((animation => animation.Name == "main"));
        if (main == null)
            return;
        var frameDuration = 1f / 60;
        var frameCount = Mathf.CeilToInt(main.Duration / frameDuration);
        var testData = select.GetComponent<TestData>();
        testData.Data.Clear();
        Debug.LogError($"frame count:{frameCount}");
        for (int i = 1; i <= frameCount; i++)
        {
            _skeletonAnimation.Update(i * frameDuration);
            _skeletonAnimation.LateUpdateMesh();
            var vertices = _meshFilter.sharedMesh.vertices;
            var frameVerticesData = new FrameVerticesData();
            foreach (var v in vertices)
                frameVerticesData.Vertices.Add(v);
            testData.Data.Add(frameVerticesData);
        }
    }
}