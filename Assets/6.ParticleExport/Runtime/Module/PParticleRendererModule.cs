using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class PParticleRendererModule : BaseParticleModule
{
    [SerializeField]
    private ParticleSystemRenderMode _renderMode;

    [SerializeField]
    private float _lengthScale;

    [SerializeField]
    private float _cameraVelocityScale;

    [SerializeField]
    private float _velocityScale;

    [SerializeField]
    private bool _freeformStretching;

    [SerializeField]
    private bool _rotateWithStretchDirection;

    [SerializeField]
    private float _normalDirection;

    [SerializeField]
    private Material _material;

    [SerializeField]
    private Material _traiMaterial;

    [SerializeField]
    private ParticleSystemSortMode _sortMode;

    [SerializeField]
    private float _sortingFudge;

    [SerializeField]
    private float _minParticleSize;

    [SerializeField]
    private float _maxParticleSize;

    [SerializeField]
    private ParticleSystemRenderSpace _alignment;

    [SerializeField]
    private Vector3 _flip;

    [SerializeField]
    private bool _allowRoll;

    [SerializeField]
    private Vector3 _pivot;

    [SerializeField]
    private bool _visiblePivot;

    [SerializeField]
    private List<ParticleSystemVertexStream> _particleSystemVertexStreams;

    [SerializeField]
    private SpriteMaskInteraction _maskInteraction;

    [SerializeField]
    private float _shadowBias;

    [SerializeField]
    private MotionVectorGenerationMode _motionVectorGenerationMode;

    [SerializeField]
    private bool _applyActiveColorSpace;

    [SerializeField]
    private bool _customVertexStreams;

    [SerializeField]
    private int _sortLayerId;

    [SerializeField]
    private int _orderInLayer;

    [SerializeField]
    private LightProbeUsage _lightProbeUsage;

    [SerializeField]
    private uint _renderingLayerMask;

    [SerializeField]
    private Mesh _mesh;

    // [SerializeField]
    // private ParticleSystemRenderer.ScaleMode _scaleMode;

    [SerializeField]
    private int _activeVertexStreamsCount;

    public override void SetModule(ParticleSystem particle)
    {
        var render = particle.GetComponent<ParticleSystemRenderer>();
        render.enabled = true;
        render.renderMode = _renderMode;
        render.lengthScale = _lengthScale;
        render.cameraVelocityScale = _cameraVelocityScale;
        render.velocityScale = _velocityScale;
        render.freeformStretching = _freeformStretching;
        render.rotateWithStretchDirection = _rotateWithStretchDirection;
        render.normalDirection = _normalDirection;
        render.sharedMaterial = _material;
        render.trailMaterial = _traiMaterial;
        render.sortMode = _sortMode;
        render.sortingFudge = _sortingFudge;
        render.minParticleSize = _minParticleSize;
        render.maxParticleSize = _maxParticleSize;
        render.alignment = _alignment;
        render.flip = _flip;
        render.allowRoll = _allowRoll;
        render.pivot = _pivot;
        render.maskInteraction = _maskInteraction;
        if (_activeVertexStreamsCount > 0)
            render.SetActiveVertexStreams(_particleSystemVertexStreams);
        render.shadowBias = _shadowBias;
        render.motionVectorGenerationMode = _motionVectorGenerationMode;
        // render.sortingLayerID = _sortLayerId;
        // render.sortingOrder = _orderInLayer;
        render.lightProbeUsage = _lightProbeUsage;
        render.renderingLayerMask = _renderingLayerMask;
        render.mesh = _mesh;

        SetRenderLayer(particle);
    }

    public override void RecordModule(ParticleSystem particle)
    {
        var render = particle.GetComponent<ParticleSystemRenderer>();
        _renderMode = render.renderMode;

        _renderMode = render.renderMode;
        _lengthScale = render.lengthScale;
        _cameraVelocityScale = render.cameraVelocityScale;
        _velocityScale = render.velocityScale;
        _freeformStretching = render.freeformStretching;
        _rotateWithStretchDirection = render.rotateWithStretchDirection;

        _normalDirection = render.normalDirection;
        _material = render.sharedMaterial;
        _traiMaterial = render.trailMaterial;
        _sortMode = render.sortMode;
        _sortingFudge = render.sortingFudge;
        _minParticleSize = render.minParticleSize;
        _maxParticleSize = render.maxParticleSize;
        _alignment = render.alignment;
        _flip = render.flip;
        _allowRoll = render.allowRoll;
        _pivot = render.pivot;
        _maskInteraction = render.maskInteraction;
        _activeVertexStreamsCount = render.activeVertexStreamsCount;
        _particleSystemVertexStreams = new List<ParticleSystemVertexStream>()
        {
            ParticleSystemVertexStream.Position,
            ParticleSystemVertexStream.Normal,
            ParticleSystemVertexStream.Color,
            ParticleSystemVertexStream.UV,
            ParticleSystemVertexStream.UV2,
            ParticleSystemVertexStream.Custom1XYZW,
            ParticleSystemVertexStream.Custom2XYZW,
        };

        render.GetActiveVertexStreams(_particleSystemVertexStreams);
        _shadowBias = render.shadowBias;
        _motionVectorGenerationMode = render.motionVectorGenerationMode;
        _sortLayerId = render.sortingLayerID;
        _orderInLayer = render.sortingOrder;
        _lightProbeUsage = render.lightProbeUsage;
        _renderingLayerMask = render.renderingLayerMask;
        _mesh = render.mesh;
    }

    private void SetRenderLayer(ParticleSystem particle)
    {
        var parent = particle.transform.parent;
        Canvas canvas = null;
        while (parent != null)
        {
            if (parent.TryGetComponent(out canvas))
                break;
            parent = parent.parent;
        }

        var render = particle.GetComponent<ParticleSystemRenderer>();
        if (canvas == null)
        {
            render.sortingLayerID = _sortLayerId;
            render.sortingOrder = _orderInLayer;
            return;
        }

        // render.sortingLayerName = canvas.sortingLayerName;
        render.sortingLayerID = canvas.sortingLayerID;
        render.sortingOrder = _orderInLayer + canvas.sortingOrder;
    }
}