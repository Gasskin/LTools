using System.Collections.Generic;
using System.IO;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

public partial class ParticleExportEditor
{
    private const string SAVE_PATH = "Assets/6.ParticleExport/Exports";

    private GameObject _source;

    public ParticleExportEditor(GameObject source)
    {
        _source = source;
    }

    public void Export()
    {
        var newPrefab = Object.Instantiate(_source);
        newPrefab.name = _source.name + "_export";

        var particles = newPrefab.GetComponentsInChildren<ParticleSystem>(true);


        // 不导出的particle
        var filter = new HashSet<ParticleSystem>();

        // UIParticle不导出
        // var uiParticles = newPrefab.GetComponentsInChildren<UIParticle>(true);
        // foreach (var particle in uiParticles)
        // {
        //     foreach (var particleSystem in particle.particles)
        //         filter.Add(particleSystem);
        // }

        // 如果一个particle开启了sub emitters，他和他的子粒子不导出
        foreach (var particle in particles)
        {
            if (particle.subEmitters.enabled)
            {
                for (int i = 0; i < particle.subEmitters.subEmittersCount; i++)
                {
                    var p = particle.subEmitters.GetSubEmitterSystem(i);
                    filter.Add(p);
                }
                filter.Add(particle);
            }
        }

        // 需要K动画的粒子，不导出
        CheckHasAnimation(newPrefab, filter);

        // 删除旧数据
        var prefabPath = $"{SAVE_PATH}/{newPrefab.name}.prefab";
        if (File.Exists(prefabPath))
            File.Delete(prefabPath);

        for (int i = 0; i < particles.Length; i++)
        {
            var particle = particles[i];
            if (filter.Contains(particle))
                continue;
            // var oneParticleData = new GameObject($"{i}_{particle.gameObject.name}_data");
            var player = particle.gameObject.AddComponent<ParticlePlayer>();
            AddOneModuleAndRecord<PMainModule>(particle);
            AddOneModuleAndRecord<PParticleRendererModule>(particle);
            if (particle.emission.enabled)
                AddOneModuleAndRecord<PEmissionModule>(particle);
            if (particle.shape.enabled)
                AddOneModuleAndRecord<PShapeModule>(particle);
            if (particle.velocityOverLifetime.enabled)
                AddOneModuleAndRecord<PVelocityOverLifetimeModule>(particle);
            if (particle.limitVelocityOverLifetime.enabled)
                AddOneModuleAndRecord<PLimitVelocityOverLifetimeModule>(particle);
            if (particle.inheritVelocity.enabled)
                AddOneModuleAndRecord<PInheritVelocityModule>(particle);
            if (particle.lifetimeByEmitterSpeed.enabled)
                AddOneModuleAndRecord<PLifetimeByEmitterSpeedModule>(particle);
            if (particle.forceOverLifetime.enabled)
                AddOneModuleAndRecord<PForceOverLifetimeModule>(particle);
            if (particle.colorOverLifetime.enabled)
                AddOneModuleAndRecord<PColorOverLifetimeModule>(particle);
            if (particle.colorBySpeed.enabled)
                AddOneModuleAndRecord<PColorBySpeedModule>(particle);
            if (particle.sizeOverLifetime.enabled)
                AddOneModuleAndRecord<PSizeOverLifetimeModule>(particle);
            if (particle.sizeBySpeed.enabled)
                AddOneModuleAndRecord<PSizeBySpeedModule>(particle);
            if (particle.rotationOverLifetime.enabled)
                AddOneModuleAndRecord<PRotationOverLifetimeModule>(particle);
            if (particle.rotationBySpeed.enabled)
                AddOneModuleAndRecord<PRotationBySpeedModule>(particle);
            if (particle.externalForces.enabled)
                AddOneModuleAndRecord<PExternalForcesModule>(particle);
            if (particle.noise.enabled)
                AddOneModuleAndRecord<PNoiseModule>(particle);
            if (particle.collision.enabled)
                AddOneModuleAndRecord<PCollisionModule>(particle);
            if (particle.trigger.enabled)
                AddOneModuleAndRecord<PTriggerModule>(particle);
            if (particle.textureSheetAnimation.enabled)
                AddOneModuleAndRecord<PTextureSheetAnimationModule>(particle);
            if (particle.lights.enabled)
                AddOneModuleAndRecord<PLightsModule>(particle);
            if (particle.trails.enabled)
                AddOneModuleAndRecord<PTrailModule>(particle);
            Object.DestroyImmediate(particle.GetComponent<ParticleSystemRenderer>());
            Object.DestroyImmediate(particle.GetComponent<ParticleSystem>());
        }
        
        PrefabUtility.SaveAsPrefabAsset(newPrefab, prefabPath);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    private void CheckHasAnimation(GameObject target, HashSet<ParticleSystem> filter)
    {
        var animators = target.GetComponentsInChildren<UnityEngine.Animator>(true);
        var animations = target.GetComponentsInChildren<UnityEngine.Animation>(true);
        var clips = new List<AnimationClip>();
        foreach (var anima in animations)
        {
            foreach (AnimationState state in anima)
            {
                clips.Add(state.clip);
            }
        }
        foreach (var animator in animators)
        {
            foreach (var clip in animator.runtimeAnimatorController.animationClips)
            {
                clips.Add(clip);
            }
        }
        foreach (var clip in clips)
        {
            var curveBindings = AnimationUtility.GetCurveBindings(clip);
            foreach (var binding in curveBindings)
            {
                if (binding.type == typeof(ParticleSystemRenderer))
                {
                    var o = target.transform.Find(binding.path);
                    if (o != null)
                    {
                        var p = o.GetComponent<ParticleSystem>();
                        filter.Add(p);
                    }
                    else
                    {
                        Debug.LogError($"Clip路径依赖失败\nGo:{target.name}\nClip:{clip.name}\nPath:{binding.path}");
                    }
                }
            }
        }
    }


    private void AddOneModuleAndRecord<T>(ParticleSystem particle) where T : BaseParticleModule
    {
        var module = particle.gameObject.AddComponent<T>();
        module.RecordModule(particle);
    }
}