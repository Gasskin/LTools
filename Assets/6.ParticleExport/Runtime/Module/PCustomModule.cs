using System;
using UnityEngine;

public class PCustomModule : BaseParticleModule
{
    [Serializable]
    struct CustomData
    {
        public ParticleSystemCustomDataMode Mode;
        public int ComponentCount;
        public ParticleSystem.MinMaxCurve CurveX;
        public ParticleSystem.MinMaxCurve CurveY;
        public ParticleSystem.MinMaxCurve CurveZ;
        public ParticleSystem.MinMaxCurve CurveW;
        public ParticleSystem.MinMaxGradient Color;
    }

    [SerializeField]
    private CustomData _customData1;

    [SerializeField]
    private CustomData _customData2;

    public override void SetModule(ParticleSystem particle)
    {
        var module = particle.customData;
        module.enabled = true;
        SetCustomData(_customData1, module, ParticleSystemCustomData.Custom1);
        SetCustomData(_customData2, module, ParticleSystemCustomData.Custom2);
    }

    public override void RecordModule(ParticleSystem particle)
    {
        _customData1 = SetCustomDataByType(particle, ParticleSystemCustomData.Custom1);
        _customData2 = SetCustomDataByType(particle, ParticleSystemCustomData.Custom2);
    }

    private CustomData SetCustomDataByType(ParticleSystem particle, ParticleSystemCustomData type)
    {
        var module = particle.customData;
        var mode = module.GetMode(type);
        var customData = new CustomData();
        customData.Mode = mode;
        if (mode == ParticleSystemCustomDataMode.Vector)
        {
            var count = module.GetVectorComponentCount(type);
            var curveX = module.GetVector(type, 0); // x分量
            var curveY = module.GetVector(type, 1); // y分量
            var curveZ = module.GetVector(type, 2); // z分量
            var curveW = module.GetVector(type, 3); // w分量
            customData.ComponentCount = count;
            customData.CurveX = curveX;
            customData.CurveY = curveY;
            customData.CurveZ = curveZ;
            customData.CurveW = curveW;
        }
        else if (mode == ParticleSystemCustomDataMode.Color)
        {
            var color = module.GetColor(ParticleSystemCustomData.Custom1);
            customData.Color = color;
        }

        return customData;
    }

    private void SetCustomData(CustomData customData, ParticleSystem.CustomDataModule module, ParticleSystemCustomData type)
    {
        module.SetMode(type, customData.Mode);
        if (customData.Mode == ParticleSystemCustomDataMode.Color)
        {
            module.SetColor(type, customData.Color);
        }
        else if (customData.Mode == ParticleSystemCustomDataMode.Vector)
        {
            module.SetVectorComponentCount(type, customData.ComponentCount);
            module.SetVector(type, 0, customData.CurveX);
            module.SetVector(type, 1, customData.CurveY);
            module.SetVector(type, 2, customData.CurveZ);
            module.SetVector(type, 3, customData.CurveW);
        }
    }
}