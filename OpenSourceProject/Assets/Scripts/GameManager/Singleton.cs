using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour                     //�� ��ũ��Ʈ ����ϸ� �̱��� ���� �Ϸ�
{
    private static T instance=null;
    public static T Instance                                                                                           //instance�� ���� �ջ���� �ʰ�
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindFirstObjectByType(typeof(T));
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
        /*if(transform.parent !=null || transform.root != null)                                                             //�̱��� ������Ʈ�� �ı����� �ʵ��� ó��
        {
            DontDestroyOnLoad(this.transform.root.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }*/
    }
}
