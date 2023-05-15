using UnityEngine;

public class ColourGenerator
{
    private const int _textureResolution = 50;
    private Texture2D _texture;
    private ColourSetting _colourSettings;

    public void UpdateSettings(ColourSetting colourSettings)
    {
        _colourSettings = colourSettings;
        if(_texture == null)
            _texture = new Texture2D(_textureResolution, 1);
    }

    public void UpdateElevation(MinMax elevationMinMax)
    {
        _colourSettings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public void UpdateColours()
    {
        var colours = new Color[_textureResolution];
        for (int i = 0; i < _textureResolution; i++) 
            colours[i] = _colourSettings.gradient.Evaluate(i / (_textureResolution - 1f));

        _texture.SetPixels(colours);
        _texture.Apply();
        _colourSettings.planetMaterial.SetTexture("_texture", _texture);
    }
}