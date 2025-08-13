using System;
using UnityEngine;
using UnityEngine.Rendering;

public class PTextureSheetAnimationModule : BaseParticleModule
{
    [SerializeField]
    private ParticleSystemAnimationMode _mode;

    [SerializeField]
    private ParticleSystemAnimationTimeMode _timeMode;

    [SerializeField]
    private float _fps;

    [SerializeField]
    private int _numTilesX;

    [SerializeField]
    private int _numTilesY;

    [SerializeField]
    private ParticleSystemAnimationType _animation;

    [SerializeField]
    private ParticleSystemAnimationRowMode _rowMode;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _frameOverTime;

    [SerializeField]
    private float _frameOverTimeMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _startFrame;

    [SerializeField]
    private float _startFrameMultiplier;

    [SerializeField]
    private int _cycleCount;

    [SerializeField]
    private int _rowIndex;

    [SerializeField]
    private UVChannelFlags _uvChannelMask;

    [SerializeField]
    private Vector2 _speedRange;

    public override void SetModule(ParticleSystem particle)
    {
        var module = particle.textureSheetAnimation;
        module.enabled = true;
        module.mode = _mode;
        module.timeMode = _timeMode;
        module.fps = _fps;
        module.numTilesX = _numTilesX;
        module.numTilesY = _numTilesY;
        module.animation = _animation;
        module.rowMode = _rowMode;
        module.frameOverTime = _frameOverTime;
        module.frameOverTimeMultiplier = _frameOverTimeMultiplier;
        module.startFrame = _startFrame;
        module.startFrameMultiplier = _startFrameMultiplier;
        module.cycleCount = _cycleCount;
        module.rowIndex = _rowIndex;
        module.uvChannelMask = _uvChannelMask;
        module.speedRange = _speedRange;
    }

    public override void RecordModule(ParticleSystem particle)
    {
        var module = particle.textureSheetAnimation;
        _mode = module.mode;
        _timeMode = module.timeMode;
        _fps = module.fps;
        _numTilesX = module.numTilesX;
        _numTilesY = module.numTilesY;
        _animation = module.animation;
        _rowMode = module.rowMode;
        _frameOverTime = module.frameOverTime;
        _frameOverTimeMultiplier = module.frameOverTimeMultiplier;
        _startFrame = module.startFrame;
        _startFrameMultiplier = module.startFrameMultiplier;
        _cycleCount = module.cycleCount;
        _rowIndex = module.rowIndex;
        _uvChannelMask = module.uvChannelMask;
        _speedRange = module.speedRange;
    }
}