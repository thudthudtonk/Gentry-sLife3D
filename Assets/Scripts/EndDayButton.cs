using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndDayButton : MonoBehaviour
{

    //Call the nextDay() function from gameManager

    [SerializeField] GameObject gameManager;
    [SerializeField] Button endDayButton;

    void Start()
    {
        endDayButton.onClick.AddListener(delegate { gameManager.GetComponent<GameManager>().nextDay(); });
    }

    void Update()
    {
        
    }

}
