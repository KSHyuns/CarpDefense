using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public IObjectPool<EnemyHealth> Ipool;

	public Slider healthSlider;

	
    private void Awake()
    {
        healthSlider ??= GetComponent<Slider>();
    }

    public void Release()
    {
      //  healthSlider = null;
        Ipool.Release(this);   

    }
}
