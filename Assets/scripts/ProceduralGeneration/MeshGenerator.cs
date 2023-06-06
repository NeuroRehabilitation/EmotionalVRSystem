using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{

    private Mesh mesh;

    private Vector3[] vertices;
    private Vector2[] uvs;
    int[] triangles;

    
    int xSize = 255;
    int zSize = 255;

    float noiseRange=3.57f;

    [Header("XR Origin")]
    public GameObject XROrigin;

    [Header("Tile Coordinates")]
    public int tileX=0;
    public int tileZ=0;


    [Header("Axis Noise Density")]
    public float XModifier = 0.014f;
    public float ZModifier = 0.014f;

    [Header("Noise Amplitude")]
    public float Height = .3f;

    [Header("Axis Noise Offsets")]
    public float offSetX = .3f;
    public float offSetZ = .3f;

    [Header("Randomizes offsets' value")]
    public int seed = 0;

    [Header("Spawnable Objects")]
    public GameObject[] Bridges;

    [Header("Path and Helpers")]
    public GameObject Scout;
    public Transform PathDestination;
    public GameObject PathMaker;
    public GameObject Finish;
    public bool isStatic=false;

    void Start()
    {

        UpdateMap();
        // debug quicker
        if (isStatic == true)
            return;

        // Spawn bridges
        SpawnRaycasts();
        // Ducktape where I spawn grass and place markers for the pathmaker agent
        StartCoroutine(putGrass());
    }

    public void UpdateMap()
    {
        seedToOffsets();
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        UpdateMesh();
    }

    // extra layer of calibration of the pseudorandom terrain
    // makes it so i can "slide" through the "noise map" of the pseudorandom function
    void seedToOffsets()
    {
        offSetX=seed/2;
        offSetZ=seed/2;
        offSetX=offSetX+tileX*noiseRange;
        offSetZ=offSetZ+tileZ*noiseRange;
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        // optionally, add a mesh collider (As suggested by Franku Kek via Youtube comments).
        // To use this, your MeshGenerator GameObject needs to have a mesh collider
        // component added to it.  Then, just re-enable the code below.
        
        mesh.RecalculateBounds();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }

    IEnumerator putGrass()
    {
        // spawn type of grass 3
        GameObject.Find("Terrain(Clone)").GetComponent<SpawnGrass>().SpawnObject(3);
        yield return new WaitForSeconds(5);

        //THE ducktape
        FitaCola();
    }

    // after defining the path markers, the navmesh can be build. so the agent can create the path
    public void FitaCola()
    {
        PlacePathBeginning();
        //StartCoroutine(WaitMainPath());
        PlacePathEnd();
        transform.parent.transform.parent.gameObject.GetComponent<NavigationBaker>().buildNavMesh();
    }

    // start searching from the other corner of the scene for land using my scout (scout is a cube with a vertical downraycasts that tell me if it's hitting water or terrain)
    // if he finds terrain, place the agent there.
    public void PlacePathBeginning()
    {
        GameObject ScoutPlaced;
        ScoutPlaced = Instantiate (Scout, new Vector3(-130, 150, 1250), Quaternion.identity);

        for (int x=-175;x>-200;x--)
        {
            for (int z=-175;z>-200;z--)
            {
                ScoutPlaced.transform.position = new Vector3(x,150f,z);
                ScoutPlaced.GetComponent<ScoutTerrain>().checkIsInWater();
                if (ScoutPlaced.GetComponent<ScoutTerrain>().isInWater == false && ScoutPlaced.GetComponent<ScoutTerrain>().yPos > 10f)
                {
                    PathMaker.transform.position = new Vector3(x, ScoutPlaced.GetComponent<ScoutTerrain>().yPos, z);
                    XROrigin.transform.position = PathMaker.transform.position;
                    return;
                }
            }
        }
         return;
    }
    
    IEnumerator WaitMainPath()
    {
        yield return new WaitForSeconds(15);
    }

    // start searching from one corner of the scene for land using my scout (scout is a cube with a vertical downraycasts that tell me if it's hitting water or terrain)
    // if he finds terrain, place marker there.
    public void PlacePathEnd()
    {
            GameObject ScoutPlaced;
            ScoutPlaced = Instantiate (Scout, new Vector3(-200, 150, -200), Quaternion.identity);
            ScoutPlaced.transform.position = new Vector3(PathDestination.position.x,150f,PathDestination.position.z);
            ScoutPlaced.GetComponent<ScoutTerrain>().checkIsInWater();
            if (ScoutPlaced.GetComponent<ScoutTerrain>().isInWater == false)
            {
                //PathDestination.transform.position = new Vector3(x, ScoutPlaced.GetComponent<ScoutTerrain>().yPos, z);
                Instantiate (Finish,new Vector3(PathDestination.position.x, ScoutPlaced.GetComponent<ScoutTerrain>().yPos, PathDestination.position.z), Quaternion.identity);

                return;
            }
        return;
    }

    // look for a suitable place to place the bridge
    // each bridge has scouts on the edges, telling me if it's hitting water or land,
    // if the middle scouts are hitting water, and the extremities hitting land, that means its a suitable place to place the bridge
    // each loop makes the bridge: longer, rotates it, changes y pos, changes z pos.
    public void SpawnRaycasts()
    {
        foreach (GameObject Bridge in Bridges)
        {
            bool breakLoop = false;
            for (int w = 0; w <= 10; w++)
            {
                if (breakLoop == true)
                            break;
                Bridge.transform.localScale -= new Vector3(0,0,1);
                for (int y = 0; y <= 360; y=y+15)
                {
                    if (breakLoop == true)
                            break;
                    Bridge.transform.Rotate(.0f,(float)y, 0.0f, Space.Self);
                    for (float x = (tileX)*255; x<255f+(tileX)*255; x+=5)
                    {
                        if (breakLoop == true)
                            break;
                        Bridge.transform.position = new Vector3(x,120f,Bridge.transform.position.z);
                        for (float z =(tileZ)*255; z<255f+(tileZ)*255; z+=5)
                        {
                            Bridge.transform.position = new Vector3(Bridge.transform.position.x,120f,z);
                            if(Bridge.GetComponent<BridgeMain>().checkPlacable() == true)
                            {
                                Bridge.transform.position = new Vector3(Bridge.transform.position.x,Bridge.GetComponent<BridgeMain>().YPosCheck.GetComponent<ScoutTerrain>().yPos+1.5f,z);
                                breakLoop=true;
                                break;
                            }
                        }
                    }
                }
            } 
            if (breakLoop == false)
               Bridge.SetActive(false);      
        } 
    }

    // Creates the mesh for the terrain
    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z =0; z<= zSize; z++)
        {
            for (int x = 0; x<=xSize; x++)
            {
                var temp = Mathf.PerlinNoise(offSetX + x * XModifier, offSetZ + z * ZModifier);
                float y = Mathf.Pow(temp , 4) * Height;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        uvs = new Vector2[vertices. Length];
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                uvs[i] = new Vector2((float)x/ xSize, (float) z / zSize);
                    i++;
            }
                
        }
    }

    
}