using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    public static DeckController instance;

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

    public List<CardScriptableObject> deckToUse = new List<CardScriptableObject>();

    private List<CardScriptableObject> activeCards = new List<CardScriptableObject>();

    public Card cardToSpawn;

    public int drawCardCost = 2;

    public float waitBetweenDrawingCards = .25f;

    void Start()
    {
        SetupDeck();
    }

 
    void Update()
    {

    }

    public void SetupDeck() 
    {
        activeCards.Clear();

        List<CardScriptableObject> tempDeck = new List<CardScriptableObject>();
        tempDeck.AddRange(deckToUse);

        int iteration = 0;//error handling
        while (tempDeck.Count > 0 && iteration < 500) 
        {
            int selected = Random.Range(0, tempDeck.Count);
            activeCards.Add(tempDeck[selected]);
            //make sure the while loop won't last forever
            tempDeck.RemoveAt(selected);

            iteration++;
        }
    }

    public void DrawCardToHand() 
    {
      if(activeCards.Count == 0) 
        {
            SetupDeck();
        }

       Card newCard = Instantiate(cardToSpawn, transform.position, transform.rotation);

        newCard.cardSO = activeCards[0];
        newCard.SetUpCard();

        HandController.instance.AddCardToHand(newCard);

        activeCards.RemoveAt(0);

        //AudioManager.instance.PlaySFX(3);

    }

    public void DrawCardForMana() 
    {
       if(BattleController.instance.playerMana >= drawCardCost) 
        {
            DrawCardToHand();
            BattleController.instance.SpendPlayerMana(drawCardCost);

        }
        else 
        {
            UIController.instance.ShowManaWarning();
            UIController.instance.drawCardButton.SetActive(false);
        }
    }


    public void DrawMultipleCards(int amountToDraw) 
    {
        StartCoroutine(DrawMultipleCo(amountToDraw));
    }

    IEnumerator DrawMultipleCo(int amountToDraw) 
    {
        for (int i = 0; i < amountToDraw; i++)
        {
            DrawCardToHand();
            yield return new WaitForSeconds(waitBetweenDrawingCards);
        }      
    }
}
