using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DifficultSelection : MonoBehaviour {

    public void OnClickPlay(GameObject button)
    {
        Debug.Log(Mian.sceName + button.name);
        SceneManager.LoadScene(Mian.sceName + button.name);
    }
    public void BackMain()
    {
        SceneManager.LoadScene("Main");
    }
}
