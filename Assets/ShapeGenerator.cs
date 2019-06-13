﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    ShapeSettings settings;
    INoiseFilter[] noiseFilters;

    public ShapeGenerator(ShapeSettings settings)
    {
        this.settings = settings;
        noiseFilters = new INoiseFilter[settings.noiseLayers.Length];

        for (int i = 0; i < noiseFilters.Length; i++)
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        //LINQ expression
        //float elevation = noiseFilters.Where((t, i) => settings.noiseLayers[i].enabled).Sum(t => t.Evaluate(pointOnUnitSphere));

        float firstLayerValue = 0;
        float elevation = 0;

        if (noiseFilters.Length > 0)
        {
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (settings.noiseLayers[0].enabled)
                elevation = firstLayerValue;
        }

        for (int i = 1; i < noiseFilters.Length; i++) //skip first layer
        {
            if (settings.noiseLayers[i].enabled)
            {
                float mask = settings.noiseLayers[i].useFirstLayerAsMask ? firstLayerValue : 1;
                elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }

        return pointOnUnitSphere * settings.planetRadius * (1 + elevation);
    }
}
