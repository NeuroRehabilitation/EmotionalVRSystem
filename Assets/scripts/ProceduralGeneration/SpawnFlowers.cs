using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnFlowers : MonoBehaviour
{
    public GameObject parent;
    public GameObject[] tree;
    public float[] treeScale;
    public GameObject[] placedTrees;
    public int numObjectsToSpawn = 0;
    public bool destroy = true;
    public TMP_Text m_TextComponent;
    public LayerMask TerrainLayer;
    public LayerMask PathLayer;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartSpawning());
    }
    IEnumerator StartSpawning()
    {
        yield return new WaitForSeconds(5f);
        SpawnObject(0);
        SpawnObject(1);
        SpawnObject(2);
        SpawnObject(3);
        SpawnObject(4);
        SpawnObject(5);
        SpawnObject(6);
        SpawnObject(7);
        SpawnObject(8);
        SpawnObject(9);
        SpawnObject(10);
        SpawnObject(11);
    }
    public void ToggleDestroy()
    {
        for (int i = 0; i < placedTrees.Length; i++)
        {
            Destroy(placedTrees[i]);
        }
    }


    public void SpawnObject(int chosen)
    {
        float spawnPointX = 0f;
        float spawnPointZ = 0f;
        placedTrees = new GameObject[numObjectsToSpawn];
        float pocketThreshold = 0.5f; // Adjust this value to control density
        float offsetX = Random.Range(-1300f, 1300f);
        float offsetZ = Random.Range(-1300f, 1300f);
        for (int i = 0; i < numObjectsToSpawn; i++)
        {
            spawnPointX = Random.Range(-300.0f + (255 * transform.GetComponent<MeshGenerator>().tileX), 1300.0f + (255 * transform.GetComponent<MeshGenerator>().tileX));
            spawnPointZ = Random.Range(-200.0f + (255 * transform.GetComponent<MeshGenerator>().tileZ), 1300.0f + (255 * transform.GetComponent<MeshGenerator>().tileZ));
            Vector3 spawnPosition = new Vector3(spawnPointX, 115, spawnPointZ);

            // Use Perlin noise to create pockets
            float noiseValue = Mathf.PerlinNoise((spawnPosition.x + offsetX) * 0.01f, (spawnPosition.z + offsetZ) * 0.01f);

            int layerMask = TerrainLayer.value & ~PathLayer.value;
            if (noiseValue > pocketThreshold)
            {
                RaycastHit hit;
                if (Physics.Raycast(spawnPosition, Vector3.down, out hit, layerMask))
                {
                    if (hit.transform.gameObject.tag != "water" && hit.transform.gameObject.tag != "Waypoint")
                    {
                        placedTrees[i] = Instantiate(tree[chosen], hit.point, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), parent.transform);
                        placedTrees[i].gameObject.transform.localScale += new Vector3(treeScale[4], treeScale[4], treeScale[4]);
                    }

                }
            }
        }
    }



}
