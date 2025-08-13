using UnityEngine;

public abstract class BaseParticleModule : MonoBehaviour
{
    public abstract void SetModule(ParticleSystem particle);
    public abstract void RecordModule(ParticleSystem particle);
}