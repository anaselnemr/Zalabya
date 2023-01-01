using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

    public GameObject PausePanel;
    public static bool ispaused = false;
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
        /*A.volume = MainSound.value;*/
        if (Input.GetKeyDown(KeyCode.Escape) && !ispaused)
        {
            ispaused = true;
            PausePanel.SetActive(true);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && ispaused)
        {
            ispaused = false;
            PausePanel.SetActive(false);
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
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
