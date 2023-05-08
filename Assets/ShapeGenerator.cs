using UnityEngine;

public class ShapeGenerator : MonoBehaviour
{
    private ShapeSetting _setting;

    public ShapeGenerator(ShapeSetting setting)
    {
        _setting = setting;
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        return pointOnUnitSphere * _setting.planetRadius;
    }
}