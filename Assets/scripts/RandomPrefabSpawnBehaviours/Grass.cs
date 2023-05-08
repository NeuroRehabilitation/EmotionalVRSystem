using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class Grass : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "water" || collision.gameObject.tag == "grass")
        {
            // 
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.tag == "terrain")
        {
             
            Destroy (this.GetComponent<Rigidbody>());
            //Destroy (this.GetComponent<Collider>());
            Destroy (this.GetComponent<Grass>());
        }   
    }
}
