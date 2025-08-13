using System;
using UnityEngine;

[Serializable]
public class PInheritVelocityModule : BaseParticleModule
{
    [SerializeField]
    private ParticleSystemInheritVelocityMode _mode;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _curve;

    [SerializeField]
    private float _curveMultiplier;

    public override void SetModule(ParticleSystem particle)
    {
        var module = particle.inheritVelocity;
        module.enabled = true;
        module.mode = _mode;
        module.curve = _curve;
        module.curveMultiplier = _curveMultiplier;
    }

    public override void RecordModule(ParticleSystem particle)
    {
        var module = particle.inheritVelocity;
        _mode = module.mode;
        _curve = module.curve;
        _curveMultiplier = module.curveMultiplier;
    }
}