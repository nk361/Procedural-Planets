using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFilter
{
    private NoiseSettings settings;
    Noise noise = new Noise();

    public NoiseFilter(NoiseSettings settings)
    {
        this.settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = (noise.Evaluate(point * settings.roughness + settings.center) + 1) * 0.5f;//change the range from -1 to 1 into 0 to 1
        return noiseValue * settings.strength;
    }
}
