using UnityEngine;

public class ColourGenerator
{
    private const int _textureResolution = 50;
    private Texture2D _texture;
    private ColourSetting _colourSettings;
    private INoiseFilter _biomeNoiseFilter;

    public void UpdateSettings(ColourSetting colourSettings)
    {
        _colourSettings = colourSettings;
        if(_texture == null || _texture.height != _colourSettings.biomeColourSettings.biomes.Length)
            _texture = new Texture2D(_textureResolution, _colourSettings.biomeColourSettings.biomes.Length);
        
        _biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(_colourSettings.biomeColourSettings.noiseSettings);
    }

    public void UpdateElevation(MinMax elevationMinMax)
    {
        _colourSettings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
    {
        float heightPercent = (pointOnUnitSphere.y + 1) / 2f;
        heightPercent += (_biomeNoiseFilter.Evaluate(pointOnUnitSphere) - _colourSettings.biomeColourSettings.noiseOffset) * _colourSettings.biomeColourSettings.noiseStrength;
        float biomeIndex = 0;
        int numBiomes = _colourSettings.biomeColourSettings.biomes.Length;
        float blendRange = _colourSettings.biomeColourSettings.blendAmount / 2f + 0.001f;

        for (int i = 0; i < numBiomes; i++)
        {
            float distance = heightPercent - _colourSettings.biomeColourSettings.biomes[i].startHeight;
            float weight = Mathf.InverseLerp(-blendRange, blendRange, distance);
            biomeIndex *= (1 - weight);
            biomeIndex += i * weight;
        }

        return biomeIndex / Mathf.Max(1, (numBiomes - 1));
    }

    public void UpdateColours()
    {
        var colours = new Color[_texture.width * _texture.height];

        int colourIndex = 0;
        foreach(var biome in _colourSettings.biomeColourSettings.biomes)
        {
            for (int i = 0; i < _textureResolution; i++)
            {
                var gradientColour = biome.gradient.Evaluate(i / (_textureResolution - 1f));
                var tintColour = biome.tint;
                colours[colourIndex] = gradientColour * (1 - biome.tintPercent) + tintColour * biome.tintPercent;
                colourIndex++;
            }
        }

        _texture.SetPixels(colours);
        _texture.Apply();
        _colourSettings.planetMaterial.SetTexture("_texture", _texture);
    }
}