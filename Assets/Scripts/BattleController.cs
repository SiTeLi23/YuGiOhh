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

    void Start()
    {
        playerMana = startingMana;
        UIController.instance.SetPlayerManaText(playerMana);
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
}
