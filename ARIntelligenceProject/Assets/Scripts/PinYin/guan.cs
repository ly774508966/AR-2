using UnityEngine;
using System.Collections;
using System ;
using Random=UnityEngine.Random;
using System.Collections.Generic;
using UnityEngine.UI;
using Vuforia;
using UnityEngine.SceneManagement;
 
public class guan : MonoBehaviour {
	public Text text;
	int ran;
	int level;
	public Canvas can;
	public GameObject empty;
	public GameObject btn1;
	public GameObject btn2;

	public AudioSource audioInfo;
	public AudioClip[] audioclips=new AudioClip[2] ;

	public List<GameObject> swords=new List<GameObject>();//存储所有的黑白字母
	public List <GameObject > allwords=new List <GameObject >();//存储每一关的黑白字母
	public GameObject[] frames=new GameObject[6] ;
	public GameObject[] tu=new GameObject[6] ;
	List<string> saveWord = new List<string> ();
	public  static guan   _instance;
	bool	isFinish=false ;
	private GameObject emptyPar;
	//bool isRight;
	// Use this for initialization
	void Start () {
		_instance = this;
		 level=0 ;
//		change ( );

		int i = 0;
		while (i<5)
		{
			ran = Random.Range (0,6);
			if (frames [ran].transform.childCount  == 0)
			{
				swords [i].transform.SetParent (frames [ran].transform);   
				swords [i].transform.localScale = Vector3.one;
				swords [i].transform.localPosition = Vector3.zero;
				frames [ran].gameObject.SetActive (true) ;
				tu[ran].gameObject.SetActive (true) ;
				swords [i].gameObject.SetActive (true);
				allwords.Add (swords[i]);
				i++;
			}
		}

		emptyPar =	GameObject.Find ("empty");
		}
	//5个字母进关卡1,6个字母进关卡2
	public void change()
	{
		if (level == 0)
		{
			guanka1 ();
		}
		else 
			guanka2();
	}

	//5个黑白字母的随机分配
public void guanka1()
	{
	int i = 0;
	while (i<5)
		{
		ran = Random.Range (0,6);
			if (frames [ran].transform.childCount  == 0)
			{
			swords [i].transform.SetParent (frames [ran].transform);   
			swords [i].transform.localScale = Vector3.one;
			swords [i].transform.localPosition = Vector3.zero;
			frames [ran].gameObject.SetActive (true) ;
			tu[ran].gameObject.SetActive (true) ;
			swords [i].gameObject.SetActive (true);
			allwords.Add (swords[i]);
			i++;
           }
		}
	}
	//6个黑白字母的随机分配
	public void guanka2()
	{
		int i = 0;
		while (i<6)
		{
			ran = Random.Range (0,6);
			if (frames [ran].transform.childCount  == 0)
			{
				swords [5*level +i+level-1 ].transform.SetParent (frames [ran].transform);   
				swords [5*level +i+level-1 ].transform.localScale = Vector3.one;
				swords [5*level +i +level-1 ].transform.localPosition = Vector3.zero;
				frames [ran].gameObject.SetActive (true) ;
				tu[ran].gameObject.SetActive (true) ;
				swords [5*level +i+level-1 ].gameObject.SetActive (true);
				allwords.Add (swords [5*level +i+level-1]);
				i++;
			}
		}
	}
	public void playAudio(int audioIndex)
	{
		audioInfo.Stop ();
		audioInfo .clip =audioclips [audioIndex];
		audioInfo.Play ();
	}
	//字母相同，换为彩色字母
	public  void  bijiao(string  str)
	{ 
		
		if (allwords.Contains (GameObject.Find (str + str)) || saveWord.Contains (str + str)) 
		{ 
			if (!saveWord.Contains (str + str))//将识别后的加进来
				saveWord.Add (str + str);
			for (int i = 0; i < 6; i++)
			{
				if (frames [i].transform.childCount != 0 && frames [i].transform.GetChild (0).name == str + str) 
				{
					if (frames [i].transform.childCount == 1) 
					{
						GameObject.Find (str + str).transform.gameObject.SetActive (false);
						GameObject root = empty.transform.Find (str).gameObject; 
						if (root != null) 
						{
							root.transform.SetParent (frames [i].transform);
							root.transform.localScale = Vector3.one;
							root.transform.localPosition = Vector3.zero;
							root.gameObject.SetActive (true);
							playAudio (0);
							break;
						}
					}
					    	 
				} else 
				{ 
					playAudio(1);
					//isFinish  = false;
					//text.text = "已找到字母";
				}
			
			}
		}
		else
			playAudio (1);
		if (level == 0 ) {
			if ( saveWord.Count == 5)
				isFinish = true;
		} else {
			if (saveWord.Count == 6)
				isFinish = true;
		}
		if (level == 3) 
		{
			if(isFinish)
				SceneManager.LoadScene ("Complete");	
				}
		//isFinish  = false;
			//text.text = "字母与图片不对应";
	}

			
			

	public void NextButtonDown()
	{
		if (isFinish ) {
			isFinish = false;
//			if (level == 3) {
//				SceneManager.LoadScene ("Complete");
//				return;
//			}
			allwords.Clear ();
			saveWord.Clear ();
			for (int i = 0; i < frames.Length; i++) {
//				while (frames [i].transform.childCount != 0) {
					while(frames [i].transform.childCount!=0){

						frames [i].transform.GetChild (0).SetParent (emptyPar.transform);
					}
				}
				//frames [i].gameObject.SetActive (false);
//			}
			for (int k = 0; k < empty.transform.childCount; k++) {
				empty.transform.GetChild (k).gameObject.SetActive (false);
			}
			if (level != 3)
				level++;
			change ();
		}

	}
	public void FrameButtonDown()
	{
		
			isFinish = false;
			if (level == 0) {
				return;
			}
			allwords.Clear ();
			saveWord.Clear ();
			for (int i = 0; i < frames.Length; i++) {
//				if (frames [i].transform.childCount != 0) {
				while(frames [i].transform.childCount!=0)
					 {
					frames [i].transform.GetChild (0).SetParent (emptyPar.transform);
					}
//				}
				//frames [i].gameObject.SetActive (false);
			}
		for (int i = 0; i < tu.Length; i++) {
			tu [i].gameObject.SetActive (false);
		}

			for (int k = 0; k < empty.transform.childCount; k++) {
				empty.transform.GetChild (k).gameObject.SetActive (false);
			}
			if (level != 0)
				level--;
			change ();
	}
	public void lost()
	{
		text.text = "";
	}
	public void fanhui()
	{
		SceneManager.LoadScene ("DifficultSelection");
	
	}
 }
		



