using UnityEngine;
using UnityEngine.SceneManagement;

public class TempRestart : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Esc");
            Application.Quit();
        }
        else if(Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("tab");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
