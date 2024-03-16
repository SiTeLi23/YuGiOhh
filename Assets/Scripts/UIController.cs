using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    
    private void Awake()
    {
        #region singleTon
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    public TMP_Text playerManaText, playerHealthText, enemyManaText, enemyHealthText;

    public GameObject manaWarning;
    public float manaWarningTime;
    private float manaWarningCounter;

    //button reference
    public GameObject drawCardButton;
    public GameObject endTurnButton;

    public UIDamageIndicator playerDamage, enemyDamage;

    public GameObject battleEndScreen;
    public TMP_Text battleResultText;

    public string mainMenuScene, battlSelectScene;

    public GameObject pauseScreen;
    void Start()
    {
        
    }

 
    void Update()
    {
        if(manaWarningCounter > 0) 
        {
            manaWarningCounter -= Time.deltaTime;
            if(manaWarningCounter <= 0) 
            {
                
                manaWarning.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            PauseUnPause();
        }
    }

    public void SetPlayerManaText(int manaAmount) 
    {
        playerManaText.text = "Mana: " + manaAmount;
    }

    public void SetEnemyManaText(int manaAmount)
    {
        enemyManaText.text = "Mana: " + manaAmount;
    }

    public void SetPlayerHealthText(int healthAmount) 
    {
        playerHealthText.text = "Player Health: " + healthAmount;
    }

    public void SetEnemyHealthText(int healthAmount)
    {
        enemyHealthText.text = "Enemy Health: " + healthAmount;
    }

    public void ShowManaWarning() 
    {
        manaWarning.SetActive(true);
        manaWarningCounter = manaWarningTime;
    }

    //button function
    public void DrawCard() 
    {
        DeckController.instance.DrawCardForMana();

        //AudioManager.instance.PlaySFX(0);
    }

    public void EndPlayerTurn() 
    {
        BattleController.instance.EndPlayerTurn();

        //AudioManager.instance.PlaySFX(0);
    }

    public void MainMenu() 
    {
        SceneManager.LoadScene(mainMenuScene);

        Time.timeScale = 1f;

        //AudioManager.instance.PlaySFX(0);
    }

    public void RestartLevel() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Time.timeScale = 1f;

        //AudioManager.instance.PlaySFX(0);
    }

    public void ChoooseNewBattle() 
    {
        SceneManager.LoadScene(battlSelectScene);

        Time.timeScale = 1f;

        //AudioManager.instance.PlaySFX(0);
    }

    public void PauseUnPause() 
    {
       if(pauseScreen.activeSelf == false) 
        {
            pauseScreen.SetActive(true);

            Time.timeScale = 0f;
        }
        else 
        {
            pauseScreen.SetActive(false);

            Time.timeScale = 1f;
        }

        //AudioManager.instance.PlaySFX(0);
    }

    public void Resume() 
    {
        pauseScreen.SetActive(false);

        Time.timeScale = 1f;
    }
}
