using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(WaitStrat());
	}
	
	// Update is called once per frame
	IEnumerator WaitStrat()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("Main");
    }
}
