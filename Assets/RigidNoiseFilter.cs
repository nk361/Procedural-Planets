using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter : INoiseFilter
{
    //The math for mountainous terrain
    //(1 - |sin(x)|)^2
    //sin for the wave shape, absolute value for pointed wave shape, subtracted from one to invert it, and raised to a power for steepness

    NoiseSettings.RigidNoiseSettings settings;
    Noise noise = new Noise();

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings settings)
    {
        this.settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < settings.numLayers; i++)
        {
            float v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + settings.center));
            v *= v;
            v *= weight;
            weight = Mathf.Clamp01(v * settings.weightMultiplier);//layers that are higher up will have more detail

            noiseValue += v * amplitude;
            frequency *= settings.roughness;//frequency will increase with each layer for more detail
            amplitude *= settings.persistence;//but the amplitude will decrease with each layer for more subtle changes
        }
        
        noiseValue = Mathf.Max(0, noiseValue - settings.minValue);//this makes terrain only show above a certain point, the rest is a circle
        return noiseValue * settings.strength;
    }
}
