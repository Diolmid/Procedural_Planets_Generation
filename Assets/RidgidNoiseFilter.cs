using UnityEngine;

public class RidgidNoiseFilter : INoiseFilter
{
    private Noise _noise = new Noise();
    private NoiseSettings.RidgidNoiseSettings _noiseSettings;

    public RidgidNoiseFilter(NoiseSettings.RidgidNoiseSettings noiseSettings)
    {
        _noiseSettings = noiseSettings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = _noiseSettings.baseRoughness;
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < _noiseSettings.layersCount; i++)
        {
            float v = 1 - Mathf.Abs(_noise.Evaluate(point * frequency + _noiseSettings.center));
            v *= v;
            v *= weight;
            weight = Mathf.Clamp01(v * _noiseSettings.weightMultiplier);

            noiseValue += v * amplitude;
            frequency *= _noiseSettings.roughness;
            amplitude *= _noiseSettings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - _noiseSettings.minValue);
        return noiseValue * _noiseSettings.strength;
    }
}