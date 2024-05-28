using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BungBBangDecPorecess : MonoBehaviour
{
    public Scriptable_Prefab scriptable_Prefabs;
    public BungBBangResources resources;

    public bungDataShowConponent bungShowData;

    public Transform invenParent;
    
    public BungData selectData;

    public Button AddDecButton;
    private Image addImage; 
    public TextMeshProUGUI curMaxCntText;

    public List<BungCard> cardDec = new List<BungCard>();
    private void Awake()
    {
        SoundMGR.Instance.SoundPlay(audioName.BungBBang);

        addImage = AddDecButton.GetComponent<Image>();
        //���� ȹ���� �ػ��� �����ϱ� �׽�Ʈ�� ��ü �����ͷ� ����

        //ī�� ����
        foreach (var data in GameDataBase.Instance.saveData.haveBungBBangList)
        {
            var card = Instantiate(scriptable_Prefabs.BungCardPrefabs, invenParent);
            card.SetProcess(this);
            card.bungImg.sprite = data.mySprite;
            card.NameText.text = string.Format(data.name);
            card.NumberImg.gameObject.SetActive(false);
            card.bungData = data;
            card.isEquip = false;
            cardDec.Add(card);
        }
        //���� �������̶�� ���ý�Ŵ
        for (int i = 0; i < GameDataBase.Instance.equipBungBBangList.Count; i++)
        {
            var data = cardDec.Find(x => x.bungData == GameDataBase.Instance.equipBungBBangList[i]);

            int idx = GameDataBase.Instance.equipBungBBangList.IndexOf(data.bungData);
            curMaxCntText.text = $"{idx + 1} / {5}";

            data.NumberImg.sprite = resources.selectIndexs[idx];
            data.isEquip = true;
        }
        AddDecButton.onClick.AddListener(BungEquip);
        AddDecButton.interactable = true;
    }

    //�����ϱ�
    public void BungEquip()
    {
        //�ػ��� ���ÿϷ�
        if (selectData != null  &&  !string.IsNullOrEmpty( selectData.name))
        {
            //���õ� �ػ��̸� �������ִ� ī�� ����
            var selectCard = cardDec.Find(x => x.bungData == selectData);

            if (selectCard.isEquip == false && GameDataBase.Instance.equipBungBBangList.Count >= 5)
            {
                GameDataBase.Instance.logManager.Show("���������� �����Ͻʽÿ�").Forget();
                return;
            }

            SoundMGR.Instance.SoundPlay(audioName.selectbunng_btn);

            //�������̶��
            if (selectCard != null && selectCard.isEquip)
            {
                //���� ����
                selectCard.isEquip = false;

                //�������̸� ����
                if (GameDataBase.Instance.equipBungBBangList.Contains(selectData))
                    GameDataBase.Instance.equipBungBBangList.Remove(selectData);

               //�������� ����Ʈ �˻�
                for (int i = 0; i < GameDataBase.Instance.equipBungBBangList.Count;i++)
                {
                    //�ش� ī�� ����
                    var card = cardDec.Find(x => x.bungData == GameDataBase.Instance.equipBungBBangList[i]);
                    //�ѹ��� �̹��� ��ü
                    card.NumberImg.sprite = resources.selectIndexs[i];
                }
                //�����ϱ��ư ��ȯ 
                addImage.sprite = resources.btnSelect;
                curMaxCntText.gameObject.SetActive(false);
            }
            else
            { 
                if(!GameDataBase.Instance.equipBungBBangList.Contains(selectData))
                     GameDataBase.Instance.equipBungBBangList.Add(selectData);

                btnTextShow();

                if (GameDataBase.Instance.equipBungBBangList.Count == 5)
                { 
                    Debug.Log("�ػ�� ������ ���ӿ� �����ϼ���");
                    //AddDecButton.interactable = false;
                }
            }
        }
        else
        {
            Debug.Log("�ػ�� �����ϼ��� �ִ� 5");
        }
    }


    public void bungDataInfoShow(BungData data)
    {
        selectData = data;
        bungShowData.image.sprite = data.mySprite;
        bungShowData.nameText.text = data.name;
        bungShowData.stkPointText.text = data.stkpoint.ToString();
        bungShowData.stkSpeedText.text = data.stkspeed.ToString();
        bungShowData.stkRangeText.text = data.stkrange.ToString();
        bungShowData.descriptionText.text = data.description.ToString();

        SoundMGR.Instance.SoundPlay(audioName.selectbung);
        btnTextShow();
    }

    private void btnTextShow()
    {
        var card = GameDataBase.Instance.equipBungBBangList.Find(x => x == selectData);
        if (card != null)
        {
            addImage.sprite = resources.btnEmtpy;
            curMaxCntText.gameObject.SetActive(true);
            int idx = GameDataBase.Instance.equipBungBBangList.IndexOf(selectData);
            int max = GameDataBase.Instance.saveData.haveBungBBangList.Count <= 5 ? GameDataBase.Instance.saveData.haveBungBBangList.Count : 5;
            curMaxCntText.text = $"{idx + 1} / {max}";

            var dt = cardDec.Find(x => x.bungData == selectData);
            dt.NumberImg.sprite = resources.selectIndexs[idx];
            dt.isEquip = true;
        }
        else
        {
            addImage.sprite = resources.btnSelect;
            curMaxCntText.gameObject.SetActive(false);
        }
    }

}
