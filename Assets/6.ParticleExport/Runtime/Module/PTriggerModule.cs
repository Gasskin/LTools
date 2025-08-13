using System;
using UnityEngine;

[Serializable]
public class PTriggerModule : BaseParticleModule
{
    [SerializeField]
    private ParticleSystemOverlapAction _inside;

    [SerializeField]
    private ParticleSystemOverlapAction _outside;

    [SerializeField]
    private ParticleSystemOverlapAction _enter;

    [SerializeField]
    private ParticleSystemOverlapAction _exit;

    [SerializeField]
    private ParticleSystemColliderQueryMode _colliderQueryMode;

    [SerializeField]
    private float _radiusScale;

    public override void SetModule(ParticleSystem particle)
    {
        var module = particle.trigger;
        module.enabled = true;
        module.inside = _inside;
        module.outside = _outside;
        module.enter = _enter;
        module.exit = _exit;
        module.colliderQueryMode = _colliderQueryMode;
        module.radiusScale = _radiusScale;
    }

    public override void RecordModule(ParticleSystem particle)
    {
        var module = particle.trigger;
        _inside = module.inside;
        _outside = module.outside;
        _enter = module.enter;
        _exit = module.exit;
        _colliderQueryMode = module.colliderQueryMode;
        _radiusScale = module.radiusScale;
    }
}