using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PrefabsObject", menuName = "scriptable / Prefabsobjct")]
public class Scriptable_Prefab : ScriptableObject
{
    public SpawnBlock spownBlockPrefabs;

    public EnemyUnit enemyPrefabs;

    public TowerUnit towerUnit;

    public Slider enemyHealthSliderPrefabs;


}