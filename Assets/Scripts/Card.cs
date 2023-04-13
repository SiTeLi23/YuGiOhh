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


    private bool isSelected;
    private Collider theCol;

    public LayerMask whatIsDesktop;
    public LayerMask whatIsPlacement;

    private bool justPressed;

    public CardPlacePoint assignedPlace;


    void Start()
    {
        SetUpCard();
        theCol = GetComponent<Collider>();
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

        if (isSelected) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if(Physics.Raycast(ray,out hit, 100f, whatIsDesktop)) 
            {
                MoveToPoint(hit.point + new Vector3(0f,2f,0f),Quaternion.identity);
            }

            if (Input.GetMouseButtonDown(1)) 
            {
                ReturnToHand();
            }

            if (Input.GetMouseButtonDown(0) && justPressed == false) 
            {
               if(Physics.Raycast(ray, out hit, 100f, whatIsPlacement)) 
                {
                    //if the place is able to place the card
                    CardPlacePoint selectedPoint = hit.collider.GetComponent<CardPlacePoint>();

                    if(selectedPoint.activeCard == null && selectedPoint.isPlayerPoint) 
                    {
                        if (BattleController.instance.playerMana >= manaCost)
                        {
                            selectedPoint.activeCard = this;
                            assignedPlace = selectedPoint;

                            MoveToPoint(selectedPoint.transform.position, Quaternion.identity);

                            inHand = false;

                            isSelected = false;

                            HandController.instance.RemoveCardFromHand(this);

                            BattleController.instance.SpendPlayerMana(manaCost);
                        }
                        else 
                        {
                            UIController.instance.ShowManaWarning();
                            ReturnToHand();
                        }
                    }
                    else 
                    {
                        ReturnToHand();
                    }
                }
                else 
                {
                    //if the place is not available
                    ReturnToHand();
                }
            }
        }

        //set back to false
        justPressed = false;
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
            MoveToPoint(HandController.instance.cardPosition[handPosition] + new Vector3(0, 1f, .5f), Quaternion.identity);
        }
    }

    private void OnMouseExit()
    {
        if (inHand)
        {
            MoveToPoint(HandController.instance.cardPosition[handPosition], HandController.instance.minPos.rotation);
        }
    }

    private void OnMouseDown()
    {
        if (inHand) 
        {
            isSelected = true;
            theCol.enabled = false;

            justPressed = true;
        }
    }


    public void ReturnToHand() 
    {
        isSelected = false;
        theCol.enabled = true;

        MoveToPoint(HandController.instance.cardPosition[handPosition], HandController.instance.minPos.rotation);
    }

}
