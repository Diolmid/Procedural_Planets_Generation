using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    [Range(1, 8)] public int layersCount = 1;
    public float persistence = 0.5f;
    public float strength = 1;
    public float baseRoughness = 1;
    public float roughness = 1;
    public float minValue;
    public Vector3 center;
}