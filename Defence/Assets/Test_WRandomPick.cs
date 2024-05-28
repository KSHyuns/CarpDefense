using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rito.Demo
{
    public class Test_WRandomPick : MonoBehaviour
    {
        public bool _flag;
        private void OnValidate()
        {
            if (_flag)
            {
                _flag = false;
                Test();
            }
        }

        void Test()
        {
            var wrPicker = new Rito.WeightedRandomPicker<string>();

            // 아이템 및 가중치 목록 전달
            wrPicker.Add(

                (GameDataBase.Instance.AllbungBBangList[1].name, GameDataBase.Instance.AllbungBBangList[1].ability),
               (GameDataBase.Instance.AllbungBBangList[2].name, GameDataBase.Instance.AllbungBBangList[2].ability),
                (GameDataBase.Instance.AllbungBBangList[3].name, GameDataBase.Instance.AllbungBBangList[3].ability),

              (GameDataBase.Instance.AllbungBBangList[4].name, GameDataBase.Instance.AllbungBBangList[4].ability),
              (GameDataBase.Instance.AllbungBBangList[5].name, GameDataBase.Instance.AllbungBBangList[5].ability),
              (GameDataBase.Instance.AllbungBBangList[6].name, GameDataBase.Instance.AllbungBBangList[6].ability),
              (GameDataBase.Instance.AllbungBBangList[7].name, GameDataBase.Instance.AllbungBBangList[7].ability),
              (GameDataBase.Instance.AllbungBBangList[8].name, GameDataBase.Instance.AllbungBBangList[8].ability),
              (GameDataBase.Instance.AllbungBBangList[9].name, GameDataBase.Instance.AllbungBBangList[9].ability),
              (GameDataBase.Instance.AllbungBBangList[10].name, GameDataBase.Instance.AllbungBBangList[10].ability),
              (GameDataBase.Instance.AllbungBBangList[11].name, GameDataBase.Instance.AllbungBBangList[11].ability)
            );

            for (int i = 0; i < 10; i++)
            {
                Debug.Log(wrPicker.GetRandomPick());
            }

            //Debug.Log("");
            //foreach (var item in wrPicker.GetItemDictReadonly())
            //{
            //    Debug.Log(item);
            //}

            //Debug.Log("");
            //foreach (var item in wrPicker.GetNormalizedItemDictReadonly())
            //{
            //    Debug.Log(item);
            //}
        }
    }
}