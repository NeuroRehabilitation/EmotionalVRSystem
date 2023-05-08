using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationBaker : MonoBehaviour {

    public NavMeshSurface[] surfaces;
    public Transform[] objectsToRotate;
    public GameObject PathMaker;
    public GameObject[] PathMakersSec;

    // builds a mesh in wish the agent can walk and create its path later.
    public void buildNavMesh()
    {
        for (int i = 0; i < surfaces.Length; i++) 
        {
            surfaces [i].BuildNavMesh ();    
        } 
        PathMaker.GetComponent<PathCreator>().DrawPath();
        
    }

}