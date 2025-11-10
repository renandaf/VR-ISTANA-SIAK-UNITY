using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroy[] list = Object.FindObjectsOfType<DontDestroy>();
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i] != this)
            {
                if (list[i].name == gameObject.name)
                {
                    Destroy(gameObject);
                }
            }
        }
        DontDestroyOnLoad(gameObject);
    }
}
