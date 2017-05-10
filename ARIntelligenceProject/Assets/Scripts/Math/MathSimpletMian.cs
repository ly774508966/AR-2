using UnityEngine;
using System.Collections;

public class MathSimpletMian : MonoBehaviour
{
    private static MathSimpletMian _instance;
    public static MathSimpletMian getInstance
    {
        get
        {
            return _instance;
        }
    }
    public string imgName;
    void Awake()
    {
        _instance = this;
    }
    //找到识别图
    public void Found(string imageName)
    {
        imgName = imageName;
    }
    //点击查看正确结果
    public void CorrectResult()
    {

    }
}
