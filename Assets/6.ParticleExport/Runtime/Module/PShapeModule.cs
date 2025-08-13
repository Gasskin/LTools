using UnityEngine;

public class PShapeModule : BaseParticleModule
{
    [SerializeField]
    private ParticleSystemShapeType _shapeType;

    [SerializeField]
    private float _randomDirectionAmount;

    [SerializeField]
    private float _sphericalDirectionAmount;

    [SerializeField]
    private float _randomPositionAmount;

    [SerializeField]
    private bool _alignToDirection;

    [SerializeField]
    private float _radius;

    [SerializeField]
    private ParticleSystemShapeMultiModeValue _radiusMode;

    [SerializeField]
    private float _radiusSpread;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _radiusSpeed;

    [SerializeField]
    private float _radiusSpeedMultiplier;

    [SerializeField]
    private float _radiusThickness;

    [SerializeField]
    private float _angle;

    [SerializeField]
    private float _length;

    [SerializeField]
    private Vector3 _boxThickness;

    [SerializeField]
    private ParticleSystemMeshShapeType _meshShapeType;

    [SerializeField]
    private Mesh _mesh;

    [SerializeField]
    private MeshRenderer _meshRenderer;

    [SerializeField]
    private SkinnedMeshRenderer _skinnedMeshRenderer;

    [SerializeField]
    private Sprite _sprite;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private bool _useMeshMaterialIndex;

    [SerializeField]
    private int _meshMaterialIndex;

    [SerializeField]
    private bool _useMeshColors;

    [SerializeField]
    private float _normalOffset;

    [SerializeField]
    private ParticleSystemShapeMultiModeValue _meshSpawnMode;

    [SerializeField]
    private float _meshSpawnSpread;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _meshSpawnSpeed;

    [SerializeField]
    private float _meshSpawnSpeedMultiplier;

    [SerializeField]
    private float _arc;

    [SerializeField]
    private ParticleSystemShapeMultiModeValue _arcMode;

    [SerializeField]
    private float _arcSpread;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _arcSpeed;

    [SerializeField]
    private float _arcSpeedMultiplier;

    [SerializeField]
    private float _donutRadius;

    [SerializeField]
    private Vector3 _position;

    [SerializeField]
    private Vector3 _rotation;

    [SerializeField]
    private Vector3 _scale;

    [SerializeField]
    private Texture2D _texture;

    [SerializeField]
    private ParticleSystemShapeTextureChannel _textureClipChannel;

    [SerializeField]
    private float _textureClipThreshold;

    [SerializeField]
    private bool _textureColorAffectsParticles;

    [SerializeField]
    private bool _textureAlphaAffectsParticles;

    [SerializeField]
    private bool _textureBilinearFiltering;

    [SerializeField]
    private int _textureUVChannel;

    public override void SetModule(ParticleSystem particle)
    {
        var module = particle.shape;
        module.enabled = true;
        module.shapeType = _shapeType;
        module.randomDirectionAmount = _randomDirectionAmount;
        module.sphericalDirectionAmount = _sphericalDirectionAmount;
        module.randomPositionAmount = _randomPositionAmount;
        module.alignToDirection = _alignToDirection;
        module.radius = _radius;
        module.radiusMode = _radiusMode;
        module.radiusSpread = _radiusSpread;
        module.radiusSpeed = _radiusSpeed;
        module.radiusSpeedMultiplier = _radiusSpeedMultiplier;
        module.radiusThickness = _radiusThickness;
        module.angle = _angle;
        module.length = _length;
        module.boxThickness = _boxThickness;
        module.meshShapeType = _meshShapeType;
        module.mesh = _mesh;
        module.meshRenderer = _meshRenderer;
        module.skinnedMeshRenderer = _skinnedMeshRenderer;
        module.sprite = _sprite;
        module.spriteRenderer = _spriteRenderer;
        module.useMeshMaterialIndex = _useMeshMaterialIndex;
        module.meshMaterialIndex = _meshMaterialIndex;
        module.useMeshColors = _useMeshColors;
        module.normalOffset = _normalOffset;
        module.meshSpawnMode = _meshSpawnMode;
        module.meshSpawnSpread = _meshSpawnSpread;
        module.meshSpawnSpeed = _meshSpawnSpeed;
        module.meshSpawnSpeedMultiplier = _meshSpawnSpeedMultiplier;
        module.arc = _arc;
        module.arcMode = _arcMode;
        module.arcSpread = _arcSpread;
        module.arcSpeed = _arcSpeed;
        module.arcSpeedMultiplier = _arcSpeedMultiplier;
        module.donutRadius = _donutRadius;
        module.position = _position;
        module.rotation = _rotation;
        module.scale = _scale;
        module.texture = _texture;
        module.textureClipChannel = _textureClipChannel;
        module.textureClipThreshold = _textureClipThreshold;
        module.textureColorAffectsParticles = _textureColorAffectsParticles;
        module.textureAlphaAffectsParticles = _textureAlphaAffectsParticles;
        module.textureBilinearFiltering = _textureBilinearFiltering;
        module.textureUVChannel = _textureUVChannel;
    }

    public override void RecordModule(ParticleSystem particle)
    {
        var module = particle.shape;
        _shapeType = module.shapeType;
        _randomDirectionAmount = module.randomDirectionAmount;
        _sphericalDirectionAmount = module.sphericalDirectionAmount;
        _randomPositionAmount = module.randomPositionAmount;
        _alignToDirection = module.alignToDirection;
        _radius = module.radius;
        _radiusMode = module.radiusMode;
        _radiusSpread = module.radiusSpread;
        _radiusSpeed = module.radiusSpeed;
        _radiusSpeedMultiplier = module.radiusSpeedMultiplier;
        _radiusThickness = module.radiusThickness;
        _angle = module.angle;
        _length = module.length;
        _boxThickness = module.boxThickness;
        _meshShapeType = module.meshShapeType;
        _mesh = module.mesh;
        _meshRenderer = module.meshRenderer;
        _skinnedMeshRenderer = module.skinnedMeshRenderer;
        _sprite = module.sprite;
        _spriteRenderer = module.spriteRenderer;
        _useMeshMaterialIndex = module.useMeshMaterialIndex;
        _meshMaterialIndex = module.meshMaterialIndex;
        _useMeshColors = module.useMeshColors;
        _normalOffset = module.normalOffset;
        _meshSpawnMode = module.meshSpawnMode;
        _meshSpawnSpread = module.meshSpawnSpread;
        _meshSpawnSpeed = module.meshSpawnSpeed;
        _meshSpawnSpeedMultiplier = module.meshSpawnSpeedMultiplier;
        _arc = module.arc;
        _arcMode = module.arcMode;
        _arcSpread = module.arcSpread;
        _arcSpeed = module.arcSpeed;
        _arcSpeedMultiplier = module.arcSpeedMultiplier;
        _donutRadius = module.donutRadius;
        _position = module.position;
        _rotation = module.rotation;
        _scale = module.scale;
        _texture = module.texture;
        _textureClipChannel = module.textureClipChannel;
        _textureClipThreshold = module.textureClipThreshold;
        _textureColorAffectsParticles = module.textureColorAffectsParticles;
        _textureAlphaAffectsParticles = module.textureAlphaAffectsParticles;
        _textureBilinearFiltering = module.textureBilinearFiltering;
        _textureUVChannel = module.textureUVChannel;
    }
}