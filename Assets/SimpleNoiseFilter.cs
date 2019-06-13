using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    NoiseSettings.SimpleNoiseSettings settings;
    Noise noise = new Noise();

    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
    {
        this.settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        //float noiseValue = (noise.Evaluate(point * settings.roughness + settings.center) + 1) * 0.5f;//change the range from -1 to 1 into 0 to 1
        float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;

        for (int i = 0; i < settings.numLayers; i++)
        {
            float v = noise.Evaluate(point * frequency + settings.center);
            noiseValue += (v + 1) * 0.5f * amplitude;//change the range from -1 to 1 into 0 to 1
            frequency *= settings.roughness;//frequency will increase with each layer for more detail
            amplitude *= settings.persistence;//but the amplitude will decrease with each layer for more subtle changes
        }

        noiseValue = Mathf.Max(0, noiseValue - settings.minValue);//this makes terrain only show above a certain point, the rest is a circle
        return noiseValue * settings.strength;
    }
}
