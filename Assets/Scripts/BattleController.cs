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
    public int playerMana,enemyMana;
    private int currentPlayerMaxMana,currentEnemyMaxMana;

    //turn start setting
    public int startingCardsAmount = 5;
    public int cardsToDrawPerTurn = 2;

    //define the turn process
    public enum TurnOrder { PlayerActive,PlayerCardAttack,EnemyActive,EnemyCardAttacks}

    public TurnOrder currentPhase;

    //discard point
    public Transform discardPoint;

    public int playerHealth;
    public int enemyHealth;

    void Start()
    {
        currentPlayerMaxMana = startingMana;
        currentEnemyMaxMana = startingMana;
        FillPlayerMana();
        FillEnemyMana();
        DeckController.instance.DrawMultipleCards(startingCardsAmount);

        UIController.instance.SetPlayerHealthText(playerHealth);
        UIController.instance.SetEnemyHealthText(enemyHealth);
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

    public void SpendEnemyMana(int amountToSpend)
    {
        enemyMana -= amountToSpend;

        if (enemyMana < 0)
        {
            enemyMana = 0;
        }

        UIController.instance.SetEnemyManaText(playerMana);

    }

    public void FillPlayerMana() 
    {
        playerMana = currentPlayerMaxMana;
        UIController.instance.SetPlayerManaText(playerMana);
    }

    public void FillEnemyMana()
    {
        enemyMana = currentEnemyMaxMana;
        UIController.instance.SetEnemyManaText(enemyMana);
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
                CardPointController.instance.PlayerAttack();
                break;
            case TurnOrder.EnemyActive:

                if (currentEnemyMaxMana < maxMana)
                {
                    currentEnemyMaxMana++;
                }
                FillEnemyMana();

                EnemyController.instance.StartAction();
                break;
            case TurnOrder.EnemyCardAttacks:
                Debug.Log("Enemy card attack!!");

                CardPointController.instance.EnemyAttack();
                break;
        }
    }


    public void EndPlayerTurn() 
    {
        UIController.instance.endTurnButton.SetActive(false);
        UIController.instance.drawCardButton.SetActive(false);

        AdvanceTurn();
    }

    public void DamagePlayer(int damageAmount) 
    {
       if(playerHealth > 0) 
        {
            playerHealth -= damageAmount;
            if(playerHealth <= 0) 
            {
                playerHealth = 0;

                //End Battle
            }

            UIController.instance.SetPlayerHealthText(playerHealth);

            UIDamageIndicator damageClone = Instantiate(UIController.instance.playerDamage, UIController.instance.playerDamage.transform.parent);
            damageClone.damageText.text = damageAmount.ToString();
            damageClone.gameObject.SetActive(true);

        }
    }

    public void DamageEnemy(int damageAmount)
    {
        if (enemyHealth > 0)
        {
            enemyHealth -= damageAmount;
            if (enemyHealth <= 0)
            {
                enemyHealth = 0;

                //End Battle
            }

            UIController.instance.SetEnemyHealthText(enemyHealth);

            UIDamageIndicator damageClone = Instantiate(UIController.instance.enemyDamage, UIController.instance.enemyDamage.transform.parent);
            damageClone.damageText.text = damageAmount.ToString();
            damageClone.gameObject.SetActive(true);

        }
    }
}
