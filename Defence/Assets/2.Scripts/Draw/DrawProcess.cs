using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawProcess : MonoBehaviour
{

    public CanvasGroup blackGround;

    public GameObject gachaPanel;


    public Image danchaImg;
    public TextMeshProUGUI dancha_bungName;
    public TextMeshProUGUI dancha_Description;
    public Image[] tenchaImgs;


    public GameObject[] gachaChildPanels;

    public Button enter;

    public Sprite defaultbung;

    private void Awake()
    {
        SoundMGR.Instance.SoundPlay(audioName.Draw);

        blackGround.blocksRaycasts = false;
        //foreach (var item in gachaChildPanels)
        //{
        //    item.SetActive(false);
        //}
        enter.onClick.AddListener(enterPanelOff);
    }

    public void DrawOne()
    {
        int price = 150;

        if (GameDataBase.Instance.saveData.daia >= price)
        {
           // GameDataBase.Instance.saveData.daia -= price;
            DOTween.To(() => GameDataBase.Instance.saveData.daia, x => GameDataBase.Instance.saveData.daia = x, GameDataBase.Instance.saveData.daia - price, 0.7f);
        }
        else
        {
            GameDataBase.Instance.logManager.Show($"������ �����մϴ�").Forget();
            return;
        }
        SoundMGR.Instance.SoundPlay(audioName.reward_btn);
        danchaOpen();
        blackGround.blocksRaycasts = true;
        BlackPanel(0.7f);

        panelOpen(true);

        gachaChildPanels[0].SetActive(true);
    }



    public void DrawTen()
    {
        int price = 1500;
        if (GameDataBase.Instance.saveData.daia >= price)
        {
           // GameDataBase.Instance.saveData.daia -= price;
            DOTween.To(() => GameDataBase.Instance.saveData.daia, x => GameDataBase.Instance.saveData.daia = x, GameDataBase.Instance.saveData.daia - price, 0.7f);
        }
        else
        {
            GameDataBase.Instance.logManager.Show($"������ �����մϴ�").Forget();
            return;
        }
        SoundMGR.Instance.SoundPlay(audioName.reward_btn);
        tenchaOpen();
        blackGround.blocksRaycasts = true;
        BlackPanel(0.7f);
        panelOpen(true);
        gachaChildPanels[1].SetActive(true);
    }

    private void BlackPanel(float alpha)
    {
        blackGround.alpha = alpha;
    }

    private void panelOpen( bool flag)
    {
        gachaPanel.SetActive(flag);
        enter.gameObject.SetActive(flag);
        SoundMGR.Instance.SoundPlay(audioName.popup_on);
    }

    private async void danchaOpen()
    {
        var bungData = randomBung();

        await UniTask.Delay(System.TimeSpan.FromSeconds(0.1f));

        danchaImg.sprite = bungData.IconSprite;
        dancha_bungName.text = bungData.name;
        dancha_Description.text = bungData.description;

        if (GameDataBase.Instance.saveData.haveBungBBangList.Contains(bungData) == false)
        {
            // ���� ���� �ؾ�� �̹� �����ϰ� ���� �ʴٸ� 
            GameDataBase.Instance.saveData.haveBungBBangList.Add(bungData);
        }
        else
        {
            // ���� ���� �ؾ�� �̹� �����ϰ� �ִٸ� 
            GameDataBase.Instance.logManager.Show($"�̹� ������ �ػ��Դϴ�").Forget();
            goldAnim(1).Forget();
        }

    }

    private async void tenchaOpen()
    {
        int cnt = 0;
        foreach (var item in tenchaImgs)
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(0.1f));

            var bungData = randomBung();
            item.sprite = bungData.IconSprite;

            if (GameDataBase.Instance.saveData.haveBungBBangList.Contains(bungData) == false)
            {
                // ���� ���� �ؾ�� �̹� �����ϰ� ���� �ʴٸ� 
                GameDataBase.Instance.saveData.haveBungBBangList.Add(bungData);
            }
            else
            {
                // ���� ���� �ؾ�� �̹� �����ϰ� �ִٸ� 
                cnt++;
            }
        }

        GameDataBase.Instance.logManager.Show($"�̹� ������ �ػ��̴� {cnt}���� �Դϴ�").Forget();
        goldAnim(cnt).Forget();
    }
    async UniTask goldAnim(int cnt)
    {
        await UniTask.Yield();

        //50 * cnt ��ŭ ��尰�� ������ �̵���Ű�� ȿ�� �ֱ�
        int targetGold =  (50 * cnt);
        //�����ϰ� �̵���Ű�� ��ƼŬ �߰�
        await UniTask.Yield();
        DOTween.To(() => GameDataBase.Instance.saveData.gold, x => GameDataBase.Instance.saveData.gold  = x, GameDataBase.Instance.saveData.gold + targetGold, 0.7f);
        // GameDataBase.Instance.saveData.gold += 50 * cnt;
    }

    private BungData randomBung()
    {
        //�ؾ���� Ȯ���� �̹� �����س���.

        return GameDataBase.Instance.wrPicker.GetRandomPick();

        //if (randomSeed > 0f && randomSeed <= 25f) return GameDataBase.Instance.AllbungBBangList[1];
        //else if(randomSeed > 25f && randomSeed <= 50f) return GameDataBase.Instance.AllbungBBangList[2];
        //else if (randomSeed > 50f && randomSeed <= 75f) return GameDataBase.Instance.AllbungBBangList[3];
        //else if (randomSeed > 75f && randomSeed <= 85f) return GameDataBase.Instance.AllbungBBangList[4];
        //else if (randomSeed > 85f && randomSeed <= 95f) return GameDataBase.Instance.AllbungBBangList[5];
        //else if (randomSeed > 95f && randomSeed <= 97f) return GameDataBase.Instance.AllbungBBangList[6];
        //else if (randomSeed > 97f && randomSeed <= 99f) return GameDataBase.Instance.AllbungBBangList[7];
        //else if (randomSeed > 99f && randomSeed <= 99.4f) return GameDataBase.Instance.AllbungBBangList[8];
        //else if (randomSeed > 99.4f && randomSeed <= 99.8f) return GameDataBase.Instance.AllbungBBangList[9];
        //else if (randomSeed > 99.8f && randomSeed <= 99.99f) return GameDataBase.Instance.AllbungBBangList[10];
        //else return GameDataBase.Instance.AllbungBBangList[11];

        // 0~25   �ʺ�
        // 25 ~ 50 �ʷպ���
        //50  ~ 75 �ƻ���
        //75 ~ 85 ������
        //85 ~ 95 �Ⱥ���
        //95 ~ 97 ������
        //97 ~99 ���
        //99 ~ 99.4 ��ũ��
        //99.4 ~ 99.8 ����
        //99.8 ~ 99.99 ���Ϻ�
        //99.99 ~ 100 ����
    }


    private void enterPanelOff()
    {
        BlackPanel(0);
        blackGround.blocksRaycasts = false;
        panelOpen(false);

        foreach (var item in gachaChildPanels)
        {
            item.SetActive(false);
        }

        foreach (var item in tenchaImgs)
        {
            item.sprite = defaultbung;
        }

        danchaImg.sprite = defaultbung;
        dancha_bungName.text = string.Empty;
        dancha_Description.text = string.Empty;

    }



}
