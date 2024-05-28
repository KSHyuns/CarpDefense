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
        //아직 획득한 붕빵이 없으니까 테스트로 전체 데이터로 구현

        //카드 세팅
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
        //만약 장착중이라면 세팅시킴
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

    //선택하기
    public void BungEquip()
    {
        //붕빵이 선택완료
        if (selectData != null  &&  !string.IsNullOrEmpty( selectData.name))
        {
            //선택된 붕빵이를 가지고있는 카드 검출
            var selectCard = cardDec.Find(x => x.bungData == selectData);

            if (selectCard.isEquip == false && GameDataBase.Instance.equipBungBBangList.Count >= 5)
            {
                GameDataBase.Instance.logManager.Show("스테이지로 진입하십시오").Forget();
                return;
            }

            SoundMGR.Instance.SoundPlay(audioName.selectbunng_btn);

            //장착중이라면
            if (selectCard != null && selectCard.isEquip)
            {
                //장착 해제
                selectCard.isEquip = false;

                //장착중이면 제거
                if (GameDataBase.Instance.equipBungBBangList.Contains(selectData))
                    GameDataBase.Instance.equipBungBBangList.Remove(selectData);

               //장착중인 리스트 검사
                for (int i = 0; i < GameDataBase.Instance.equipBungBBangList.Count;i++)
                {
                    //해당 카드 검출
                    var card = cardDec.Find(x => x.bungData == GameDataBase.Instance.equipBungBBangList[i]);
                    //넘버링 이미지 교체
                    card.NumberImg.sprite = resources.selectIndexs[i];
                }
                //선택하기버튼 변환 
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
                    Debug.Log("붕빵즈를 데리고 게임에 출전하세요");
                    //AddDecButton.interactable = false;
                }
            }
        }
        else
        {
            Debug.Log("붕빵즈를 세팅하세요 최대 5");
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
