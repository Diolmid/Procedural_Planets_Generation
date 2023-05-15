using UnityEngine;

public class Planet : MonoBehaviour
{
    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back}
    public FaceRenderMask faceRenderMask;

    public bool autoUpdate = true;
    [HideInInspector]public bool shapeSettingsFoldout;
    [HideInInspector]public bool colourSettingsFoldout;

    public ShapeSetting shapeSettings;
    public ColourSetting colourSettings;

    [Range(2, 256)]
    [SerializeField] private int resolution = 10;
    
    [SerializeField, HideInInspector] 
    private TerrainFace[] _terrainFaces;
    private MeshFilter[] _meshFilters;

    private ShapeGenerator _shapeGenerator;
    private ColourGenerator _colourGenerator;

    private void Initialize()
    {
        _shapeGenerator = new ShapeGenerator();
        _colourGenerator = new ColourGenerator();

        _shapeGenerator.UpdateSettings(shapeSettings);
        _colourGenerator.UpdateSettings(colourSettings);

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

                meshObject.AddComponent<MeshRenderer>();
                _meshFilters[i] = meshObject.AddComponent<MeshFilter>();
                _meshFilters[i].sharedMesh = new Mesh();
            }
            _meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colourSettings.planetMaterial;

            _terrainFaces[i] = new TerrainFace(resolution, directions[i], _meshFilters[i].sharedMesh, _shapeGenerator);
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            _meshFilters[i].gameObject.SetActive(renderFace);
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
        for (int i = 0; i < 6; i++)
        {
            if (_meshFilters[i].gameObject.activeSelf)
                _terrainFaces[i].ConstructMesh();
        }

        _colourGenerator.UpdateElevation(_shapeGenerator.elevationMinMax);
    }

    private void GenerateColours()
    {
        _colourGenerator.UpdateColours();
    }
}