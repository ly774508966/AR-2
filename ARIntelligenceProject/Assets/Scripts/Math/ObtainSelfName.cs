using UnityEngine;
using System.Collections;
using System;


public class ObtainSelfName : MonoBehaviour
{
    public string imageNames = null;

    public void Add(string name)
    {
        imageNames = name;
        MathDifficultMian.GetInstance.Found(imageNames);
    }
    public void Remove()
    {
        Debug.Log(imageNames);
        if (MathDifficultMian.GetInstance != null)
        {
            if (imageNames != "")
                MathDifficultMian.GetInstance.Lost(imageNames);
            imageNames = "";
        }

    }
}

