using UnityEngine;
using UnityEngine.SceneManagement;

public class TempRestart : MonoBehaviour
{
    public static TempRestart Instance;
    [HideInInspector] public Transform BallParent;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if(Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            FindActiveBall(0);
        }
        else if(Input.GetKeyDown(KeyCode.Keypad2))
        {
            FindActiveBall(1);
        }
        else if(Input.GetKeyDown(KeyCode.Keypad3))
        {
            FindActiveBall(2);
        }
        else if(Input.GetKeyDown(KeyCode.Keypad6))
        {
            WaveManager.Instance.SpawnEnemy(EEnemyType.Small);
        }
        else if(Input.GetKeyDown(KeyCode.Keypad7))
        {
            WaveManager.Instance.SpawnEnemy(EEnemyType.Big);
        }
        else if(Input.GetKeyDown(KeyCode.Keypad8))
        {
            WaveManager.Instance.SpawnEnemy(EEnemyType.Abductor);
        }
        else if(Input.GetKeyDown(KeyCode.Keypad9))
        {
            WaveManager.Instance.SpawnEnemy(EEnemyType.Dropper);
        }
    }

    private void FindActiveBall(int index)
    {
        foreach(Transform child in BallParent)
        {
            if(child.gameObject.activeSelf)
            {
                PowerUpMaster.Instance.SpawnPowerUp(index, child.position, Quaternion.identity);
                return;
            }
        }
    }
}
