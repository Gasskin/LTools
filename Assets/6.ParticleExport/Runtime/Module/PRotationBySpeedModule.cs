using System;
using UnityEngine;

[Serializable]
public class PRotationBySpeedModule : BaseParticleModule
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

    [SerializeField]
    private Vector2 _range;

    public override void SetModule(ParticleSystem particle)
    {
        var module = particle.rotationBySpeed;
        module.enabled = true;
        module.x = _x;
        module.xMultiplier = _xMultiplier;
        module.y = _y;
        module.yMultiplier = _yMultiplier;
        module.z = _z;
        module.zMultiplier = _zMultiplier;
        module.separateAxes = _separateAxes;
        module.range = _range;
    }

    public override void RecordModule(ParticleSystem particle)
    {
        var module = particle.rotationBySpeed;
        _x = module.x;
        _xMultiplier = module.xMultiplier;
        _y = module.y;
        _yMultiplier = module.yMultiplier;
        _z = module.z;
        _zMultiplier = module.zMultiplier;
        _separateAxes = module.separateAxes;
        _range = module.range;
    }
}