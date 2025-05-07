using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour                     //이 스크립트 상속하면 싱글톤 생성 완료
{
    private static T instance=null;
    public static T Instance                                                                                           //instance의 값이 손상되지 않게
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if(instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    instance = obj.GetComponent<T>();
                }

            }
            return instance;
        }
    }
    private void Awake()
    {
        /*if(transform.parent !=null || transform.root != null)                                                             //싱글톤 오브젝트가 파괴되지 않도록 처리
        {
            DontDestroyOnLoad(this.transform.root.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }*/
    }
}
