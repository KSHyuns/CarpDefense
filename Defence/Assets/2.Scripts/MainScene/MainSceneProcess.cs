using Cysharp.Threading.Tasks;
using DG.Tweening;
using EasyTransition;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class MainSceneProcess : MonoBehaviour
{

    float[] xpos = new float[2] { -4f , 4f};
    float[] ypos = new float[2] { -5f, 5f };

    public Scriptable_Prefab scriptable;

  

    private float xPos = 3.5f;
    private float yPos = 6.5f;



    // Start is called before the first frame update
    void Start()
    {

        if (GameDataBase.Instance.saveData.haveBungBBangList.Count <= 0) return;

        for (int i = 0; i < GameDataBase.Instance.saveData.haveBungBBangList.Count ; i++)
        {
            //¼ÒÁöÇÏ°í ÀÖ´Â ºØ¾î»§µéÀ» »ý¼º
            BungData data = GameDataBase.Instance.saveData.haveBungBBangList[i];
            var bung = Instantiate(scriptable.interectionBung);
            bung.spriteRenderer.sprite = data.mySprite;
            bung.mainSceneProcess = this;

            float xRand = Random.Range(xPos, -xPos );
            float yRand = Random.Range(yPos, -yPos );

            bung.transform.position = new Vector3(xRand, yRand,1);
            //ºØ¾î»§µéÀÌ µ¹¾Æ´Ù´Ô
            bung.MoveInterectionBung();
        }

        SoundMGR.Instance.SoundPlay(audioName.Main);
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }











}
