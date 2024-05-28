using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "BungBBangResources" , menuName = "scriptable / BungBBang" , order = 2)]
public class BungBBangResources :SerializedScriptableObject
{
    [Title("∫ÿªß Sprites")]
    public Sprite[] bungBBangSprites;

    [Title("∫ÿªß Icon Sprites")]
    public Sprite[] bungBBangIconSprites;

    [Title("∫ÿªß º±≈√πˆ∆∞ Sprite")]
    [TabGroup("Btn ∫ÿªßº±≈√∫Œ∫–")] public Sprite btnSelect;
    [TabGroup("Btn ∫ÿªßº±≈√∫Œ∫–")]  public Sprite btnEmtpy;

    [Title("ø°≥ πÃ Sprite")]
    [TabGroup("ø°≥ πÃ")] public Sprite enemyIdle;
    [TabGroup("ø°≥ πÃ")] public Sprite[] enemyMove;
    [TabGroup("ø°≥ πÃ")] public Sprite enemyDeath;

    [Title("∫ÿªß º±≈√ ¿Œµ¶Ω∫ Sprite")]
    public Sprite[] selectIndexs;

    [Title("BGM DownUp")]
    [TabGroup("BGM πˆ∆∞ ¿ÃπÃ¡ˆ")] public Sprite BGM_on;
    [TabGroup("BGM πˆ∆∞ ¿ÃπÃ¡ˆ")] public Sprite BGM_off;

    public Sprite ShopDefault;
    public Sprite ShopRoom;

    public Sprite DrawDefault;
    public Sprite DrawRoom;

    public Sprite SelectDefault;
    public Sprite SelectRoom;


}
