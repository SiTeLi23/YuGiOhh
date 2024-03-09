using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSelectButton : MonoBehaviour
{
    public string levelToLoad;
    void Start()
    {
        AudioManager.instance.PlayBattleSelectMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectBattle() 
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
