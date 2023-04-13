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

    void Start()
    {
        SetupDeck();
    }

 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) 
        {
            DrawCardToHand();
        }
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

    }
}
