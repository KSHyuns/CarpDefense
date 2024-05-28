using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "BungBBangResources" , menuName = "scriptable / BungBBang" , order = 2)]
public class BungBBangResources :SerializedScriptableObject
{
    [Title("�ػ� Sprites")]
    public Sprite[] bungBBangSprites;

    [Title("�ػ� Icon Sprites")]
    public Sprite[] bungBBangIconSprites;

    [Title("�ػ� ���ù�ư Sprite")]
    [TabGroup("Btn �ػ����úκ�")] public Sprite btnSelect;
    [TabGroup("Btn �ػ����úκ�")]  public Sprite btnEmtpy;

    [Title("���ʹ� Sprite")]
    [TabGroup("���ʹ�")] public Sprite enemyIdle;
    [TabGroup("���ʹ�")] public Sprite[] enemyMove;
    [TabGroup("���ʹ�")] public Sprite enemyDeath;

    [Title("�ػ� ���� �ε��� Sprite")]
    public Sprite[] selectIndexs;

    [Title("BGM DownUp")]
    [TabGroup("BGM ��ư �̹���")] public Sprite BGM_on;
    [TabGroup("BGM ��ư �̹���")] public Sprite BGM_off;

    public Sprite ShopDefault;
    public Sprite ShopRoom;

    public Sprite DrawDefault;
    public Sprite DrawRoom;

    public Sprite SelectDefault;
    public Sprite SelectRoom;


}
