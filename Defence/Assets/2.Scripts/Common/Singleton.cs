using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : SerializedMonoBehaviour where T :Component 
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<T>();

                if(instance == null)
                {
                    var obj = new GameObject();
                    obj.name = typeof(T).Name;
                    instance = obj.AddComponent<T>();
                }
            }


            return instance;
        }
    }

    public virtual  void Awake() 
    {
        if(instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
              if(instance != this) 
                Destroy(gameObject);
            
         
        }
    }


}
