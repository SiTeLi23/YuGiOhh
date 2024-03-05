using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController instance;

    private void Awake()
    {
        #region singleton
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
    public Transform cardSpawnPoint;

    public enum AIType { placedFromDeck, handRandomPlace, handDefensive, handAttacking }
    public AIType enemyAIType;

    private List<CardScriptableObject> cardsInHand = new List<CardScriptableObject>();
    public int startHandSize;

    void Start()
    {
        SetupDeck();

        if(enemyAIType != AIType.placedFromDeck) 
        {
            SetupHand();
        }
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
            
            tempDeck.RemoveAt(selected);

            iteration++;//make sure the while loop won't last forever
        }
    }

    public void StartAction() 
    {
        StartCoroutine(EnemyActionCo());
    }

    /// <summary>
    /// pick an available random card point and put card on it
    /// </summary>
    IEnumerator EnemyActionCo() 
    {
        if(activeCards.Count == 0) 
        {
            SetupDeck();
        }

        yield return new WaitForSeconds(0.5f);

        if(enemyAIType != AIType.placedFromDeck) 
        {
            //draw cards to fill the cards in hand 
            for (int i = 0; i < BattleController.instance.cardsToDrawPerTurn; i++)
            {
                cardsInHand.Add(activeCards[0]);
                activeCards.RemoveAt(0);

                if(activeCards.Count == 0) 
                {
                    SetupDeck();
                }
            }
        }

        //randomly selected a card point which is empty
        List<CardPlacePoint> cardPoints = new List<CardPlacePoint>();
        cardPoints.AddRange(CardPointController.instance.enemyCardPoints);

        int randomPoint = Random.Range(0, cardPoints.Count);
        CardPlacePoint selectedPoint = cardPoints[randomPoint];

        //loop through to find an empty card point 
        if (enemyAIType == AIType.placedFromDeck || enemyAIType == AIType.handRandomPlace)
        {
            cardPoints.Remove(selectedPoint);

            while (selectedPoint.activeCard != null && cardPoints.Count > 0)
            {
                randomPoint = Random.Range(0, cardPoints.Count);
                selectedPoint = cardPoints[randomPoint];
                cardPoints.RemoveAt(randomPoint);
            }
        }

        CardScriptableObject selectedCard = null;
        int iterations = 0;

        List<CardPlacePoint> preferredPoints = new List<CardPlacePoint>();
        List<CardPlacePoint> secondaryPoints = new List<CardPlacePoint>();

        //AIAction based on AIType
        switch (enemyAIType)
        {
            case AIType.placedFromDeck:
                if (selectedPoint.activeCard == null)
                {
                    Card newCard = Instantiate(cardToSpawn, cardSpawnPoint.position, cardSpawnPoint.rotation);
                    newCard.cardSO = activeCards[0];
                    activeCards.RemoveAt(0);
                    newCard.SetUpCard();
                    newCard.MoveToPoint(selectedPoint.transform.position, selectedPoint.transform.rotation);

                    selectedPoint.activeCard = newCard;
                    newCard.assignedPlace = selectedPoint;
                }
                break;
            case AIType.handRandomPlace:

                selectedCard = SelectedCardToPlay();

                iterations = 50;
                while(selectedCard != null && iterations > 0 && selectedPoint.activeCard == null)
                {
                    PlayCard(selectedCard, selectedPoint);

                    //check if we should try play another card
                    selectedCard = SelectedCardToPlay();

                    iterations--;

                    yield return new WaitForSeconds(CardPointController.instance.timeBetweenAttacks);

                    //check if there's empty card point, if yes, keeps play card until no cards is able to be played
                    while (selectedPoint.activeCard != null && cardPoints.Count > 0)
                    {
                        randomPoint = Random.Range(0, cardPoints.Count);
                        selectedPoint = cardPoints[randomPoint];
                        cardPoints.RemoveAt(randomPoint);
                    }

                }
                break;
            case AIType.handDefensive:

                selectedCard = SelectedCardToPlay();

                preferredPoints.Clear();
                secondaryPoints.Clear();

                
                for (int i = 0; i < cardPoints.Count; i++)
                {
                    if(cardPoints[i].activeCard == null) 
                    {
                       if(CardPointController.instance.playerCardPoints[i].activeCard != null) 
                        {
                            preferredPoints.Add(cardPoints[i]); //if opposite side card points have cards, those points will be preferred points for this AI logic
                        }
                        else 
                        {
                            secondaryPoints.Add(cardPoints[i]);
                        }
                    }
                }

                iterations = 50;
                while(selectedCard != null && iterations > 0 && preferredPoints.Count + secondaryPoints.Count > 0) 
                {
                    //pick a point to be used
                    if(preferredPoints.Count > 0) 
                    {
                        int selectPoint = Random.Range(0, preferredPoints.Count);
                        selectedPoint = preferredPoints[selectPoint];

                        preferredPoints.RemoveAt(selectPoint);
                    }
                    //pick a backup point to be used
                    else 
                    {
                        int selectPoint = Random.Range(0, preferredPoints.Count);
                        selectedPoint = secondaryPoints[selectPoint];

                        secondaryPoints.RemoveAt(selectPoint);
                    }

                    PlayCard(selectedCard, selectedPoint);

                    //check if we can play another card                  
                    selectedCard = SelectedCardToPlay(); //whenever the selecedCard's value changed, the while loop check will be triggered again 

                    iterations--;

                    yield return new WaitForSeconds(CardPointController.instance.timeBetweenAttacks);
                }
                
                break;
            case AIType.handAttacking:

                selectedCard = SelectedCardToPlay();

                preferredPoints.Clear();
                secondaryPoints.Clear();


                for (int i = 0; i < cardPoints.Count; i++)
                {
                    if (cardPoints[i].activeCard == null)
                    {
                        if (CardPointController.instance.playerCardPoints[i].activeCard == null)
                        {
                            preferredPoints.Add(cardPoints[i]); //if opposite side card points do not have cards, those points will be preferred points for this AI logic
                        }
                        else
                        {
                            secondaryPoints.Add(cardPoints[i]);
                        }
                    }
                }

                iterations = 50;
                while (selectedCard != null && iterations > 0 && preferredPoints.Count + secondaryPoints.Count > 0)
                {
                    //pick a point to be used
                    if (preferredPoints.Count > 0)
                    {
                        int selectPoint = Random.Range(0, preferredPoints.Count);
                        selectedPoint = preferredPoints[selectPoint];

                        preferredPoints.RemoveAt(selectPoint);
                    }
                    //pick a backup point to be used
                    else
                    {
                        int selectPoint = Random.Range(0, preferredPoints.Count);
                        selectedPoint = secondaryPoints[selectPoint];

                        secondaryPoints.RemoveAt(selectPoint);
                    }

                    PlayCard(selectedCard, selectedPoint);

                    //check if we can play another card                  
                    selectedCard = SelectedCardToPlay(); //whenever the selecedCard's value changed, the while loop check will be triggered again 

                    iterations--;

                    yield return new WaitForSeconds(CardPointController.instance.timeBetweenAttacks);
                }
                break;
            default:
                break;
        }



        yield return new WaitForSeconds(0.5f);

        BattleController.instance.AdvanceTurn();

    }




    void SetupHand() 
    {
        for (int i = 0; i < startHandSize; i++)
        {
            if(activeCards.Count == 0) 
            {
                SetupDeck();
            }

            cardsInHand.Add(activeCards[0]);
            activeCards.RemoveAt(0);
        }
    }

    public void PlayCard(CardScriptableObject cardSO, CardPlacePoint placePoint) 
    {
        Card newCard = Instantiate(cardToSpawn, cardSpawnPoint.position, cardSpawnPoint.rotation);
        newCard.cardSO = cardSO;

        newCard.SetUpCard();
        newCard.MoveToPoint(placePoint.transform.position, placePoint.transform.rotation);

        placePoint.activeCard = newCard;
        newCard.assignedPlace = placePoint;

        cardsInHand.Remove(cardSO); //remove card from hand

        BattleController.instance.SpendEnemyMana(cardSO.manaCost);
    }

    /// <summary>
    /// select an available card from hands to be played
    /// </summary>
    CardScriptableObject SelectedCardToPlay() 
    {
        CardScriptableObject cardToPlay = null;

        List<CardScriptableObject> cardsToPlay = new List<CardScriptableObject>();
        foreach (CardScriptableObject card in cardsInHand)
        {
            if(card.manaCost <= BattleController.instance.enemyMana) 
            {
                cardsToPlay.Add(card); //find all avaialble card that can be player
            }
        }

        //randomly pick a card to be played from available list
        if(cardsToPlay.Count > 0) 
        {
            int selected = Random.Range(0, cardsToPlay.Count);

            cardToPlay = cardsToPlay[selected];
        }
        return cardToPlay;
    }
}
