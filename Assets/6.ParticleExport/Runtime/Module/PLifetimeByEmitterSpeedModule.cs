using System;
using UnityEngine;

[Serializable]
public class PLifetimeByEmitterSpeedModule : BaseParticleModule
{
    [SerializeField]
    private ParticleSystem.MinMaxCurve _curve;

    [SerializeField]
    private float _curveMultiplier;

    [SerializeField]
    private Vector2 _range;

    public override void SetModule(ParticleSystem particle)
    {
        var module = particle.lifetimeByEmitterSpeed;
        module.enabled = true;
        module.curve = _curve;
        module.curveMultiplier = _curveMultiplier;
        module.range = _range;
    }

    public override void RecordModule(ParticleSystem particle)
    {
        var module = particle.lifetimeByEmitterSpeed;
        _curve = module.curve;
        _curveMultiplier = module.curveMultiplier;
        _range = module.range;
    }
}