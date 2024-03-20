using System.Collections;
using System.Collections.Generic;
using UGS;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{

	public Slider healthSlider;

	private int health;

	public int Health
	{
		get { return health; }
		set { health = value; }
	}



    private void Awake()
    {
        
    }

    private void Update()
    {
        if (healthSlider != null)
        {
            healthSlider.transform.position = transform.position;

        }
    }



}
