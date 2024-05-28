using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PrefabsObject", menuName = "scriptable / Prefabsobjct")]
public class Scriptable_Prefab : SerializedScriptableObject
{
    public SpawnBlock spownBlockPrefabs;

    public EnemyUnit enemyPrefabs;

    public TowerUnit towerUnit;

    public EnemyHealth enemyHealthSliderPrefabs;

    public Bullet bulletPrefabs;

   // [Title("ºØ»§ ÀúÀå°í Icon ÇÁ¸®ÆÕ")]
    public Image IconPrefabs;

    [Title("ºØ»§ ¼¿·ºÆ®Ä«µå ÇÁ¸®ÆÕ")]
    public BungCard BungCardPrefabs;

    [Title("InterectionBungBBang ÇÁ¸®ÆÕ")]
    public InterectionBung interectionBung;

    public Sprite PlayingGoalTarget;
    public Sprite GameOverGoalTarget;
}