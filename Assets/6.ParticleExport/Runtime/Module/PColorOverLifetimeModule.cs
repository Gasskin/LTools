using System;
using UnityEngine;

[Serializable]
public class PColorOverLifetimeModule : BaseParticleModule
{
    [SerializeField]
    private ParticleSystem.MinMaxGradient _color;

    public override void SetModule(ParticleSystem particle)
    {
        var module = particle.colorOverLifetime;
        module.enabled = true;
        module.color = _color;
    }

    public override void RecordModule(ParticleSystem particle)
    {
        var module = particle.colorOverLifetime;
        _color = module.color;
    }
}