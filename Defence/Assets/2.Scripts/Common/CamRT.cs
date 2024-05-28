using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRT : MonoBehaviour
{

   [SerializeField] private CinemachineVirtualCamera mCam;

    [SerializeField] private float size;

    private void Awake()
    {
        mCam = GetComponent<CinemachineVirtualCamera>();

        mCam.m_Lens.OrthographicSize = size / Camera.main.aspect;
    }
    private void Update()
    {
     //   Camera.main.orthographicSize =  size / Camera.main.aspect;

    
    
    }

}
