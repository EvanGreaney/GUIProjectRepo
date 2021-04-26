using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMyo : MonoBehaviour
{

    public static DontDestroyMyo instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Method()
    {
        throw new System.NotImplementedException();
    }
}
