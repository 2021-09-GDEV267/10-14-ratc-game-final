using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    static public MoveCam S; 
    [Header("Set In Inspector")]
    public GameObject POI = null;
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;
    [Header("Set Dynamically")]
    public float camZ;

    void Awake()
    {
        camZ = this.transform.position.z;
    }

    // Start is called before the first frame update
    void Start()
    {
        S = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 destination;

        if (POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            destination = POI.transform.position;
            Debug.Log(destination);
            if(transform.position == destination)
            {
                POI = null;
                return;
            }
        }

        destination = Vector3.Lerp(transform.position, destination, easing);

        destination.x = Mathf.Max(minXY.x, destination.x);

        destination.y = Mathf.Max(minXY.y, destination.y);

        destination.z = camZ;

        transform.position = destination;
    }
}
