using UnityEngine;

[CreateAssetMenu()]
public class ColourSetting : ScriptableObject
{
    public BiomeColourSettings biomeColourSettings;
    public Material planetMaterial;

    [System.Serializable]
    public class BiomeColourSettings
    {
        public Biome[] biomes;
        public NoiseSettings noiseSettings;
        public float noiseOffset;
        public float noiseStrength;
        [Range(0f, 1f)] public float blendAmount;

        [System.Serializable]
        public class Biome
        {
            [Range(0f, 1f)] public float startHeight;
            [Range(0f, 1f)] public float tintPercent;
            public Color tint;
            public Gradient gradient;
        }
    }
}