using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
	public GameObject pnlGameOver;
	public GameObject PausePanel;
	public static bool won = false;
	public GameObject WinPanel;
	public GameObject player;
	public static bool ispaused = false;
	private bool WinOverActive;
    /*public AudioSource A;*/
    /*    public Slider MainSound;
		public Slider EffectsSound;*/
    // Start is called before the first frame update
    void Start()
	{
		/*        if (PlayerPrefs.HasKey("volume"))
                {
                    MusicVolume = (PlayerPrefs.GetFloat("volume"));

                    A.Play();
                    A.volume = MusicVolume;
                    Slider.value = MusicVolume;
                }*/
	}

	// Update is called once per frame
	void Update()
	{
		bool gameOverActive = pnlGameOver.activeSelf;
        if (won)
		{
			Won();
		}
		if (!ispaused && !won  && !gameOverActive)
		{
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (player != null)
			player.SetActive(true);
			Time.timeScale = 1f;
		}
		else
		{
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            player.SetActive(false);
		}
		if(gameOverActive)
		{
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

		if(WinPanel != null)
			WinOverActive = WinPanel.activeSelf;
		else
		{
			WinOverActive = false;

        }
		/*A.volume = MainSound.value;*/
		if (Input.GetKeyDown(KeyCode.Escape) && !ispaused && !gameOverActive && !WinOverActive  )
		{
			Debug.Log("ASFsa");
			Time.timeScale = 0f; 
			ispaused = true;
			PausePanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
		}
		if (Input.GetKeyDown(KeyCode.Escape) && ispaused)
		{	Time.timeScale = 1f;
                ispaused = false;
			PausePanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
		/*        PlayerPrefs.SetFloat("MainSound", MainSound.value);
				PlayerPrefs.SetFloat("EffectsSound", EffectsSound.value);*/
	}

	// for the pause panel
	public void Resume()
	{
		PausePanel.SetActive(false);
		ispaused = false;
	}
    public void Won()
    {
        player.SetActive(false);
		Time.timeScale = 0f;
        WinPanel.SetActive(true);
        
    }
    public void MainMenu()
	{
		Time.timeScale = 1f;
        if (player != null)
		player.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
	/*        ispaused = false;
	*/        SceneManager.LoadScene(0);

	}
	public void Restart()
    {
        won = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		won = false;
		Time.timeScale = 1f;
        if (player != null)
        player.SetActive(true);
		ispaused = false;
	}

}
