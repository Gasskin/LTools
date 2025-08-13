using System;
using UnityEngine;

[Serializable]
public class PTrailModule : BaseParticleModule
{
    [SerializeField]
    private ParticleSystemTrailMode _mode;

    [SerializeField]
    private float _ratio;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _lifetime;

    [SerializeField]
    private float _lifetimeMultiplier;

    [SerializeField]
    private float _minVertexDistance;

    [SerializeField]
    private ParticleSystemTrailTextureMode _textureMode;

    [SerializeField]
    private bool _worldSpace;

    [SerializeField]
    private bool _dieWithParticles;

    [SerializeField]
    private bool _sizeAffectsWidth;

    [SerializeField]
    private bool _sizeAffectsLifetime;

    [SerializeField]
    private bool _inheritParticleColor;

    [SerializeField]
    private ParticleSystem.MinMaxGradient _colorOverLifetime;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _widthOverTrail;

    [SerializeField]
    private float _widthOverTrailMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxGradient _colorOverTrail;

    [SerializeField]
    private bool _generateLightingData;

    [SerializeField]
    private int _ribbonCount;

    [SerializeField]
    private float _shadowBias;

    [SerializeField]
    private bool _splitSubEmitterRibbons;

    [SerializeField]
    private bool _attachRibbonsToTransform;

    public override void SetModule(ParticleSystem particle)
    {
        var module = particle.trails;
        module.enabled = true;
        module.mode = _mode;
        module.ratio = _ratio;
        module.lifetime = _lifetime;
        module.lifetimeMultiplier = _lifetimeMultiplier;
        module.minVertexDistance = _minVertexDistance;
        module.textureMode = _textureMode;
        module.worldSpace = _worldSpace;
        module.dieWithParticles = _dieWithParticles;
        module.sizeAffectsWidth = _sizeAffectsWidth;
        module.sizeAffectsLifetime = _sizeAffectsLifetime;
        module.inheritParticleColor = _inheritParticleColor;
        module.colorOverLifetime = _colorOverLifetime;
        module.widthOverTrail = _widthOverTrail;
        module.widthOverTrailMultiplier = _widthOverTrailMultiplier;
        module.colorOverTrail = _colorOverTrail;
        module.generateLightingData = _generateLightingData;
        module.ribbonCount = _ribbonCount;
        module.shadowBias = _shadowBias;
        module.splitSubEmitterRibbons = _splitSubEmitterRibbons;
        module.attachRibbonsToTransform = _attachRibbonsToTransform;
    }

    public override void RecordModule(ParticleSystem particle)
    {
        var module = particle.trails;
        _mode = module.mode;
        _ratio = module.ratio;
        _lifetime = module.lifetime;
        _lifetimeMultiplier = module.lifetimeMultiplier;
        _minVertexDistance = module.minVertexDistance;
        _textureMode = module.textureMode;
        _worldSpace = module.worldSpace;
        _dieWithParticles = module.dieWithParticles;
        _sizeAffectsWidth = module.sizeAffectsWidth;
        _sizeAffectsLifetime = module.sizeAffectsLifetime;
        _inheritParticleColor = module.inheritParticleColor;
        _colorOverLifetime = module.colorOverLifetime;
        _widthOverTrail = module.widthOverTrail;
        _widthOverTrailMultiplier = module.widthOverTrailMultiplier;
        _colorOverTrail = module.colorOverTrail;
        _generateLightingData = module.generateLightingData;
        _ribbonCount = module.ribbonCount;
        _shadowBias = module.shadowBias;
        _splitSubEmitterRibbons = module.splitSubEmitterRibbons;
        _attachRibbonsToTransform = module.attachRibbonsToTransform;
    }
}