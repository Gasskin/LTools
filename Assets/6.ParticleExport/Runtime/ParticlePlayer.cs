using System;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    private BaseParticleModule[] _modules;

    private void Awake()
    {
        _modules = gameObject.GetComponents<BaseParticleModule>();
    }

    public void Play()
    {
        
    }

    private void InternalPlay(ParticleSystem p)
    {
        p.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
        p.transform.localPosition = Vector3.zero;
        p.transform.localRotation = Quaternion.identity;
        p.transform.localScale = Vector3.one;
        SetParticleSystem(p);
        p.Play();
    }

    private void SetParticleSystem(ParticleSystem p)
    {
        foreach (var module in _modules)
        {
            module.SetModule(p);
        }
    }
}