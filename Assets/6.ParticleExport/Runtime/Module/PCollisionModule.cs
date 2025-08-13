using System;
using UnityEngine;

public class PCollisionModule : BaseParticleModule
{
    [SerializeField]
    private ParticleSystemCollisionType _type;

    [SerializeField]
    private ParticleSystemCollisionMode _mode;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _dampen;

    [SerializeField]
    private float _dampenMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _bounce;

    [SerializeField]
    private float _bounceMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _lifetimeLoss;

    [SerializeField]
    private float _lifetimeLossMultiplier;

    [SerializeField]
    private float _minKillSpeed;

    [SerializeField]
    private float _maxKillSpeed;

    [SerializeField]
    private LayerMask _collidesWith;

    [SerializeField]
    private bool _enableDynamicColliders;

    [SerializeField]
    private int _maxCollisionShapes;

    [SerializeField]
    private ParticleSystemCollisionQuality _quality;

    [SerializeField]
    private float _voxelSize;

    [SerializeField]
    private float _radiusScale;

    [SerializeField]
    private bool _sendCollisionMessages;

    [SerializeField]
    private float _colliderForce;

    [SerializeField]
    private bool _multiplyColliderForceByCollisionAngle;

    [SerializeField]
    private bool _multiplyColliderForceByParticleSpeed;

    [SerializeField]
    private bool _multiplyColliderForceByParticleSize;

    public override void SetModule(ParticleSystem particle)
    {
        var module = particle.collision;
        module.enabled = true;
        module.type = _type;
        module.mode = _mode;
        module.dampen = _dampen;
        module.dampenMultiplier = _dampenMultiplier;
        module.bounce = _bounce;
        module.bounceMultiplier = _bounceMultiplier;
        module.lifetimeLoss = _lifetimeLoss;
        module.lifetimeLossMultiplier = _lifetimeLossMultiplier;
        module.minKillSpeed = _minKillSpeed;
        module.maxKillSpeed = _maxKillSpeed;
        module.collidesWith = _collidesWith;
        module.enableDynamicColliders = _enableDynamicColliders;
        module.maxCollisionShapes = _maxCollisionShapes;
        module.quality = _quality;
        module.voxelSize = _voxelSize;
        module.radiusScale = _radiusScale;
        module.sendCollisionMessages = _sendCollisionMessages;
        module.colliderForce = _colliderForce;
        module.multiplyColliderForceByCollisionAngle = _multiplyColliderForceByCollisionAngle;
        module.multiplyColliderForceByParticleSpeed = _multiplyColliderForceByParticleSpeed;
        module.multiplyColliderForceByParticleSize = _multiplyColliderForceByParticleSize;
    }

    public override void RecordModule(ParticleSystem particle)
    {
        var module = particle.collision;
        _type = module.type;
        _mode = module.mode;
        _dampen = module.dampen;
        _dampenMultiplier = module.dampenMultiplier;
        _bounce = module.bounce;
        _bounceMultiplier = module.bounceMultiplier;
        _lifetimeLoss = module.lifetimeLoss;
        _lifetimeLossMultiplier = module.lifetimeLossMultiplier;
        _minKillSpeed = module.minKillSpeed;
        _maxKillSpeed = module.maxKillSpeed;
        _collidesWith = module.collidesWith;
        _enableDynamicColliders = module.enableDynamicColliders;
        _maxCollisionShapes = module.maxCollisionShapes;
        _quality = module.quality;
        _voxelSize = module.voxelSize;
        _radiusScale = module.radiusScale;
        _sendCollisionMessages = module.sendCollisionMessages;
        _colliderForce = module.colliderForce;
        _multiplyColliderForceByCollisionAngle = module.multiplyColliderForceByCollisionAngle;
        _multiplyColliderForceByParticleSpeed = module.multiplyColliderForceByParticleSpeed;
        _multiplyColliderForceByParticleSize = module.multiplyColliderForceByParticleSize;
    }
}