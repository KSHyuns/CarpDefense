using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BungCard : MonoBehaviour
{
    public Image bungImg;
    public Image NumberImg;
    public TextMeshProUGUI NameText;


    public BungData bungData;

    private BungBBangDecPorecess bungporocess;

    [SerializeField] private bool isequip;
    public bool isEquip
    {   
        get => isequip;
        set 
        { 
            isequip = value;
            NumberImg.gameObject.SetActive(value);
        }
    }

    public void SetProcess(BungBBangDecPorecess _process)
    { 
       bungporocess = _process;
    }

    public void Down()
    { 
        bungporocess.bungDataInfoShow(bungData);
    }

}
