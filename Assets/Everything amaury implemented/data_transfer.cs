using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class data_transfer : MonoBehaviour
{
    void awake()
    {
        DontDestroyOnLoad(transform.gameObject);

    }
}
