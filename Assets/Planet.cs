using UnityEngine;

public class Planet : MonoBehaviour
{
    public bool autoUpdate = true;
    [HideInInspector]public bool shapeSettingsFoldout;
    [HideInInspector]public bool colourSettingsFoldout;

    public ShapeSetting shapeSetting;
    public ColourSetting colourSetting;

    [Range(2, 256)]
    [SerializeField] private int resolution = 10;
    
    [SerializeField, HideInInspector] 
    private TerrainFace[] _terrainFaces;
    private MeshFilter[] _meshFilters;

    private ShapeGenerator _shapeGenerator;

    private void Initialize()
    {
        _shapeGenerator = new ShapeGenerator(shapeSetting);

        if(_meshFilters == null || _meshFilters.Length == 0)
            _meshFilters = new MeshFilter[6];

        _terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (_meshFilters[i] == null)
            {
                var meshObject = new GameObject("mesh");
                meshObject.transform.parent = transform;

                meshObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                _meshFilters[i] = meshObject.AddComponent<MeshFilter>();
                _meshFilters[i].sharedMesh = new Mesh();
            }    

            _terrainFaces[i] = new TerrainFace(resolution, directions[i], _meshFilters[i].sharedMesh, _shapeGenerator);
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColours();
    }

    public void OnShapeSettingUpdated()
    {
        if (autoUpdate)
        { 
            Initialize();
            GenerateMesh();
        }
    }

    public void OnColourSettingUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColours();
        }
    }

    private void GenerateMesh()
    {
        foreach (var face in _terrainFaces)
        {
            face.ConstructMesh();
        }
    }

    private void GenerateColours()
    {
        foreach (var mesh in _meshFilters)
        {
            mesh.GetComponent<MeshRenderer>().sharedMaterial.color = colourSetting.planetColor;
        }
    }
}