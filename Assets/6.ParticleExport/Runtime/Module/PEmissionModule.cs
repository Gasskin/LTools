using System;
using UnityEngine;

public class PEmissionModule : BaseParticleModule
{
    [Serializable]
    public struct Burst
    {
        public ParticleSystem.MinMaxCurve Count;
        public float Time;
        public int Cycles;
        public float Interval;
        public float Probability;
    }

    [SerializeField]
    private ParticleSystem.MinMaxCurve _rateOverTime;

    [SerializeField]
    private float _rateOverTimeMultiplier;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _rateOverDistance;

    [SerializeField]
    private float _rateOverDistanceMultiplier;

    [SerializeField]
    private int _burstCount;

    [SerializeField]
    private Burst[] _bursts;

    public override void SetModule(ParticleSystem particle)
    {
        var module = particle.emission;
        module.enabled = true;
        module.rateOverTime = _rateOverTime;
        module.rateOverTimeMultiplier = _rateOverTimeMultiplier;
        module.rateOverDistance = _rateOverDistance;
        module.rateOverDistanceMultiplier = _rateOverDistanceMultiplier;
        module.burstCount = _burstCount;
        if (_burstCount > 0) 
        {
            for (int i = 0; i < _burstCount; i++)
            {
                var b = _bursts[i];
                var newBurst = new ParticleSystem.Burst()
                {
                    count = b.Count,
                    time = b.Time,
                    cycleCount = b.Cycles,
                    repeatInterval = b.Interval,
                    probability = b.Probability,
                };
                module.SetBurst(i, newBurst);
            }
        }
    }

    public override void RecordModule(ParticleSystem particle)
    {
        var module = particle.emission;
        _rateOverTime = module.rateOverTime;
        _rateOverTimeMultiplier = module.rateOverTimeMultiplier;
        _rateOverDistance = module.rateOverDistance;
        _rateOverDistanceMultiplier = module.rateOverDistanceMultiplier;
        _burstCount = module.burstCount;
        if (module.burstCount > 0)
        {
            _bursts = new Burst[_burstCount];
            for (int i = 0; i < _burstCount; i++)
            {
                var b = module.GetBurst(i);
                var newBurst = new Burst()
                {
                    Count = b.count,
                    Time = b.time,
                    Cycles = b.cycleCount,
                    Interval = b.repeatInterval,
                    Probability = b.probability
                };
                _bursts[i] = newBurst;
            }
        }
    }
}