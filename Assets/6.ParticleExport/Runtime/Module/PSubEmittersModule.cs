using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PSubEmittersModule : BaseParticleModule
{
    [Serializable]
    public struct SubEmitterData
    {
        public ParticleSystemSubEmitterProperties Properties;
        public ParticleSystem SubParticleSystem;
        public ParticleSystemSubEmitterType SubEmitterType;
        public float Probability;
    }

    [SerializeField]
    private List<SubEmitterData> _subEmitterDataLists = new();

    public override void SetModule(ParticleSystem particle)
    {
        var module = particle.subEmitters;
        module.enabled = true;
        var count = module.subEmittersCount;
        for (var i = 0; i < count; i++)
        {
            var subEmitterData = _subEmitterDataLists[i];
            module.SetSubEmitterSystem(i, subEmitterData.SubParticleSystem);
            module.SetSubEmitterProperties(i, subEmitterData.Properties);
            module.SetSubEmitterEmitProbability(i, subEmitterData.Probability);
            module.SetSubEmitterType(i, subEmitterData.SubEmitterType);
        }
    }

    public override void RecordModule(ParticleSystem particle)
    {
        var module = particle.subEmitters;
        _subEmitterDataLists.Clear();
        var count = module.subEmittersCount;
        for (var i = 0; i < count; i++)
        {
            var properties = module.GetSubEmitterProperties(i);
            var subParticleSystem = module.GetSubEmitterSystem(i);
            var type = module.GetSubEmitterType(i);
            var probability = module.GetSubEmitterEmitProbability(i);
            var subEmitterData = new SubEmitterData();
            subEmitterData.Properties = properties;
            subEmitterData.SubParticleSystem = subParticleSystem;
            subEmitterData.Probability = probability;
            subEmitterData.SubEmitterType = type;
            _subEmitterDataLists.Add(subEmitterData);
        }
    }
}