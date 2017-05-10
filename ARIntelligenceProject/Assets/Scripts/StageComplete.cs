using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class StageComplete : MonoBehaviour {

    public Image[] stars;

	// Use this for initialization
	void Start () 
    {
        for (int i = 0; i < stars.Length; ++i)
        {
            stars[i].transform.localScale = Vector3.zero;
            stars[i].transform.DOScale(1f,0.3f).SetDelay(0.3f+0.3f*i);
        }
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

	public void OnReturn()
    {
			SceneManager.LoadScene("DifficultSelection");
    }
    
}
