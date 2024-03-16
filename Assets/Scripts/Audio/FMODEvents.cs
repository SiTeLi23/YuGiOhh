using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents instance;

    [field: Header("Card SFX")]
    [field: SerializeField] public EventReference cardAttack { get; private set; }



    private void Awake()
    {
        #region singleTon
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        #endregion
    }
}
