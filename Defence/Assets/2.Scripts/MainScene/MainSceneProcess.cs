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
             //�����ϰ� �ִ� �ؾ���� ����

             //�ؾ���� ���ƴٴ�
        }
    }


    /// <summary>
    /// �����÷��� �� 
    /// </summary>
    public void GameStart()
    {
        //if (TowerDataBase.Instance.equipBungBBangList.Count < 5)
        //{
        //    print($"�ؾ�� ���������� ������ �ؾ�� ���ڶ��ϴ�.  �ʿ� : {5}  ���� : {TowerDataBase.Instance.equipBungBBangList.Count}");
        //    return;
        //}

        SceneChanger.Instance.LoadScene("GameScene" , TransitionManager.Instance().onTransitionEnd = () => { });
    }

    /// <summary>
    /// �ؾ ������
    /// </summary>
    public void BungBBangEnter()
    {
        SceneChanger.Instance.LoadScene("BungBBangScene" , TransitionManager.Instance().onTransitionEnd = () => { });
    }

    /// <summary>
    /// ������
    /// </summary>
    public void ShopEnter()
    {
        SceneChanger.Instance.LoadScene("ShopScene" , TransitionManager.Instance().onTransitionEnd = () => { });
    }

    /// <summary>
    /// �̱� = �� vs �˾�â
    /// </summary>
    public void Drawing()
    {
        SceneChanger.Instance.LoadScene("DrawScene" , TransitionManager.Instance().onTransitionEnd = () => { });
    }



  
}
