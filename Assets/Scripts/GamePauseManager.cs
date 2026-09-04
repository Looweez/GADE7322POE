using UnityEngine;

public class GamePauseManager : MonoBehaviour
{
    public GameObject PauseMenuPanel;
    
    private bool paused = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PauseMenuPanel != null)
            PauseMenuPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        paused = !paused;

        if (PauseMenuPanel != null)
        {
            PauseMenuPanel.SetActive(paused);
        }
        
        Time.timeScale = paused ? 0 : 1;
    }

    public void ResumeGame()
    {
        paused = false;
        if (PauseMenuPanel != null)
            PauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
