using System;
using UnityEngine;

[Serializable]
public class PLightsModule : BaseParticleModule
{
    [SerializeField]
    private float _ratio;

    [SerializeField]
    private bool _useRandomDistribution;

    [SerializeField]
    private Light _light;

    [SerializeField]
    private bool _useParticleColor;

    [SerializeField]
    private bool _sizeAffectsRange;

    [SerializeField]
    private bool _alphaAffectsIntensity;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _range;

    [SerializeField]
    private float _rangeMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _intensity;

    [SerializeField]
    private float _intensityMultiplier;

    [SerializeField]
    private int _maxLights;

    public override void SetModule(ParticleSystem particle)
    {
        var module = particle.lights;
        module.enabled = true;
        module.ratio = _ratio;
        module.useRandomDistribution = _useRandomDistribution;
        module.light = _light;
        module.useParticleColor = _useParticleColor;
        module.sizeAffectsRange = _sizeAffectsRange;
        module.alphaAffectsIntensity = _alphaAffectsIntensity;
        module.range = _range;
        module.rangeMultiplier = _rangeMultiplier;
        module.intensity = _intensity;
        module.intensityMultiplier = _intensityMultiplier;
        module.maxLights = _maxLights;
    }

    public override void RecordModule(ParticleSystem particle)
    {
        var module = particle.lights;
        _ratio = module.ratio;
        _useRandomDistribution = module.useRandomDistribution;
        _light = module.light;
        _useParticleColor = module.useParticleColor;
        _sizeAffectsRange = module.sizeAffectsRange;
        _alphaAffectsIntensity = module.alphaAffectsIntensity;
        _range = module.range;
        _rangeMultiplier = module.rangeMultiplier;
        _intensity = module.intensity;
        _intensityMultiplier = module.intensityMultiplier;
        _maxLights = module.maxLights;
    }
}