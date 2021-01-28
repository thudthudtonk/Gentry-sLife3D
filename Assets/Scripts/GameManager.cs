using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameMenu;
    [SerializeField] GameObject startMenu;
    [SerializeField] Slider hungerSlider;
    [SerializeField] Slider moraleSlider;
    [SerializeField] Button optionButton1;
    [SerializeField] Button optionButton2;
    [SerializeField] Button optionButton3;
    [SerializeField] TextMeshProUGUI optionButton1Text;
    [SerializeField] TextMeshProUGUI optionButton2Text;
    [SerializeField] TextMeshProUGUI optionButton3Text;
    string fileName = "EventData.txt";
    string fullPath;
    int phase;
    string[] lines;
    List<gameEvent> eventDataPhase1 = new List<gameEvent>();
    List<gameEvent> eventDataPhase2 = new List<gameEvent>();

    private int foodBar;
    private int moraleBar;
    
    // Start is called before the first frame update
    void Start()
    {
        gameMenu.SetActive(false);
        startMenu.SetActive(true);
        fullPath = Path.GetFullPath(fileName);
        lines = System.IO.File.ReadAllLines(fullPath);
        initializeEventData();
        testEventData();
        phase = 1;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        gameMenu.SetActive(true);
        startMenu.SetActive(false);
        foodBar = 50;
        moraleBar = 50;
        UpdateSliders();
    }

    public void UpdateSliders()
    {
        hungerSlider.value = foodBar;
        moraleSlider.value = moraleBar;
    }

    void initializeEventData()
    {
     
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] != "phase2")
            {
                
                string[] words = lines[i].Split(',');

                
                gameEvent gameEvent = new gameEvent(words[0]);
                for (int k = 1; k < words.Length; k += 4)
                {
                    outcome outcome = new outcome(int.Parse(words[k]), int.Parse(words[k + 1]), words[k + 2], bool.Parse(words[k + 3]));
                    gameEvent.addOutcome(outcome);
                }

                eventDataPhase1.Add(gameEvent);
            }
            else
            {
                
                for (int j = i + 1; j < lines.Length; j++)
                {
                    
                    string[] words = lines[j].Split(',');


                    gameEvent gameEvent = new gameEvent(words[0]);
                    for (int l = 1; l < words.Length; l += 4)
                    {
                        outcome outcome = new outcome(int.Parse(words[l]), int.Parse(words[l + 1]), words[l + 2], bool.Parse(words[l + 3]));
                        gameEvent.addOutcome(outcome);
                    }
                    eventDataPhase2.Add(gameEvent);
                }

                break;
            }
            

            
        }


    }

    void testEventData()
    {
        foreach (gameEvent word in eventDataPhase1)
        {
            Debug.Log(word.getOutcome(0).getEventText());
        }
    }

    public void randomizeButtons()
    {
        if (phase == 1)
        {
            int randomVal1 = Random.Range(0, eventDataPhase1.Count);
            int randomVal2 = Random.Range(0, eventDataPhase1.Count);
            int randomVal3 = Random.Range(0, eventDataPhase1.Count);
            
            //this method of random non-repeat is slow, clunky, and only works if there are at least 3 events (which there will be)

            /*while (randomVal2 == randomVal1)
            *{
            *    randomVal2 = Random.Range(0, eventDataPhase1.Count);
            *}
            *while (randomVal3 == randomVal1 || randomVal3 == randomVal2)
            *{
            *    randomVal3 = Random.Range(0, eventDataPhase1.Count);
            *}    
            */

            int randomOutcome1 = Random.Range(0, eventDataPhase1[randomVal1].getNumOutcomes());
            int randomOutcome2 = Random.Range(0, eventDataPhase1[randomVal2].getNumOutcomes());
            int randomOutcome3 = Random.Range(0, eventDataPhase1[randomVal3].getNumOutcomes());

            optionButton1Text.text = eventDataPhase1[randomVal1].getName();
            optionButton1.onClick.AddListener(delegate { buttonEvent(eventDataPhase1[randomVal1].getOutcome(randomOutcome1).getHealthVal(),
                eventDataPhase1[randomVal1].getOutcome(randomOutcome1).getMoraleVal(), eventDataPhase1[randomVal1].getOutcome(randomOutcome1).getEventText(),
                eventDataPhase1[randomVal1].getOutcome(randomOutcome1).getHasFriend()); });

            optionButton2Text.text = eventDataPhase1[randomVal2].getName();
            optionButton2.onClick.AddListener(delegate {
                buttonEvent(eventDataPhase1[randomVal2].getOutcome(randomOutcome2).getHealthVal(),
                eventDataPhase1[randomVal2].getOutcome(randomOutcome2).getMoraleVal(), eventDataPhase1[randomVal2].getOutcome(randomOutcome2).getEventText(),
                eventDataPhase1[randomVal2].getOutcome(randomOutcome2).getHasFriend());
            });

            optionButton3Text.text = eventDataPhase1[randomVal3].getName();
            optionButton3.onClick.AddListener(delegate {
                buttonEvent(eventDataPhase1[randomVal3].getOutcome(randomOutcome3).getHealthVal(),
                eventDataPhase1[randomVal3].getOutcome(randomOutcome3).getMoraleVal(), eventDataPhase1[randomVal3].getOutcome(randomOutcome3).getEventText(),
                eventDataPhase1[randomVal3].getOutcome(randomOutcome3).getHasFriend());
            });
            //add a listener to each button using the randomVals to select an index in the eventData lists
            //then from each index of the eventData list grab first the eventName (the first value of tuple)
            //to set the button's text to that value, then add a listener to call buttonEvent using the 
            //value of one of the tuples within the tuple, 


        }
    }
    
    void buttonEvent(int healthVal, int moraleVal, string eventText, bool hasFriend)
    {
        Debug.Log("buttonEvent has been called with values: " + healthVal + ", " + moraleVal + ", " + eventText + ", " + hasFriend);

        toggleOptionButtons(false);
    }


    /* So I want to be able to store event data in some kind of variable structure,
     * I want each event to have an event name, and some number of possible outcomes
     * I want each outcome to have a healthVal, a moraleVal, an eventText, and if it's a phase2 event, a hasFriend bool
     * what about a tuple, with an eventName string, and then multiple additional tuples that are the outcomes?
     * 
     * 
     * declare a new class
     * 
     * 
     * 
     * 
     * 
     * 
     */

    void toggleOptionButtons(bool state)
    {
        optionButton1.interactable = state;
        optionButton2.interactable = state;
        optionButton3.interactable = state;

        //figure out color button change

    }
}



public class gameEvent
{
    public string eventName;
    List<outcome> outcomes = new List<outcome>();
    
    public gameEvent(string e)
    {
        eventName = e;
    }

    public void addOutcome(outcome o)
    {
        outcomes.Add(o);
    }

    public outcome getOutcome(int index)
    {
        return outcomes[index];
    }

    public int getNumOutcomes()
    {
        return outcomes.Count;
    }

    public string getName()
    {
        return eventName;
    }
}

public class outcome
{
    public int healthVal;
    public int moraleVal;
    public string eventText;
    public bool hasFriend;

    public outcome(int h, int m, string e, bool hf)
    {
        healthVal = h;
        moraleVal = m;
        eventText = e;
        hasFriend = hf;
    }

    public int getHealthVal()
    {
        return healthVal;
    }

    public int getMoraleVal()
    {
        return moraleVal;
    }

    public string getEventText()
    {
        return eventText;
    }

    public bool getHasFriend()
    {
        return hasFriend;
    }

} 
