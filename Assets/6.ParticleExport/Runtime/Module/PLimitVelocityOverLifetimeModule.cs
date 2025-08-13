using System;
using UnityEngine;

[Serializable]
public class PLimitVelocityOverLifetimeModule : BaseParticleModule
{
    [SerializeField]
    private ParticleSystem.MinMaxCurve _limitX;

    [SerializeField]
    private float _limitXMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _limitY;

    [SerializeField]
    private float _limitYMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _limitZ;

    [SerializeField]
    private float _limitZMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _limit;

    [SerializeField]
    private float _limitMultiplier;

    [SerializeField]
    private float _dampen;

    [SerializeField]
    private bool _separateAxes;

    [SerializeField]
    private ParticleSystemSimulationSpace _space;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _drag;

    [SerializeField]
    private float _dragMultiplier;

    [SerializeField]
    private bool _multiplyDragByParticleSize;

    [SerializeField]
    private bool _multiplyDragByParticleVelocity;

    public override void SetModule(ParticleSystem particle)
    {
        var module = particle.limitVelocityOverLifetime;
        module.enabled = true;
        module.limitX = _limitX;
        module.limitXMultiplier = _limitXMultiplier;
        module.limitY = _limitY;
        module.limitYMultiplier = _limitYMultiplier;
        module.limitZ = _limitZ;
        module.limitZMultiplier = _limitZMultiplier;
        module.limit = _limit;
        module.limitMultiplier = _limitMultiplier;
        module.dampen = _dampen;
        module.separateAxes = _separateAxes;
        module.space = _space;
        module.drag = _drag;
        module.dragMultiplier = _dragMultiplier;
        module.multiplyDragByParticleSize = _multiplyDragByParticleSize;
        module.multiplyDragByParticleVelocity = _multiplyDragByParticleVelocity;
    }

    public override void RecordModule(ParticleSystem particle)
    {
        var module = particle.limitVelocityOverLifetime;
        _limitX = module.limitX;
        _limitXMultiplier = module.limitXMultiplier;
        _limitY = module.limitY;
        _limitYMultiplier = module.limitYMultiplier;
        _limitZ = module.limitZ;
        _limitZMultiplier = module.limitZMultiplier;
        _limit = module.limit;
        _limitMultiplier = module.limitMultiplier;
        _dampen = module.dampen;
        _separateAxes = module.separateAxes;
        _space = module.space;
        _drag = module.drag;
        _dragMultiplier = module.dragMultiplier;
        _multiplyDragByParticleSize = module.multiplyDragByParticleSize;
        _multiplyDragByParticleVelocity = module.multiplyDragByParticleVelocity;
    }
}