using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public static HandController instance;



    public List<Card> heldCards = new List<Card>();

    public Transform minPos, maxPos;
    public List<Vector3> cardPosition = new List<Vector3>();

    void Start()
    {
        SetCardPositionInHand();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCardPositionInHand() 
    {

        cardPosition.Clear();

        Vector3 distanceBetweenPoints = Vector3.zero;
        if(heldCards.Count > 1) 
        {
            distanceBetweenPoints = (maxPos.position - minPos.position) / (heldCards.Count - 1);
        }

        for(int i = 0; i< heldCards.Count; i++) 
        {
            cardPosition.Add(minPos.position + (distanceBetweenPoints * i));

            //heldCards[i].transform.position = cardPosition[i];
            //heldCards[i].transform.rotation = minPos.rotation;

            //move the card to the hand position
            heldCards[i].MoveToPoint(cardPosition[i],minPos.rotation);

            heldCards[i].inHand = true;
            heldCards[i].handPosition = i;
        }
       
    }
}
