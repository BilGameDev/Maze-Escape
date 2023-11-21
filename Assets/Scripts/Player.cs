
using UnityEditor.SearchService;
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

    [Header("Mobile")]
    [SerializeField] bool mobileMode; //Enable Mobile Controls
    [SerializeField] GameObject mobileCanvas;
 
    void Start()
    {
        Time.timeScale = 1; //Pause Time
        if(mobileMode) mobileCanvas.SetActive(true);
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
        Time.timeScale = 0;   
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
