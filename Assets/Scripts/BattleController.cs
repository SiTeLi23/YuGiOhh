using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController instance;

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

    public int startingMana = 4, maxMana = 12;
    public int playerMana;
    private int currentPlayerMaxMana;

    //turn start setting
    public int startingCardsAmount = 5;
    public int cardsToDrawPerTurn = 2;

    //define the turn process
    public enum TurnOrder { PlayerActive,PlayerCardAttack,EnemyActive,EnemyCardAttacks}

    public TurnOrder currentPhase;
    void Start()
    {
        currentPlayerMaxMana = startingMana;
        FillPlayerMana();
        DeckController.instance.DrawMultipleCards(startingCardsAmount);
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    public void SpendPlayerMana(int amountToSpend) 
    {
        playerMana -= amountToSpend;

        if (playerMana < 0) 
        {
            playerMana = 0;
        }

        UIController.instance.SetPlayerManaText(playerMana);

    }

    public void FillPlayerMana() 
    {
        playerMana = currentPlayerMaxMana;
        UIController.instance.SetPlayerManaText(playerMana);
    }

    //move to next Turn
    public void AdvanceTurn() 
    {
        currentPhase++;
        if((int)currentPhase >= System.Enum.GetValues(typeof(TurnOrder)).Length) 
        {
            currentPhase = 0;
        }

        switch (currentPhase)
        {
            case TurnOrder.PlayerActive:
                UIController.instance.endTurnButton.SetActive(true);
                UIController.instance.drawCardButton.SetActive(true);

                if (currentPlayerMaxMana < maxMana) 
                {
                    currentPlayerMaxMana++;
                }

                FillPlayerMana();

                DeckController.instance.DrawMultipleCards(cardsToDrawPerTurn);
                break;
            case TurnOrder.PlayerCardAttack:
                Debug.Log("magic attack!");
                AdvanceTurn();
                break;
            case TurnOrder.EnemyActive:
                Debug.Log("Enemy Turn now!");
                AdvanceTurn();
                break;
            case TurnOrder.EnemyCardAttacks:
                Debug.Log("Enemy card attack!!");
                AdvanceTurn();
                break;
        }
    }


    public void EndPlayerTurn() 
    {
        UIController.instance.endTurnButton.SetActive(false);
        UIController.instance.drawCardButton.SetActive(false);

        AdvanceTurn();
    }
}
