using System;
using UnityEngine;

[Serializable]
public class PRotationOverLifetimeModule : BaseParticleModule
{
    [SerializeField]
    private ParticleSystem.MinMaxCurve _x;

    [SerializeField]
    private float _xMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _y;

    [SerializeField]
    private float _yMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _z;

    [SerializeField]
    private float _zMultiplier;

    [SerializeField]
    private bool _separateAxes;

    public override void SetModule(ParticleSystem particle)
    {
        var module = particle.rotationOverLifetime;
        module.enabled = true;
        module.x = _x;
        module.xMultiplier = _xMultiplier;
        module.y = _y;
        module.yMultiplier = _yMultiplier;
        module.z = _z;
        module.zMultiplier = _zMultiplier;
        module.separateAxes = _separateAxes;
    }

    public override void RecordModule(ParticleSystem particle)
    {
        var module = particle.rotationOverLifetime;
        _x = module.x;
        _xMultiplier = module.xMultiplier;
        _y = module.y;
        _yMultiplier = module.yMultiplier;
        _z = module.z;
        _zMultiplier = module.zMultiplier;
        _separateAxes = module.separateAxes;
    }
}