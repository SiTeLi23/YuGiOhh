using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string battleSelectScene;
    void Start()
    {
        //AudioManager.instance.PlayMenuMusic();
        FMODAudioManager.instance.InitializeMusic(FMODEvents.instance.MainMenumusic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame() 
    {
        SceneManager.LoadScene(battleSelectScene);

        //AudioManager.instance.PlaySFX(0);
    }

    public void QuitGame() 
    {
        Application.Quit();

        //AudioManager.instance.PlaySFX(0);
    }
}
