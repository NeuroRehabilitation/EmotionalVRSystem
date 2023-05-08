using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutTerrain : MonoBehaviour
{
    public bool isInWater = false;
    public float yPos = 0f;

    public Vector3 hitPos;

    public bool checkIsInWater()
    {
        RaycastHit hit;
        Ray landingRay = new Ray(transform.position,Vector3.down);
        if (Physics.Raycast(landingRay, out hit, 100f))
        {
            
            if (hit.collider.tag == "water")
            {
                yPos=hit.point.y;
                hitPos=hit.point;
                isInWater = true;
                return true;
            }
            else
            {
                yPos=hit.point.y;
                hitPos=hit.point;
                isInWater = false;
                return false;
            }
            
        }
        return isInWater;
    }

    void Update()
    {
        RaycastHit hit;
        Ray landingRay = new Ray(transform.position,Vector3.down);
        if (Physics.Raycast(landingRay, out hit, 100f))
        {
            if (hit.collider.tag == "water")
            {
                Debug.DrawRay(transform.position, Vector2.down * 100f, Color.blue); // try 
            }
            else
            {
                hitPos=hit.point;
                Debug.DrawRay(transform.position, Vector2.down * 100f, Color.red); // try 
            }
        }
    }
}
