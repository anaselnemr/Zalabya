using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
	public GameObject MainPanel;
	public GameObject PlayPanel;
	public GameObject OptionsPanel;
	/*public AudioSource A;*/
	public Slider MainSound;
	public Slider EffectsSound;
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
		PlayerPrefs.SetFloat("MainSound", MainSound.value);
		PlayerPrefs.SetFloat("EffectsSound", EffectsSound.value);
	}
	public void play()
	{
		//panel of play
		MainPanel.SetActive(false);
		PlayPanel.SetActive(true);
	}
	public void Level1()
	{
		SceneManager.LoadScene(1);
	}
	public void Level2()
	{
		SceneManager.LoadScene(2);
	}
	public void Level3()
	{
		SceneManager.LoadScene(3);
	}
	public void Level4()
	{
		SceneManager.LoadScene(4);
	}
	public void options()
	{
		//panel options
		MainPanel.SetActive(false);
		OptionsPanel.SetActive(true);
	}
	public void back()
	{
		//panel options
		if (PlayPanel.activeSelf)
		{

			MainPanel.SetActive(true);
			PlayPanel.SetActive(false);
		}
		if (OptionsPanel.activeSelf)
		{

			MainPanel.SetActive(true);
			OptionsPanel.SetActive(false);
		}
	}
	public void Quit()
	{
		Application.Quit();
	}


}
