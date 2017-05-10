using UnityEngine;
using System ;
using Random=UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Vuforia;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

	public class test : MonoBehaviour {
	public Text text;
	int ran;
	int count;
	int num;
	int level;	

	//场景中已经生成的关卡
	public GameObject[] findItem;
	public GameObject empty;
	public Canvas can;
	public  GameObject btn1 ;
	public  GameObject btn2 ;
	public  GameObject black ;

	//存储运算式的list
	public List<List<GameObject >> equaltionList=new List<List<GameObject >>();
	public List<GameObject> swords=new List<GameObject>();//23个声母
	public List <GameObject> rndword = new List <GameObject>();//随机出的字母数组
	public List <GameObject> duoword = new List <GameObject>();
	Dictionary<string,int> dic = new  Dictionary<string, int>();//字典 为了和swords中的gameobject对应，以免判断时因为层太多，找不到物体


	public AudioSource audioInfo;
	public AudioClip[] audioclips=new AudioClip[2] ;


	public  static  test   _instance;
	//判断是否按下显示答案的按钮
	bool isPress;
	//判断是否全部成功
	bool isFinish=false ;
	//黑板动画
	public Animator aniBlack;


	void Start () 
	{
		dic.Add ("b", 0);
		dic.Add ("p",1);
		dic.Add ("m", 2);
		dic.Add ("f",3);
		dic.Add ("d", 4);
		dic.Add ("t",5);
		dic.Add ("n", 6);
		dic.Add ("l",7);
		dic.Add ("g", 8);
		dic.Add ("k",9);
		dic.Add ("h", 10);
		dic.Add ("j",11);
		dic.Add ("q", 12);
		dic.Add ("x",13);
		dic.Add ("zh", 14);
		dic.Add ("ch",15);
		dic.Add ("sh",16);
		dic.Add ("r", 17);
		dic.Add ("z",18);
		dic.Add ("c", 19);
		dic.Add ("s",20);
		dic.Add ("y", 21);
		dic.Add ("w",22);
		dic.Add ("a", 23);
		dic.Add ("ai",24);
		dic.Add ("an", 25);
		dic.Add ("ang",26);
		dic.Add ("ao", 27);
		dic.Add ("e",28);
		dic.Add ("ei", 29);
		dic.Add ("en",30);
		dic.Add ("eng",31);
		dic.Add ("er",32);
		dic.Add ("i", 33);
		dic.Add ("ie",34);
		dic.Add ("in", 35);
		dic.Add ("ing",36);
		dic.Add ("iu", 37);
		dic.Add ("o",38);
		dic.Add ("ong",39);
		dic.Add ("ou", 40);
		dic.Add ("u",41);
		dic.Add ("ü", 42);
		dic.Add ("üe",43);
		dic.Add ("ui", 44);
		dic.Add ("un",45);
		dic.Add ("ün",46);
		_instance = this;
		for (int i=0;i<findItem.Length;i++)
		{
			findItem[i].SetActive(false);
		} 
		btn1.name = "before";
		btn2.name = "next";
		if (level == 1)
			btn1.gameObject.SetActive (false );
		if (level == 6)
			btn2.gameObject.SetActive (false  );
		equaltionList = randomEqualtion();
		setlocation(equaltionList, findItem);
	}
		
	public int  change(int level )
	{
		if (level == 1 || level == 2) 
		{
			num = 5;
		} else if (level == 3 || level == 4) 
		{
			num = 4;
		} else if (level == 5 || level == 6)
		{
			num = 3;
		}
		return  num;
	}
	public void playAudio(int audioIndex)
	{
		audioInfo.Stop ();
		audioInfo .clip =audioclips [audioIndex];
		audioInfo.Play ();
	}
	public List<GameObject > randomNum(int point)
	{                                                                                                                               
		List<GameObject > rndword = new List<GameObject> ();//改用局部变量，否则下一步的清除就会有问题
	
		//随机字母
		while (rndword.Count < change (point)) 
		{ 
			ran = Random.Range (0, 23);
			if (!rndword.Contains (swords [ran]))
				rndword.Add (swords [ran]);
		}
		return rndword;
	}
	public List<List<GameObject  >> randomEqualtion()
	{
		List<List<GameObject >> EqList = new List<List<GameObject >>();
		level = 1;
		int i = 0;
		while  (i<6)
		{
			EqList.Add(randomNum(level));
			i++;
			level++;
		}
		level = 1;
		return EqList;
	}
		//随机位置
	public void setlocation(List<List<GameObject>> EqList, GameObject[] findItem)
	{
		int i = level - 1;
		    findItem [i].transform.gameObject.SetActive (true);
			count = 0;
		while(count !=change (level)) 
			{
				ran = Random.Range (0, 6);
				if (findItem[i].transform.GetChild(ran ).transform.childCount == 0) 
				{
					EqList[i][count].transform.SetParent (findItem[i].transform.GetChild(ran).transform);   
					EqList[i][count].transform.localPosition = Vector3.zero;
					EqList[i][count].transform.localScale = Vector3.one;
					EqList[i][count].gameObject.SetActive (true);
				    rndword.Add (EqList[i][count]);
					count++;
				}
			}
	}
	//设置上一关下一关
	public void levelSet(int level)
	{
		for (int i = 0; i < findItem.Length; i++)
		{
			findItem [i].gameObject.SetActive (false);
		}
		switch (level )
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

	public  void  panduan(string  str)
	{
		isPress = false;
		aniBlack.SetBool ("blackboard", isPress);
		int k= dic [str];//判断在字典中的索引值
		if (k > 22) 
		{
			playAudio(1); 
			//isFinish  = false;
			//text.text = "不是声母";	
		} else 
		{
			GameObject go = swords [k];//字典的值和swords中值一样，也就是找到了扫描到的字母
			if (rndword.Contains (go))
			{
				playAudio(1);
				//isFinish  = false;
				//text.text = "字母重复";	
			} else
			{	
				while (rndword.Count < 6) 
				{
					
					ran = Random.Range (0, 6);
					if (findItem [level - 1].transform.GetChild (ran).transform.childCount == 0) 
					{
						go.transform.SetParent (findItem [level - 1].transform.GetChild (ran).transform);
						go.transform.localPosition = Vector3.zero;
						go.transform.localScale = Vector3.one;
						go.gameObject.SetActive (true);
						rndword.Add (go);
						playAudio (0);
						break;
				
					}
				}
				if (rndword .Count ==6)
					isFinish = true;
					if (level == 6) 
					{
						for (int i = 0; i < 6; i++) 
						{
							if (findItem [level - 1].transform.GetChild (i).transform.childCount == 0)
							{
								isFinish = false;
								break;
							} else 
							{
								isFinish = true;
							}
						}
					if(isFinish)
							SceneManager.LoadScene ("Complete");	
				}
					
			 }
		}
	}


	public void NextButtonDown()
	{
		if (isFinish) {
			isFinish = false;
//			if (level == 6) {
//				return;
//			}
			rndword.Clear ();
			for (int i = 0; i < 6; i++) {
				GameObject ji = findItem [level - 1].transform.GetChild (i).gameObject;
				while (ji.transform.childCount != 0)
					ji.transform.GetChild (0).SetParent (GameObject.Find ("empty").transform);
			}
			for (int i = 0; i < empty.transform.childCount; i++) {
				empty.transform.GetChild (i).gameObject.SetActive (false);
			}
			levelSet (++level);
			setlocation (equaltionList, findItem);
		}
	}
	public void FrameButtonDown()
	{

			isFinish = false;
			if (level == 1) {
				return;
			}
			rndword.Clear ();
			for (int i = 0; i < 6; i++) {
				GameObject ji = findItem [level - 1].transform.GetChild (i).gameObject;
				while (ji.transform.childCount != 0)
					ji.transform.GetChild (0).SetParent (GameObject.Find ("empty").transform);
			}
			for (int i = 0; i < empty.transform.childCount; i++) {
				empty.transform.GetChild (i).gameObject.SetActive (false);
			}
			levelSet (--level);
			setlocation (equaltionList, findItem);


	}

	public void lost()
	{
		text.text = "";
	}
	public void fanhui()
	{
		SceneManager.LoadScene ("DifficultSelection");
	}
	public void AniBlack()
	{
		black.SetActive (true);
		isPress = !isPress;
		aniBlack.SetBool ("blackboard", isPress);
	}


}



