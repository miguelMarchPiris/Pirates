using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Script : MonoBehaviour
{
    private Transform player_transform;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        GameObject p = GameObject.FindGameObjectWithTag("Boat");
        if (p == null)
        {
            Debug.Log("ERROR: El script de la cámara no encuentra al Boat");
        }
        else
        { 
            player_transform = p.transform;
        }
        
        Debug.Log("Player transform: "+ p);

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = player_transform.position;
        v = player_transform.position + offset;
        this.transform.position = v;
    }
}
