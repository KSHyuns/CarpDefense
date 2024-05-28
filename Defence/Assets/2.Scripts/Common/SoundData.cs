using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SaundData", menuName = "scriptable / SoundData")]
public class SoundData : SerializedScriptableObject
{
    [Title("»ç¿îµå µñ¼Å³Ê¸®")]
    public Dictionary<audioName, soundData> SoundDic = new Dictionary<audioName, soundData>();
}
