using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public TMP_Text playerManaText;

    public GameObject manaWarning;
    public float manaWarningTime;
    private float manaWarningCounter;

    //button reference
    public GameObject drawCardButton;
    public GameObject endTurnButton;
    void Start()
    {
        
    }

    // Update is called once per frame
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
    }

    public void SetPlayerManaText(int manaAmount) 
    {
        playerManaText.text = "Mana: " + manaAmount;
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
    }

    public void EndPlayerTurn() 
    {
        BattleController.instance.EndPlayerTurn();
    }
}
