using System;
using UnityEngine;

[Serializable]
public class PExternalForcesModule : BaseParticleModule
{
    [SerializeField]
    private float _multiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _multiplierCurve;

    [SerializeField]
    private ParticleSystemGameObjectFilter _influenceFilter;

    [SerializeField]
    private LayerMask _influenceMask;

    public override void SetModule(ParticleSystem particle)
    {
        var module = particle.externalForces;
        module.enabled = true;
        module.multiplier = _multiplier;
        module.multiplierCurve = _multiplierCurve;
        module.influenceFilter = _influenceFilter;
        module.influenceMask = _influenceMask;
    }

    public override void RecordModule(ParticleSystem particle)
    {
        var module = particle.externalForces;
        _multiplier = module.multiplier;
        _multiplierCurve = module.multiplierCurve;
        _influenceFilter = module.influenceFilter;
        _influenceMask = module.influenceMask;
    }
}