using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MathEasy : MonoBehaviour
{
    private static MathEasy _instance;
    public static MathEasy getInstance
    {
        get
        {
            return _instance;
        }
    }
    public string imgName;

    //当前关卡状态标识
    int levelFlag = 1;
    //是否可以点击
    private bool isClick = true;
    //场景中已经生成的关卡
    public GameObject[] findItem;
    public Animator[] planeFly;
    List<List<int>> equaltionList = new List<List<int>>();

    public AudioSource audioInfo;
    public AudioClip[] audioClips = new AudioClip[2];

	bool isFinish;
    void Awake()
    {
        _instance = this;
    }
    // Use this for initialization
    void Start()
    {
        equaltionList = easyEquationList();
        setNum();
    }

    //设置数字
    public void setNum()
    {
        for (int i = 0; i < findItem.Length; i++)
        {
            findItem[i].transform.GetChild(0).GetComponent<Text>().text = equaltionList[i][0].ToString();
            findItem[i].transform.GetChild(1).GetComponent<Text>().text = equaltionList[i][1].ToString();
        }
    }
    //关卡设置
    public void levelSet(int flag)
    {
        StartCoroutine(hindFindItem(flag));
        switch (flag)
        {
            case 1:
                findItem[0].gameObject.SetActive(true);
                break;
            case 2:
                findItem[1].gameObject.SetActive(true);
                break;
            case 3:
                findItem[2].gameObject.SetActive(true);
                break;
            case 4:
                findItem[3].gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
    //隐藏飞走的飞机
    private IEnumerator hindFindItem(int flag)
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < findItem.Length; i++)
        {
            if (i != flag - 1)
                findItem[i].SetActive(false);
        }
        isClick = true;
    }

    //随机生成简单的分解算式
    public List<int> easyEquation()
    {
        List<int> eqlist = new List<int>();
        int res = Random.Range(2, 10);
        int leftNum = 0;
        int rightNum = 0;
        leftNum = Random.Range(1, res);
        rightNum = res - leftNum;
        eqlist.Add(res);
        eqlist.Add(leftNum);
        eqlist.Add(rightNum);
        return eqlist;
    }
    //生成4组
    public List<List<int>> easyEquationList()
    {
        List<List<int>> easyEqList = new List<List<int>>();
        for (int i = 0; i < 4; i++)
        {
            easyEqList.Add(easyEquation());
        }
        return easyEqList;
    }
    //点击下一关按钮
    public void OnClickNextBtn()
    {
		
		if (isFinish) {
			if (levelFlag != 4 && isClick == true)
			{
				findItem[levelFlag - 1].transform.GetChild(2).GetComponent<Text>().text = "";
				findItem[levelFlag - 1].transform.GetChild(2).GetComponent<Text>().color = Color.black;
				planeFly[levelFlag - 1].SetBool("isgoaway", true);
				levelSet(++levelFlag);
				isClick = false;
			}
			else if (levelFlag == 4)
			{
				SceneManager.LoadScene("Complete");
			}
			isFinish = false;
		}
        
    }
    //点击上一关按钮
    public void OnClickLastBtn()
    {
		
			if (levelFlag != 1 && isClick == true) {
			findItem[levelFlag - 1].transform.GetChild(2).GetComponent<Text>().text = "";
			findItem[levelFlag - 1].transform.GetChild(2).GetComponent<Text>().color = Color.black;
				planeFly [levelFlag - 1].SetBool ("isgoaway", true);
				levelSet (--levelFlag);
				isClick = false;

			//isFinish = false;
		}
    }
    //比较识别的数字与正确的数字是否相同
    public bool compareNum(string obj)
    {
        if (obj.Equals(equaltionList[levelFlag - 1][2].ToString()))
            return true;
        return false;
    }
    //识别成功之后将空白区域数字换成识别的数字
    public void Found(string obj)
    {
        imgName = obj;
        findItem[levelFlag - 1].transform.GetChild(2).GetComponent<Text>().text = obj;
        findItem[levelFlag - 1].transform.GetChild(2).GetComponent<Text>().color = Color.black;
        if (compareNum(obj))
        {
			isFinish = true;
            //对
            StartCoroutine("WaitFound");
            PlayAudio(0);
            imgName = "";
        }

        else
        {
			isFinish = false;
            //错
            PlayAudio(1);
            StopCoroutine("WaitFound");
        }
        isCorrect = false;
    }
    //提示音
    public void PlayAudio(int audioIndex)
    {
        audioInfo.Stop();
        audioInfo.clip = audioClips[audioIndex];
        audioInfo.Play();
    }
    IEnumerator WaitFound()
    {
        yield return new WaitForSeconds(0.5f);
        //OnClickNextBtn();
        StopCoroutine("WaitFound");
    }
    bool isCorrect;
    //点击查看正确结果
    public void CorrectResult()
    {
        isCorrect = !isCorrect;
        if (isCorrect == true)
        {
            findItem[levelFlag - 1].transform.GetChild(2).GetComponent<Text>().text = equaltionList[levelFlag - 1][2].ToString();
            findItem[levelFlag - 1].transform.GetChild(2).GetComponent<Text>().color = Color.red;
        }
        else
        {
            findItem[levelFlag - 1].transform.GetChild(2).GetComponent<Text>().text = imgName;
            findItem[levelFlag - 1].transform.GetChild(2).GetComponent<Text>().color = Color.black;
        }

    }

    public void Back()
    {
        SceneManager.LoadScene("DifficultSelection");
    }
}
