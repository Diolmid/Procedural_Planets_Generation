using UnityEngine;

public class ShapeGenerator : MonoBehaviour
{
    private ShapeSetting _setting;
    private NoiseFilter[] _noiseFilters;

    public ShapeGenerator(ShapeSetting setting)
    {
        _setting = setting;

        _noiseFilters = new NoiseFilter[_setting.noiseLayers.Length];
        for (int i = 0; i < _noiseFilters.Length; i++)
            _noiseFilters[i] = new NoiseFilter(_setting.noiseLayers[i].noiseSettings);
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        float firsLayerValue = 0;
        float elevation = 0;

        if(_noiseFilters.Length > 0)
        {
            firsLayerValue = _noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (_setting.noiseLayers[0].enabled) 
            {
                elevation = firsLayerValue;
            }
        }

        for (int i = 1; i < _noiseFilters.Length; i++)
        {
            float mask = (_setting.noiseLayers[i].useFirstLayerAsMask) ? firsLayerValue : 1;
            if (_setting.noiseLayers[i].enabled)
                elevation += _noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
        }

        return pointOnUnitSphere * _setting.planetRadius * (1 + elevation);
    }
}