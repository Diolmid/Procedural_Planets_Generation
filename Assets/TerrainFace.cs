using UnityEngine;

public class TerrainFace
{
    private int _resolution;
    private Vector3 _localUp;
    private Vector3 _axisA;
    private Vector3 _axisB;
    private Mesh _mesh;
    private ShapeGenerator _shapeGenerator;

    public TerrainFace(int resolution, Vector3 localUp, Mesh mesh, ShapeGenerator shapeGenerator)
    {
        _resolution = resolution;
        _localUp = localUp;
        _mesh = mesh;
        _shapeGenerator = shapeGenerator;

        _axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        _axisB = Vector3.Cross(localUp, _axisA);
    }

    public void ConstructMesh()
    {
        var vertices = new Vector3[_resolution * _resolution];
        var triangles = new int[(_resolution - 1) * (_resolution - 1) * 6];
        int trianglesIndex = 0;

        for (int y = 0; y < _resolution; y++)
        {
            for (int x = 0; x < _resolution; x++)
            {
                int i = x + y * _resolution;
                var percent = new Vector2(x, y) / (_resolution - 1);
                var pointOnUnitCube = _localUp + (percent.x - .5f) * 2 * _axisA + (percent.y - .5f) * 2 * _axisB;
                var pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] = _shapeGenerator.CalculatePointOnPlanet(pointOnUnitSphere);

                if(x != _resolution - 1 &&  y != _resolution - 1)
                {
                    triangles[trianglesIndex] = i;
                    triangles[trianglesIndex + 1] = i + _resolution + 1;
                    triangles[trianglesIndex + 2] = i + _resolution;

                    triangles[trianglesIndex + 3] = i;
                    triangles[trianglesIndex + 4] = i + 1;
                    triangles[trianglesIndex + 5] = i + _resolution + 1;
                    trianglesIndex += 6;
                }
            }
        }
        _mesh.Clear();
        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();
    }
}