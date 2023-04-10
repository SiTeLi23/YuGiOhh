using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardScriptableObject cardSO;

    public int currentHealth;
    public int attackPower, manaCost;

    public TMP_Text healthText, attackText, costText, nameText, actionDescriptionText, loreText;

    public Image characterArt, bgArt;

    private Vector3 targetPoint;
    private Quaternion targetRot;
    public float moveSpeed = 5f, rotateSpeed = 540f;


    public bool inHand;
    public int handPosition;

    private HandController theHC;


    void Start()
    {
        SetUpCard();
        theHC = FindObjectOfType<HandController>();
    }

    public void SetUpCard() 
    {
        currentHealth = cardSO.currentHealth;
        attackPower = cardSO.attackPower;
        manaCost = cardSO.manaCost;


        healthText.text = currentHealth.ToString();
        attackText.text = attackPower.ToString();
        costText.text = manaCost.ToString();

        nameText.text = cardSO.cardName;
        actionDescriptionText.text = cardSO.actionDescription;
        loreText.text = cardSO.cardLore;

        characterArt.sprite = cardSO.characterSprite;
        bgArt.sprite = cardSO.bgSprite;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPoint, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRot, rotateSpeed * Time.deltaTime);
    }


    public void MoveToPoint(Vector3 pointToMoveTo, Quaternion rotToMatch) 
    {
        targetPoint = pointToMoveTo;
        targetRot = rotToMatch;
       
    }

    private void OnMouseOver()
    {
        if (inHand) 
        {
            MoveToPoint(theHC.cardPosition[handPosition] + new Vector3(0, 1f, .5f), Quaternion.identity);
        }
    }

    private void OnMouseExit()
    {
        if (inHand)
        {
            MoveToPoint(theHC.cardPosition[handPosition], theHC.minPos.rotation);
        }
    }
}
