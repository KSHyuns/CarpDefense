using EasyTransition;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MainSceneProcess : MonoBehaviour
{

    float[] xpos = new float[2] { -4f , 4f};
    float[] ypos = new float[2] { -5f, 5f };

    // Start is called before the first frame update
    void Start()
    {
        if (TowerDataBase.Instance.equipBungBBangList.Count <= 0) return;

        for (int i = 0; i < TowerDataBase.Instance.equipBungBBangList.Count ; i++)
        { 
             //¼ÒÁöÇÏ°í ÀÖ´Â ºØ¾î»§µéÀ» »ý¼º

             //ºØ¾î»§µéÀÌ µ¹¾Æ´Ù´Ô
        }
    }


    /// <summary>
    /// °ÔÀÓÇÃ·¹ÀÌ ¾À 
    /// </summary>
    public void GameStart()
    {
        //if (TowerDataBase.Instance.equipBungBBangList.Count < 5)
        //{
        //    print($"ºØ¾î»§À» °¡Á®°¡¼¼¿ä ÀåÂøµÈ ºØ¾î»§ÀÌ ¸ðÀÚ¶ø´Ï´Ù.  ÇÊ¿ä : {5}  ÇöÀç : {TowerDataBase.Instance.equipBungBBangList.Count}");
        //    return;
        //}

        SceneChanger.Instance.LoadScene("GameScene" , TransitionManager.Instance().onTransitionEnd = () => { });
    }

    /// <summary>
    /// ºØ¾î»§ ÀåÂø¾À
    /// </summary>
    public void BungBBangEnter()
    {
        SceneChanger.Instance.LoadScene("BungBBangScene" , TransitionManager.Instance().onTransitionEnd = () => { });
    }

    /// <summary>
    /// »óÁ¡¾À
    /// </summary>
    public void ShopEnter()
    {
        SceneChanger.Instance.LoadScene("ShopScene" , TransitionManager.Instance().onTransitionEnd = () => { });
    }

    /// <summary>
    /// »Ì±â = ¾À vs ÆË¾÷Ã¢
    /// </summary>
    public void Drawing()
    {
        SceneChanger.Instance.LoadScene("DrawScene" , TransitionManager.Instance().onTransitionEnd = () => { });
    }



  
}
