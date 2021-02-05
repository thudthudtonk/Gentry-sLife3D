using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenialLaborButton : MonoBehaviour
{

    [SerializeField] GameObject gameManager;
    [SerializeField] Button menialLaborButton;

    // Start is called before the first frame update
    void Start()
    {
        menialLaborButton.onClick.AddListener(delegate { gameManager.GetComponent<GameManager>().startMenialLabor(); 
                                                         menialLaborButton.interactable = false; });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
