using UnityEngine;

public class PMainModule : BaseParticleModule
{
    [SerializeField]
    private float _duration;

    [SerializeField]
    private bool _loop;

    [SerializeField]
    private bool _prewarm;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _startDelay;

    [SerializeField]
    private float _startDelayMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _startLifetime;

    [SerializeField]
    private float _startLifetimeMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _startSpeed;

    [SerializeField]
    private float _startSpeedMultiplier;

    [SerializeField]
    private bool _startSize3D;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _startSize;

    [SerializeField]
    private float _startSizeMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _startSizeX;

    [SerializeField]
    private float _startSizeXMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _startSizeY;

    [SerializeField]
    private float _startSizeYMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _startSizeZ;

    [SerializeField]
    private float _startSizeZMultiplier;

    [SerializeField]
    private bool _startRotation3D;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _startRotation;

    [SerializeField]
    private float _startRotationMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _startRotationX;

    [SerializeField]
    private float _startRotationXMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _startRotationY;

    [SerializeField]
    private float _startRotationYMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _startRotationZ;

    [SerializeField]
    private float _startRotationZMultiplier;

    [SerializeField]
    private float _flipRotation;

    [SerializeField]
    private ParticleSystem.MinMaxGradient _startColor;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _gravityModifier;

    [SerializeField]
    private float _gravityModifierMultiplier;

    [SerializeField]
    private ParticleSystemSimulationSpace _simulationSpace;

    [SerializeField]
    private Transform _customSimulationSpace;

    [SerializeField]
    private float _simulationSpeed;

    [SerializeField]
    private bool _useUnscaledTime;

    [SerializeField]
    private ParticleSystemScalingMode _scalingMode;

    [SerializeField]
    private bool _playOnAwake;

    [SerializeField]
    private int _maxParticles;

    [SerializeField]
    private ParticleSystemEmitterVelocityMode _emitterVelocityMode;

    [SerializeField]
    private ParticleSystemStopAction _stopAction;

    [SerializeField]
    private ParticleSystemRingBufferMode _ringBufferMode;

    [SerializeField]
    private Vector2 _ringBufferLoopRange;

    [SerializeField]
    private ParticleSystemCullingMode _cullingMode;

    public override void SetModule(ParticleSystem particle)
    {
        var module = particle.main;
        module.duration = _duration;
        module.loop = _loop;
        module.prewarm = _prewarm;
        module.startDelay = _startDelay;
        module.startDelayMultiplier = _startDelayMultiplier;
        module.startLifetime = _startLifetime;
        module.startLifetimeMultiplier = _startLifetimeMultiplier;
        module.startSpeed = _startSpeed;
        module.startSpeedMultiplier = _startSpeedMultiplier;
        module.startSize3D = _startSize3D;
        module.startSize = _startSize;
        module.startSizeMultiplier = _startSizeMultiplier;
        module.startSizeX = _startSizeX;
        module.startSizeXMultiplier = _startSizeXMultiplier;
        module.startSizeY = _startSizeY;
        module.startSizeYMultiplier = _startSizeYMultiplier;
        module.startSizeZ = _startSizeZ;
        module.startSizeZMultiplier = _startSizeZMultiplier;
        module.startRotation3D = _startRotation3D;
        module.startRotation = _startRotation;
        module.startRotationMultiplier = _startRotationMultiplier;
        module.startRotationX = _startRotationX;
        module.startRotationXMultiplier = _startRotationXMultiplier;
        module.startRotationY = _startRotationY;
        module.startRotationYMultiplier = _startRotationYMultiplier;
        module.startRotationZ = _startRotationZ;
        module.startRotationZMultiplier = _startRotationZMultiplier;
        module.flipRotation = _flipRotation;
        module.startColor = _startColor;
        module.gravityModifier = _gravityModifier;
        module.gravityModifierMultiplier = _gravityModifierMultiplier;
        module.simulationSpace = _simulationSpace;
        module.customSimulationSpace = _customSimulationSpace;
        module.simulationSpeed = _simulationSpeed;
        module.useUnscaledTime = _useUnscaledTime;
        module.scalingMode = _scalingMode;
        module.playOnAwake = _playOnAwake;
        module.maxParticles = _maxParticles;
        module.emitterVelocityMode = _emitterVelocityMode;
        module.stopAction = _stopAction;
        module.ringBufferMode = _ringBufferMode;
        module.ringBufferLoopRange = _ringBufferLoopRange;
        module.cullingMode = _cullingMode;
    }

    public override void RecordModule(ParticleSystem particle)
    {
        var module = particle.main;
        _duration = module.duration;
        _loop = module.loop;
        _prewarm = module.prewarm;
        _startDelay = module.startDelay;
        _startDelayMultiplier = module.startDelayMultiplier;
        _startLifetime = module.startLifetime;
        _startLifetimeMultiplier = module.startLifetimeMultiplier;
        _startSpeed = module.startSpeed;
        _startSpeedMultiplier = module.startSpeedMultiplier;
        _startSize3D = module.startSize3D;
        _startSize = module.startSize;
        _startSizeMultiplier = module.startSizeMultiplier;
        _startSizeX = module.startSizeX;
        _startSizeXMultiplier = module.startSizeXMultiplier;
        _startSizeY = module.startSizeY;
        _startSizeYMultiplier = module.startSizeYMultiplier;
        _startSizeZ = module.startSizeZ;
        _startSizeZMultiplier = module.startSizeZMultiplier;
        _startRotation3D = module.startRotation3D;
        _startRotation = module.startRotation;
        _startRotationMultiplier = module.startRotationMultiplier;
        _startRotationX = module.startRotationX;
        _startRotationXMultiplier = module.startRotationXMultiplier;
        _startRotationY = module.startRotationY;
        _startRotationYMultiplier = module.startRotationYMultiplier;
        _startRotationZ = module.startRotationZ;
        _startRotationZMultiplier = module.startRotationZMultiplier;
        _flipRotation = module.flipRotation;
        _startColor = module.startColor;
        _gravityModifier = module.gravityModifier;
        _gravityModifierMultiplier = module.gravityModifierMultiplier;
        _simulationSpace = module.simulationSpace;
        _customSimulationSpace = module.customSimulationSpace;
        _simulationSpeed = module.simulationSpeed;
        _useUnscaledTime = module.useUnscaledTime;
        _scalingMode = module.scalingMode;
        _playOnAwake = module.playOnAwake;
        _maxParticles = module.maxParticles;
        _emitterVelocityMode = module.emitterVelocityMode;
        _stopAction = module.stopAction;
        _ringBufferMode = module.ringBufferMode;
        _ringBufferLoopRange = module.ringBufferLoopRange;
        _cullingMode = module.cullingMode;
    }
}