using System;
using UnityEngine;

public class PColorBySpeedModule : BaseParticleModule
{
    [SerializeField]
    private ParticleSystem.MinMaxGradient _color;

    [SerializeField]
    private Vector2 _range;

    public override void SetModule(ParticleSystem particle)
    {
        var module = particle.colorBySpeed;
        module.enabled = true;
        module.color = _color;
        module.range = _range;
    }

    public override void RecordModule(ParticleSystem particle)
    {
        var module = particle.colorBySpeed;
        _color = module.color;
        _range = module.range;
    }
}