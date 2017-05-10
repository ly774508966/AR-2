using UnityEngine;
using System;
using System.Collections;
//using Vuforia ;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class EnglishSimpleMain : MonoBehaviour
{

    private static EnglishSimpleMain instance;
    public static EnglishSimpleMain Instance
    {
        get
        {
            return instance;
        }
    }
    //存放固定的四组单词
    public string[] Allwordlist = { "boy", "cat", "dog", "girl" };
    //存放字母图片
    public GameObject[] WordsImage;
    //定义关卡
    public GameObject[] image;


    //存放随机生成的四组单词
    public List<List<string>> allword = new List<List<string>>();
    //存放所有字母
    public List<string> Word = new List<string>();
    //添加随机获取的一组单词
    public List<string> listWord = new List<string>();
    //存储单词的list
    List<string> wordsList = new List<string>();
    //存放随机获取的一组单词
    public List<List<string>> SltWord = new List<List<string>>();

    //当前关卡状态标识
    int levelFlag = 1;
    //选择随机出现的单词对应的空框
    int tempValue = -1;
    public int j;
    //识别成功的次数
    int foundCount;

    GameObject pic;
    public GameObject can;
    public GameObject close;
    public GameObject Nextbt, Lastbt;

    //随机选择的单词
    private string str;

    public AudioSource audioInfo;
    public AudioClip[] audioClips = new AudioClip[2];

	bool isRight;
	//统计所有被追踪到的字母个数
	int val;

    void Start()
    {
        instance = this;
        SltWord = FoureRandomWords();
        selectWords();

    }

    public void selectWords()
    {
        wordsList = SltWord[levelFlag - 1];

        listWord.Clear();
        for (int i = 0; i < wordsList[0].Length; i++)
        {
            if (!listWord.Contains(wordsList[0][i].ToString()))
            {
                listWord.Add(wordsList[0][i].ToString());
            }
        }
        //隐藏其他场景，只显示当前场景
        for (int i = 0; i < image.Length; i++)
        {
            image[i].SetActive(false);

        }
        for (int i = 0; i < Allwordlist.Length; i++)
        {
            if (Allwordlist[i] == wordsList[0])
            {
                image[i].SetActive(true);
                j = i;
                break;
            }
        }
    }
    //提示音
    public void PlayAudio(int audioIndex)
    {
        audioInfo.Stop();
        audioInfo.clip = audioClips[audioIndex];
        audioInfo.Play();
    }
    //点击下一关
    public void OnClickNext()
    {
		if (isRight) {
			foundCount = 0;
			val = 0;
			if (levelFlag != 4) {
				ReturnImage ();
				levelSet (++levelFlag);
				selectWords ();
			}
		}
		isRight = false;
	}
    //点击上一关
    public void OnClickLast()
    {
		//if (isRight) {
			foundCount = 0;
		    val = 0;
			if (levelFlag != 1) {
				ReturnImage ();
				levelSet (--levelFlag);
				selectWords ();
			}
	//	}
		isRight = false;
    }

    //设置上一关、下一关
    public void levelSet(int flag)
    {
        int v;
        for (int i = 0; i < image.Length; i++)
        {
            image[i].SetActive(false);

        }
        switch (flag)
        {
            case 1:
                image[0].gameObject.SetActive(true);
                break;
            case 2:
                image[1].gameObject.SetActive(true);
                break;
            case 3:
                image[2].gameObject.SetActive(true);
                break;
            case 4:
                image[3].gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    //随机生成四组单词存储在list列表中
    public List<List<string>> FoureRandomWords()
    {
        //存放四组单词
        List<List<string>> allword = new List<List<string>>();
        List<int> allCount = new List<int>();
        int k;
        while (allCount.Count < 4)
        {
            k = Random.Range(0, Allwordlist.Length);
            if (!allCount.Contains(k))
            {
                allCount.Add(k);
            }
        }
        for (int i = 0; i < allCount.Count; i++)
        {
            List<string> str = new List<string>() { Allwordlist[allCount[i]] };
            allword.Add(str);
        }
        return allword;
    }

    //显示提示信息按钮
    public void Show()
    {
        if (close.activeSelf == false)
            close.SetActive(true);
    }

    //丢失处理
    public void Lost(string lostName)
    {
        close.SetActive(false);
    }
    //识别图片
    public void Check(string name)
    {
		val++;

        if (listWord.Contains(name))
        {
            int y = listWord.IndexOf(name);
            int s = Word.IndexOf(name);
            pic = WordsImage[s];
            if (pic == null)
                return;
            if (image[j].GetComponent<SelectImage>().allBlank[y].transform.childCount == 0)
            {
                pic.SetActive(true);
                pic.transform.SetParent(image[j].GetComponent<SelectImage>().allBlank[y].transform);
                pic.transform.localScale = Vector3.one;
                pic.transform.localPosition = Vector3.zero;
                foundCount++;
				if (foundCount == listWord.Count && foundCount <= val) {

					PlayAudio (0);
					isRight = true;
					if (levelFlag == 4)
						SceneManager.LoadScene ("Complete");
				} 
            }
        
        }
        else
        {
            //识别错误
            PlayAudio(1);

        }

    }

    //归还所有的识别图
    public void ReturnImage()
    {
        for (int i = 0; i < image[j].GetComponent<SelectImage>().allBlank.Count; i++)
        {
            if (image[j].GetComponent<SelectImage>().allBlank[i].transform.childCount != 0)
            {
                GameObject go = image[j].GetComponent<SelectImage>().allBlank[i].transform.GetChild(0).gameObject;
                go.SetActive(false);
                go.transform.SetParent(can.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
            }
        }
    }

    public void Back()
    {
        SceneManager.LoadScene("DifficultSelection");
    }
}
