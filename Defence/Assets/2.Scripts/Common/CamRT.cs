using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRT : MonoBehaviour
{

    [SerializeField] private float size;


    private void Update()
    {
        Camera.main.orthographicSize =  size / Camera.main.aspect;
    }

}
