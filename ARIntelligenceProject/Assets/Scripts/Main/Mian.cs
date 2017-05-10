using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Mian : MonoBehaviour {
    public static string sceName;
	
    public void ClickButton(GameObject button)
    {
        sceName = button.name;
        SceneManager.LoadScene("DifficultSelection");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
