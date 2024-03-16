using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPointController : MonoBehaviour
{
    public static CardPointController instance;
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

    public CardPlacePoint[] playerCardPoints, enemyCardPoints;

    public float timeBetweenAttacks = .25f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayerAttack() 
    {
        StartCoroutine(PlayerAttackCo());
    }

    IEnumerator PlayerAttackCo() 
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        for(int i = 0; i < playerCardPoints.Length; i++) 
        {
           if(playerCardPoints[i].activeCard != null) 
            {
               if(enemyCardPoints[i].activeCard != null) 
                {
                    //attack the enemy card
                    enemyCardPoints[i].activeCard.DamageCard(playerCardPoints[i].activeCard.attackPower);

                    
                }
                else 
                {
                    //attack the enemy's overall health
                    BattleController.instance.DamageEnemy(playerCardPoints[i].activeCard.attackPower);
                }

                playerCardPoints[i].activeCard.anim.SetTrigger("Attack");

                //AudioManager.instance.PlaySFX(1);
                FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.cardAttack, transform.position);
                yield return new WaitForSeconds(timeBetweenAttacks);
            }

           if(BattleController.instance.battleEnded == true) 
            {
                i = playerCardPoints.Length; // move to the last point to stop card attacking behaviour to break out the loop
            }
        }

        CheckAssignedCards();

        BattleController.instance.AdvanceTurn();
    }

    public void EnemyAttack() 
    {
        StartCoroutine(EnemyAttackCo());
    }

    IEnumerator EnemyAttackCo() 
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        for (int i = 0; i < enemyCardPoints.Length; i++)
        {
            if (enemyCardPoints[i].activeCard != null)
            {
                if (playerCardPoints[i].activeCard != null)
                {
                    //attack the player card
                    playerCardPoints[i].activeCard.DamageCard(enemyCardPoints[i].activeCard.attackPower);

                   
                }
                else
                {
                    //attack the Player's overall health
                    BattleController.instance.DamagePlayer(enemyCardPoints[i].activeCard.attackPower);
                }

                enemyCardPoints[i].activeCard.anim.SetTrigger("Attack");

                //AudioManager.instance.PlaySFX(1);
                FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.cardAttack, transform.position);
                yield return new WaitForSeconds(timeBetweenAttacks);
            }

            if (BattleController.instance.battleEnded == true)
            {
                i = playerCardPoints.Length; // move to the last point to stop card attacking behaviour to break out the loop
            }
        }

        CheckAssignedCards();

        BattleController.instance.AdvanceTurn();

    }

    //make sure there's not assigned card when cards get destroyed
    public void CheckAssignedCards() 
    {
       foreach(CardPlacePoint point in enemyCardPoints) 
        {
            if(point.activeCard != null) 
            {
                if (point.activeCard.currentHealth <= 0) 
                {
                    point.activeCard = null;
                }
            }
        }

        foreach (CardPlacePoint point in playerCardPoints)
        {
            if (point.activeCard != null)
            {
                if (point.activeCard.currentHealth <= 0)
                {
                    point.activeCard = null;
                }
            }
        }
    }
}
