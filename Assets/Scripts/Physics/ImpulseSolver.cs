using UnityEngine;
using System.Collections.Generic;

public class ImpulseSolver
{
    private class ImpulseData
    {
        public Vector3 InitialVelocity;
        public Vector3 Acceleration;
        public float Timer;
        public float Duration;

        public Vector3 CurrentVelocity
        {
            get
            {
                return InitialVelocity + Acceleration * Timer;
            }
        }
    };

    private List<ImpulseData> impulses = new List<ImpulseData>();
    private List<int> removalIndices = new List<int>();

    public void AddImpulseAndDeceleration(Vector3 initialVelocity, float decelerationMagnitude, bool rebound = false)
    {
        float magnitude = initialVelocity.magnitude;
        if (magnitude <= 0.01f)
        {
            return;
        }

        Vector3 acceleration = -initialVelocity / magnitude * decelerationMagnitude;
        float duration = magnitude / decelerationMagnitude * (rebound ? 2.0f : 1.0f);
        AddImpulse(initialVelocity, acceleration, duration);
    }

    public void AddImpulseForDuration(Vector3 initialVelocity, float duration, bool rebound = false)
    {
        float magnitude = initialVelocity.magnitude;
        if (magnitude <= 0.01f)
        {
            return;
        }

        Vector3 acceleration = -initialVelocity / duration * (rebound ? 2.0f : 1.0f);
        AddImpulse(initialVelocity, acceleration, duration);
    }

    public void Clear()
    {
        impulses.Clear();
    }

    public Vector3 TotalVelocity
    {
        get
        {
            Vector3 impulseVector = Vector3.zero;
            for (int i = 0; i < impulses.Count; ++i)
            {
                impulseVector += impulses[i].CurrentVelocity;
            }
            return impulseVector;
        }
    }

    public void Update(float deltaTime)
    {
        removalIndices.Clear();
        for (int i = 0; i < impulses.Count; ++i)
        {
            impulses[i].Timer = Mathf.Min(impulses[i].Duration, impulses[i].Timer + deltaTime);
            if (impulses[i].Timer >= impulses[i].Duration)
            {
                removalIndices.Add(i);
            }
        }

        for (int i = removalIndices.Count - 1; i >= 0; --i)
        {
            impulses.RemoveAt(removalIndices[i]);
        }
    }

    private void AddImpulse(Vector3 velocity, Vector3 acceleration, float duration)
    {
        impulses.Add(new ImpulseData()
        {
            InitialVelocity = velocity,
            Acceleration = acceleration,
            Timer = 0.0f,
            Duration = duration
        });
    }
}
