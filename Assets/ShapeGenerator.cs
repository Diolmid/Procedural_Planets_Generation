using UnityEngine;

public class ShapeGenerator : MonoBehaviour
{
    public MinMax elevationMinMax;

    private ShapeSetting _settings;
    private INoiseFilter[] _noiseFilters;

    public void UpdateSettings(ShapeSetting settings)
    {
        _settings = settings;

        _noiseFilters = new INoiseFilter[_settings.noiseLayers.Length];
        for (int i = 0; i < _noiseFilters.Length; i++)
            _noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(_settings.noiseLayers[i].noiseSettings);
        elevationMinMax = new MinMax();
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        float firsLayerValue = 0;
        float elevation = 0;

        if(_noiseFilters.Length > 0)
        {
            firsLayerValue = _noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (_settings.noiseLayers[0].enabled) 
            {
                elevation = firsLayerValue;
            }
        }

        for (int i = 1; i < _noiseFilters.Length; i++)
        {
            float mask = (_settings.noiseLayers[i].useFirstLayerAsMask) ? firsLayerValue : 1;
            if (_settings.noiseLayers[i].enabled)
                elevation += _noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
        }
        elevation = _settings.planetRadius * (1 + elevation);
        elevationMinMax.AddValue(elevation);
        return pointOnUnitSphere * elevation;
    }
}