using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{ 
    private Noise _noise = new Noise();
    private NoiseSettings.SimpleNoiseSettings _noiseSettings;

    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings noiseSettings)
    {
        _noiseSettings = noiseSettings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = _noiseSettings.baseRoughness;
        float amplitude = 1;

        for (int i = 0; i < _noiseSettings.layersCount; i++)
        {
            float v = _noise.Evaluate(point * frequency + _noiseSettings.center);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= _noiseSettings.roughness;
            amplitude *= _noiseSettings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - _noiseSettings.minValue);
        return noiseValue * _noiseSettings.strength;
    }
}