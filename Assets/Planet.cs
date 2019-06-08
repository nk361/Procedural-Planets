using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 10;
    public bool autoUpdate = true;

    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;

    [HideInInspector]
    public bool shapeSettingsFoldOut;
    [HideInInspector]
    public bool colorSettingsFoldOut;

    ShapeGenerator shapeGenerator;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    void Initialize()
    {
        shapeGenerator = new ShapeGenerator(shapeSettings);

        if(meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; ++i)
        {
            if (meshFilters[i] == null)
            {
                //Material test = Resources.Load<Material>("Standard");

                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                //I'm still not sure if this is correct, I selected the materials manually in the unity editor for each mesh
                //meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));// = Resources.Load<Material>("Standard.mat");//I think all works great except this material doesn't load
                //meshObj.AddComponent<MeshRenderer>().sharedMaterial = Instantiate(Resources.Load<Material>("Standard")) as Material;
                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Resources.Load<Material>("Standard"));//no idea if this is doing anything
                //need to put the resource into a resources folder in the project to find
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    public void OnShapeSettingsUpdated()//if only shape settings were changed, call this
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    public void OnColorSettingsUpdated()//if only color settings were changed, call this
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColors();
        }
    }

    void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
    }

    void GenerateColors()
    {
        foreach (MeshFilter m in meshFilters)
        {
            m.GetComponent<MeshRenderer>().sharedMaterial.color = colorSettings.planetColor;
        }
    }
}
