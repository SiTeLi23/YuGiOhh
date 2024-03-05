using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDamageIndicator : MonoBehaviour
{
    public TMP_Text damageText;

    public float moveSpeed = 100f;

    public float lifeTime = 3f;

    private RectTransform myRect;
    void Start()
    {
        myRect = GetComponent<RectTransform>();
        damageText = GetComponent<TMP_Text>();

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        myRect.anchoredPosition += new Vector2(0f, -moveSpeed*Time.deltaTime);
    }
}
