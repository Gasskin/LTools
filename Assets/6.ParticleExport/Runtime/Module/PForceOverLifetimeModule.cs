using System;
using UnityEngine;

[Serializable]
public class PForceOverLifetimeModule : BaseParticleModule
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
    private ParticleSystemSimulationSpace _space;

    [SerializeField]
    private bool _randomized;

    public override void SetModule(ParticleSystem particle)
    {
        var module = particle.forceOverLifetime;
        module.enabled = true;
        module.x = _x;
        module.y = _y;
        module.z = _z;
        module.xMultiplier = _xMultiplier;
        module.yMultiplier = _yMultiplier;
        module.zMultiplier = _zMultiplier;
        module.space = _space;
        module.randomized = _randomized;
    }

    public override void RecordModule(ParticleSystem particle)
    {
        var module = particle.forceOverLifetime;
        _x = module.x;
        _y = module.y;
        _z = module.z;
        _xMultiplier = module.xMultiplier;
        _yMultiplier = module.yMultiplier;
        _zMultiplier = module.zMultiplier;
        _space = module.space;
        _randomized = module.randomized;
    }
}