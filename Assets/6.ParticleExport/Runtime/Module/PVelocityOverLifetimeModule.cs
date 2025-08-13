using System;
using UnityEngine;

[Serializable]
public class PVelocityOverLifetimeModule : BaseParticleModule
{
    [SerializeField]
    private ParticleSystem.MinMaxCurve _x;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _y;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _z;

    [SerializeField]
    private float _xMultiplier;

    [SerializeField]
    private float _yMultiplier;

    [SerializeField]
    private float _zMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _orbitalX;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _orbitalY;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _orbitalZ;

    [SerializeField]
    private float _orbitalXMultiplier;

    [SerializeField]
    private float _orbitalYMultiplier;

    [SerializeField]
    private float _orbitalZMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _orbitalOffsetX;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _orbitalOffsetY;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _orbitalOffsetZ;

    [SerializeField]
    private float _orbitalOffsetXMultiplier;

    [SerializeField]
    private float _orbitalOffsetYMultiplier;

    [SerializeField]
    private float _orbitalOffsetZMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _radial;

    [SerializeField]
    private float _radialMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _speedModifier;

    [SerializeField]
    private float _speedModifierMultiplier;

    [SerializeField]
    private ParticleSystemSimulationSpace _space;

    public override void SetModule(ParticleSystem particle)
    {
        var module = particle.velocityOverLifetime;
        module.enabled = true;
        module.x = _x;
        module.y = _y;
        module.z = _z;
        module.xMultiplier = _xMultiplier;
        module.yMultiplier = _yMultiplier;
        module.zMultiplier = _zMultiplier;
        module.orbitalX = _orbitalX;
        module.orbitalY = _orbitalY;
        module.orbitalZ = _orbitalZ;
        module.orbitalXMultiplier = _orbitalXMultiplier;
        module.orbitalYMultiplier = _orbitalYMultiplier;
        module.orbitalZMultiplier = _orbitalZMultiplier;
        module.orbitalOffsetX = _orbitalOffsetX;
        module.orbitalOffsetY = _orbitalOffsetY;
        module.orbitalOffsetZ = _orbitalOffsetZ;
        module.orbitalOffsetXMultiplier = _orbitalOffsetXMultiplier;
        module.orbitalOffsetYMultiplier = _orbitalOffsetYMultiplier;
        module.orbitalOffsetZMultiplier = _orbitalOffsetZMultiplier;
        module.radial = _radial;
        module.radialMultiplier = _radialMultiplier;
        module.speedModifier = _speedModifier;
        module.speedModifierMultiplier = _speedModifierMultiplier;
        module.space = _space;
    }

    public override void RecordModule(ParticleSystem particle)
    {
        var module = particle.velocityOverLifetime;
        _x = module.x;
        _y = module.y;
        _z = module.z;
        _xMultiplier = module.xMultiplier;
        _yMultiplier = module.yMultiplier;
        _zMultiplier = module.zMultiplier;
        _orbitalX = module.orbitalX;
        _orbitalY = module.orbitalY;
        _orbitalZ = module.orbitalZ;
        _orbitalXMultiplier = module.orbitalXMultiplier;
        _orbitalYMultiplier = module.orbitalYMultiplier;
        _orbitalZMultiplier = module.orbitalZMultiplier;
        _orbitalOffsetX = module.orbitalOffsetX;
        _orbitalOffsetY = module.orbitalOffsetY;
        _orbitalOffsetZ = module.orbitalOffsetZ;
        _orbitalOffsetXMultiplier = module.orbitalOffsetXMultiplier;
        _orbitalOffsetYMultiplier = module.orbitalOffsetYMultiplier;
        _orbitalOffsetZMultiplier = module.orbitalOffsetZMultiplier;
        _radial = module.radial;
        _radialMultiplier = module.radialMultiplier;
        _speedModifier = module.speedModifier;
        _speedModifierMultiplier = module.speedModifierMultiplier;
        _space = module.space;
    }
}