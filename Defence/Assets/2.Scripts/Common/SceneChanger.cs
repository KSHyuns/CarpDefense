using UnityEngine;
using UnityEngine.SceneManagement;
using EasyTransition;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;


[System.Serializable]
public struct btnComponent
{
    public Image shop;
    public Image draw;
    public Image select;
}

public class SceneChanger : Singleton<SceneChanger>
{
    public TransitionSettings transition;
    public float startDelay;

    public GameObject moneyPanel;
    public GameObject btnPanel;
    public GameObject jewelPanel;

    private bool isChange = false;

    FullScreenMode fullScreenMode = FullScreenMode.Windowed;

    public btnComponent Btns = new btnComponent();
    private void Start()
    {
        moneyPanel.gameObject.SetActive(false);
        btnPanel.gameObject.SetActive(false);
        jewelPanel.gameObject.SetActive(false);

#if UNITY_ANDROID
#else
        fullScreenMode = FullScreenMode.Windowed;
        Screen.SetResolution(480, 920, fullScreenMode);
#endif
        }
    public override void Awake()
    {
        base.Awake();
    }
    public void LoadScene(string SceneName)
    {
        SoundMGR.Instance.SoundPlay(audioName.MenuBtn);
        if (SceneManager.GetActiveScene().name == SceneName || isChange) return;

        isChange = true;
        TransitionManager.Instance().Transition(SceneName, transition, startDelay);
    }


    public void MainScene()
    {
        TransitionManager.Instance().onTransitionCutPointReached = () =>
        {
            jewelPanel.SetActive(true);
            btnPanel.SetActive(true);
            isChange = false;

            GoImageChange(GameDataBase.Instance.bungBBangResources.ShopDefault,
                                     GameDataBase.Instance.bungBBangResources.DrawDefault,
                                     GameDataBase.Instance.bungBBangResources.SelectDefault);
        };
        LoadScene("MainScene");
    }


    /// <summary>
    /// �����÷��� �� 
    /// </summary>
    public void GameStart()
    {
        if (GameDataBase.Instance.equipBungBBangList.Count < 1)
        {
            GameDataBase.Instance.logManager.Show($"�ؾ�� ���������� ������ �ؾ�� ���ڶ��ϴ�.   ���� : {GameDataBase.Instance.equipBungBBangList.Count} ����").Forget();
            return;
        }
        TransitionManager.Instance().onTransitionCutPointReached = () => 
        {
            jewelPanel.SetActive(false);
            btnPanel.SetActive(false);
            isChange = false;

            GoImageChange(GameDataBase.Instance.bungBBangResources.ShopDefault,
                                     GameDataBase.Instance.bungBBangResources.DrawDefault, 
                                     GameDataBase.Instance.bungBBangResources.SelectDefault);
        };
        LoadScene("GameScene");
    }

    /// <summary>
    /// �ؾ ������
    /// </summary>
    public void BungBBangEnter()
    {
        TransitionManager.Instance().onTransitionCutPointReached = () =>
        {
            jewelPanel.SetActive(false);
            btnPanel.SetActive(true);
            isChange = false;

            GoImageChange(GameDataBase.Instance.bungBBangResources.ShopDefault,
                                     GameDataBase.Instance.bungBBangResources.DrawDefault,
                                     GameDataBase.Instance.bungBBangResources.SelectRoom);
        };
        LoadScene("BungBBangScene");
    }

    /// <summary>
    /// ������
    /// </summary>
    public void ShopEnter()
    {
        TransitionManager.Instance().onTransitionCutPointReached = () =>
        {
            jewelPanel.SetActive(false);
            btnPanel.SetActive(true);
            isChange = false;

            GoImageChange(GameDataBase.Instance.bungBBangResources.ShopRoom,
                                     GameDataBase.Instance.bungBBangResources.DrawDefault,
                                     GameDataBase.Instance.bungBBangResources.SelectDefault);
        };
         LoadScene("ShopScene");
        //  GameDataBase.Instance.logManager.Show("�����̿�").Forget();

    }

    /// <summary>
    /// �̱� = �� vs �˾�â
    /// </summary>
    public void Drawing()
    {
        TransitionManager.Instance().onTransitionCutPointReached = () =>
        {
            jewelPanel.SetActive(true);
            btnPanel.SetActive(true);
            isChange = false;

            GoImageChange(GameDataBase.Instance.bungBBangResources.ShopDefault,
                                     GameDataBase.Instance.bungBBangResources.DrawRoom,
                                     GameDataBase.Instance.bungBBangResources.SelectDefault);
        };
        LoadScene("DrawScene");
    }


    public void GoImageChange(Sprite shop , Sprite draw , Sprite select )
    {
        Btns.shop.sprite = shop;
        Btns.select.sprite = select;
        Btns.draw.sprite = draw;
    }




}
