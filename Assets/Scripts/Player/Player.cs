using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    public float health;
    [SerializeField] Image healthBar;
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] GameObject winCanvas;
 
    void Start()
    {
        Time.timeScale = 1; //Pause Time
        ToggleCursor(false);
    }

    void Update()
    {
        //Updates the healthbar UI
        healthBar.fillAmount = health / 100;
    }

    public void GameOver()
    {
        ToggleCursor(true);
        Time.timeScale = 0;
        gameOverCanvas.SetActive(true);
    }

    public void Win()
    {
        ToggleCursor(true);
        Time.timeScale = 0.4f;   
        winCanvas.SetActive(true);
    }

    //Restarts the level
    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    //Toggles the state of the cursor
    void ToggleCursor(bool active)
    {
        Cursor.lockState = active? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = !active;
    }
}
