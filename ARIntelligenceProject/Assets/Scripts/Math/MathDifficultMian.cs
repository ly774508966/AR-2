using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MathDifficultMian : MonoBehaviour
{

    private static MathDifficultMian _instance;
    public static MathDifficultMian GetInstance
    {
        get
        {
            return _instance;
        }
    }

    //添加所有的识别图下面子物体，用来标志子物体的坐标（相对于屏幕的）
    public List<GameObject> objOfIamge = new List<GameObject>();
    //根据image的名字进行查值，然后根据值去生成
    public Dictionary<string, int> allImage = new Dictionary<string, int>();
    //所有的10个对应的图片
    public GameObject[] allSprites = new GameObject[10];
    //所有3个空框的位置
    public GameObject[] transformPositions = new GameObject[2];
    //所有显示image的父物体
    public GameObject parent;

    //当前关卡状态标识
    int levelFlag = 1;
    //存储数字和符号图像的数组
    public Sprite[] numImageListUp;
    //运算符
    //public Text operatorText;
    //场景中已经生成的关卡
    public GameObject[] findItem;
    //存储运算式的list
    List<List<string>> equaltionList = new List<List<string>>();

    //所有的答案提示对应的十张图
    public GameObject[] allRightImage = new GameObject[10];
    //3个正确空框的位置
    public GameObject[] rightPositions = new GameObject[3];
    //判断正确答案的点击
    bool isClick;
    //判断是否全部完成
    bool isFinish = true;
    //暂存数据
    GameObject[] tempList = new GameObject[3];

    public AudioSource audioInfo;
    public AudioClip[] audioClips = new AudioClip[2];

	bool isRight;
    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        //下面字典添加
        allImage.Add("0", 0);
        allImage.Add("1", 1);
        allImage.Add("2", 2);
        allImage.Add("3", 3);
        allImage.Add("4", 4);
        allImage.Add("5", 5);
        allImage.Add("6", 6);
        allImage.Add("7", 7);
        allImage.Add("8", 8);
        allImage.Add("9", 9);
        //上面对应的字典添加

        equaltionList = randomEqualtion();
        setImage(equaltionList, findItem);

    }

    // Update is called once per frame
    void Update()
    {
        ChangePositions();
    }

    //交换位置
    public void ChangePositions()
    {
        int flag = 0;
        if (objOfIamge.Count >= 2)
        {
            for (int i = 0; i < objOfIamge.Count - 1; i++)
            {
                for (int j = objOfIamge.Count - 1; j > i; j--)
                {
                    if (Camera.main.WorldToScreenPoint(objOfIamge[j - 1].transform.position).x > Camera.main.WorldToScreenPoint(objOfIamge[j].transform.position).x)
                    {
                        GameObject temp;
                        temp = objOfIamge[j - 1];
                        objOfIamge[j - 1] = objOfIamge[j];
                        objOfIamge[j] = temp;
                        flag = 1;
                    }
                }
                if (flag == 0)
                    break;
                else flag = 1;
            }
        }
        for (int k = 0; k < objOfIamge.Count; k++)
        {
            int num = allImage[int.Parse(objOfIamge[k].name).ToString()];
            allSprites[num].transform.SetParent(transformPositions[k].transform);
            allSprites[num].transform.localPosition = Vector3.zero;
            allSprites[num].transform.localScale = Vector3.one;
        }

        if (objOfIamge.Count == 3)
        {
            InvokeRepeating("Time", 0, 0.3f);
            if (isFinish == true)
            {

                isFinish = false;
                objOfIamge.CopyTo(tempList);
                if (compare(objOfIamge, equaltionList[levelFlag - 1]))
                {
                    if (levelFlag == 6)
                        SceneManager.LoadScene("Complete");
                    //完成
                    PlayAudio(0);
                    Debug.LogError("完全正确！！！");
					isRight = true;
                }
                else
                {
                    PlayAudio(1);
					isRight = false;
                    Debug.LogError("答案错误！！！");
                }
            }

        }

    }

    public void PlayAudio(int audioIndex)
    {
        audioInfo.Stop();
        audioInfo.clip = audioClips[audioIndex];
        audioInfo.Play();
    }
    public void Time()
    {
        for (int i = 0; i < objOfIamge.Count; i++)
        {
            if (objOfIamge[i] != tempList[i])
            {
                isFinish = true;
                return;
            }
        }
        return;
    }


    //找到识别图
    public void Found(string foundname)
    {
        isClick = true;
        ClickRightButton();

        GameObject go = GameObject.Find(foundname).transform.GetChild(0).gameObject;
        if (!objOfIamge.Contains(go))
            objOfIamge.Add(go);
        int i = allImage[foundname];
        if (allSprites[i] != null)
            allSprites[i].SetActive(true);
        allSprites[i].transform.SetParent(transformPositions[0].transform);
        allSprites[i].transform.localPosition = Vector3.zero;
        allSprites[i].transform.localScale = Vector3.one;
    }
    //丢失识别图
    public void Lost(string lostName)
    {
        if (lostName != "")
        {
            int i = allImage[lostName];
            if (allSprites[i] != null)
            {
                allSprites[i].SetActive(false);
                allSprites[i].transform.SetParent(parent.transform);
                allSprites[i].transform.localPosition = Vector3.zero;
                allSprites[i].transform.localScale = Vector3.one;
            }

            GameObject lostgo = GameObject.Find(lostName).transform.GetChild(0).gameObject;
            if (lostgo != null && objOfIamge.Contains(lostgo))
                objOfIamge.Remove(lostgo);
        }
    }
    //归还所有的识别图
    public void ReturnImage()
    {
        objOfIamge.Clear();
        for (int i = 0; i < transformPositions.Length; i++)
        {
            if (transformPositions[i].transform.childCount != 0)
            {
                GameObject go = transformPositions[i].transform.GetChild(0).gameObject;
                go.SetActive(false);
                go.transform.SetParent(parent.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
            }
        }
    }
    //点击下一关按钮
    public void OnClickNextBtn()
    {
		if (isRight) {
			if (levelFlag != 6)
			{
				levelSet(++levelFlag);
				ReturnImage();
			}
			isRight = false;
		}
       
    }
    //点击上一关按钮
    public void OnClickLastBtn()
    {
		
			if (levelFlag != 1)
			{
				levelSet(--levelFlag);
				ReturnImage();
			}
			isRight = false;
       
    }

    //设置上一关下一关
    public void levelSet(int flag)
    {
        for (int i = 0; i < findItem.Length; i++)
        {
            findItem[i].SetActive(false);
        }
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
            case 5:
                findItem[4].gameObject.SetActive(true);
                break;
            case 6:
                findItem[5].gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
    //将六组算式对应的图片和符号填入对应的关卡中
    public void setImage(List<List<string>> EqList, GameObject[] findItem)
    {
        for (int i = 0; i < findItem.Length; i++)
        {
            findItem[i].transform.GetChild(0).GetComponent<Image>().sprite = numImageListUp[int.Parse(EqList[i][0])];
            findItem[i].transform.GetChild(1).GetComponent<Image>().sprite = numImageListUp[int.Parse(EqList[i][2])];
        }
    }

    //随机生成六组数学算式存储在算式list列表中
    public List<List<string>> randomEqualtion()
    {
        List<List<string>> EqList = new List<List<string>>();
        for (int i = 0; i < 6; i++)
        {
            EqList.Add(randomNum());
        }
        return EqList;
    }

    //随机生成一组数学算式 x ± y =z 存储在list列表中
    public List<string> randomNum()
    {
        int numRes, numLeft = 0, numRight = 0;
        string op;
        List<string> resList = new List<string>();

        numRes = Random.Range(5, 10);
        op = "+";
        while (numRes == 2 * numLeft || numLeft == 0)
        {
            numLeft = Random.Range(1, numRes);
        }
        numRight = numRes - numLeft;

        resList.Add(numLeft.ToString());
        resList.Add(op);
        resList.Add(numRight.ToString());
        resList.Add("=");
        resList.Add(numRes.ToString());
        return resList;
    }

    //判断识别的算式与正确的算式是否相同
    public bool compare(List<GameObject> res, List<string> correct)
    {
        for (int i = 0; i < res.Count; i++)
        {
            if (int.Parse(res[i].name).ToString() != correct[2 * i])
                return false;
        }
        return true;
    }

    //点击正确答案按钮
    public void ClickRightButton()
    {
        isClick = !isClick;
        if (isClick)
        {
            HideFoundInfo();
            RightInfo(equaltionList[levelFlag - 1]);
        }
        else
        {
            ShowFoundInfo();
            HideRightInfo();
        }
    }

    //提示正确答案,并放在对应位置
    public void RightInfo(List<string> right)
    {
        for (int m = 0; m < 3; m++)
        {
            int i = allImage[right[m * 2]];
            if (allRightImage[i] != null)
            {
                allRightImage[i].SetActive(true);
                allRightImage[i].transform.SetParent(rightPositions[m].transform);
                allRightImage[i].transform.localPosition = Vector3.zero;
                allRightImage[i].transform.localScale = Vector3.one;
            }
        }
    }
    //隐藏所有正确答案
    public void HideRightInfo()
    {
        for (int i = 0; i < rightPositions.Length; i++)
        {
            if (rightPositions[i].transform.childCount != 0)
            {
                GameObject go = rightPositions[i].transform.GetChild(0).gameObject;
                go.gameObject.SetActive(false);
                go.transform.SetParent(parent.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
            }
        }
    }

    //隐藏所有的扫描出来的答案
    public void HideFoundInfo()
    {
        for (int i = 0; i < rightPositions.Length; i++)
        {
            if (rightPositions[i].transform.childCount != 0)
            {
                rightPositions[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
    //显示所有的扫描出来的答案
    public void ShowFoundInfo()
    {
        for (int i = 0; i < rightPositions.Length; i++)
        {
            if (rightPositions[i].transform.childCount != 0)
            {
                rightPositions[i].transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    public void Back()
    {
        SceneManager.LoadScene("DifficultSelection");
    }
}
