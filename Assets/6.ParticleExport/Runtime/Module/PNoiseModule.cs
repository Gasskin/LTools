using System;
using UnityEngine;

[Serializable]
public class PNoiseModule : BaseParticleModule
{
    [SerializeField]
    private bool _separateAxes;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _strength;

    [SerializeField]
    private float _strengthMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _strengthX;

    [SerializeField]
    private float _strengthXMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _strengthY;

    [SerializeField]
    private float _strengthYMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _strengthZ;

    [SerializeField]
    private float _strengthZMultiplier;

    [SerializeField]
    private float _frequency;

    [SerializeField]
    private bool _damping;

    [SerializeField]
    private int _octaveCount;

    [SerializeField]
    private float _octaveMultiplier;

    [SerializeField]
    private float _octaveScale;

    [SerializeField]
    private ParticleSystemNoiseQuality _quality;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _scrollSpeed;

    [SerializeField]
    private float _scrollSpeedMultiplier;

    [SerializeField]
    private bool _remapEnabled;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _remap;

    [SerializeField]
    private float _remapMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _remapX;

    [SerializeField]
    private float _remapXMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _remapY;

    [SerializeField]
    private float _remapYMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _remapZ;

    [SerializeField]
    private float _remapZMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _positionAmount;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _rotationAmount;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _sizeAmount;

    public override void SetModule(ParticleSystem particle)
    {
        var module = particle.noise;
        module.enabled = true;
        module.separateAxes = _separateAxes;
        module.strength = _strength;
        module.strengthMultiplier = _strengthMultiplier;
        module.strengthX = _strengthX;
        module.strengthXMultiplier = _strengthXMultiplier;
        module.strengthY = _strengthY;
        module.strengthYMultiplier = _strengthYMultiplier;
        module.strengthZ = _strengthZ;
        module.strengthZMultiplier = _strengthZMultiplier;
        module.frequency = _frequency;
        module.damping = _damping;
        module.octaveCount = _octaveCount;
        module.octaveMultiplier = _octaveMultiplier;
        module.octaveScale = _octaveScale;
        module.quality = _quality;
        module.scrollSpeed = _scrollSpeed;
        module.scrollSpeedMultiplier = _scrollSpeedMultiplier;
        module.remapEnabled = _remapEnabled;
        module.remap = _remap;
        module.remapMultiplier = _remapMultiplier;
        module.remapX = _remapX;
        module.remapXMultiplier = _remapXMultiplier;
        module.remapY = _remapY;
        module.remapYMultiplier = _remapYMultiplier;
        module.remapZ = _remapZ;
        module.remapZMultiplier = _remapZMultiplier;
        module.positionAmount = _positionAmount;
        module.rotationAmount = _rotationAmount;
        module.sizeAmount = _sizeAmount;
    }

    public override void RecordModule(ParticleSystem particle)
    {
        var module = particle.noise;
        _separateAxes = module.separateAxes;
        _strength = module.strength;
        _strengthMultiplier = module.strengthMultiplier;
        _strengthX = module.strengthX;
        _strengthXMultiplier = module.strengthXMultiplier;
        _strengthY = module.strengthY;
        _strengthYMultiplier = module.strengthYMultiplier;
        _strengthZ = module.strengthZ;
        _strengthZMultiplier = module.strengthZMultiplier;
        _frequency = module.frequency;
        _damping = module.damping;
        _octaveCount = module.octaveCount;
        _octaveMultiplier = module.octaveMultiplier;
        _octaveScale = module.octaveScale;
        _quality = module.quality;
        _scrollSpeed = module.scrollSpeed;
        _scrollSpeedMultiplier = module.scrollSpeedMultiplier;
        _remapEnabled = module.remapEnabled;
        _remap = module.remap;
        _remapMultiplier = module.remapMultiplier;
        _remapX = module.remapX;
        _remapXMultiplier = module.remapXMultiplier;
        _remapY = module.remapY;
        _remapYMultiplier = module.remapYMultiplier;
        _remapZ = module.remapZ;
        _remapZMultiplier = module.remapZMultiplier;
        _positionAmount = module.positionAmount;
        _rotationAmount = module.rotationAmount;
        _sizeAmount = module.sizeAmount;
    }
}