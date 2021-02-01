﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Game objects
    [SerializeField] GameObject gameMenu;
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject goldenCoin;
    [SerializeField] GameObject coinSpawnPoint;

    // UI Elements

    // Sliders
    [SerializeField] Slider hungerSlider;
    [SerializeField] Slider moraleSlider;
    
    // Buttons
    [SerializeField] Button optionButton1;
    [SerializeField] Button optionButton2;
    [SerializeField] Button optionButton3;
    
    // Text
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI optionButton1Text;
    [SerializeField] TextMeshProUGUI optionButton2Text;
    [SerializeField] TextMeshProUGUI optionButton3Text;
    
    // File system/management
    string fileName = "EventData.txt";
    string fullPath;
    string[] lines;
    
    // Game object lists
    List<gameEvent> eventDataPhase1 = new List<gameEvent>();
    List<gameEvent> eventDataPhase2 = new List<gameEvent>();
    List<GameObject> coinClones = new List<GameObject>();

    // Character progress/values
    int phase;
    private int foodBar;
    private int moraleBar;
    private int friendAmount;
    private int moneyBalance;
    
    // Start is called before the first frame update
    /* Sets correct menus active and inactive
     * Establishes file path and reads file lines into "lines" array
     * Runs initializeEventData
     * Initializes phase to 1
     */

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

    /* Switches menu to gameMenu
     * Updates food and morale sliders to start value of 50
     * Updates money to default amount of 5 dollars
     */
    public void StartGame()
    {
        gameMenu.SetActive(true);
        startMenu.SetActive(false);

        UpdateSliders(50, 50);
        UpdateMoney(5);
    }

    /* Adds input value to foodBar and moraleBar values
     * Updates slider accordingly
     */
    public void UpdateSliders(int h, int m)
    {

        foodBar += h;
        moraleBar += m;
        
        hungerSlider.value = foodBar;
        moraleSlider.value = moraleBar;
    }

    /* Adds input value to moneyBalance value
     * Updates moneyText accordingly
     * Runs through a loop to spawn coins until there are as many coins in your jar
     * as you have money, using the coinClones list to store the objects
     * To fix: Currently the starting 1 coin that I clone to spawn the other coins
     * just sits in the jar active, I either need to move it out of view or figure
     * out how to activate each clone as it's instantiated
     */
    public void UpdateMoney(int m)
    {
        moneyBalance += m;
        moneyText.text = "Money: " + moneyBalance;

        if (coinClones.Count < moneyBalance)
        {
            for (int i = coinClones.Count; i < moneyBalance; i++)
            {
                coinClones.Add(Instantiate(goldenCoin, coinSpawnPoint.transform.position, goldenCoin.transform.rotation));
            }
            Debug.Log(coinClones.Count);
        }
        else if (coinClones.Count > moneyBalance)
        {
            while (coinClones.Count > moneyBalance)
            {
                Destroy(coinClones[coinClones.Count - 1]);
                coinClones.RemoveAt(coinClones.Count - 1);
            }
        }
        else if (coinClones.Count == moneyBalance)
        {
            //For now do nothing? I don't know if I need this. Might just be nice organization wise to leave this here.
        }
    }

    /* Reads through the lines array splitting each line at the comma and reading in the values
     * Starts by making a gameEvent with the first value from the split line as the eventName
     * then uses the next 6 values read in to construct an outcome object and add it to the
     * gameEvent's outcomes list
     * 
     * Splits up the events in the text file between phase1 and phase2 by checking whether
     * the line the loop is on reads "phase2" and if it does it switches over to inputting
     * the values into the eventDataPhase2 list instead of eventDataPhase1
     */
    void initializeEventData()
    {
     
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] != "phase2")
            {
                
                string[] words = lines[i].Split(',');

                gameEvent gameEvent = new gameEvent(words[0]);
                for (int k = 1; k < words.Length; k += 6)
                {
                    
                    outcome outcome = new outcome(int.Parse(words[k].Trim()), int.Parse(words[k + 1].Trim()), int.Parse(words[k + 2].Trim()), words[k + 3], bool.Parse(words[k + 4].Trim()), bool.Parse(words[k + 5].Trim()));
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
                    for (int l = 1; l < words.Length; l += 6)
                    {
                        outcome outcome = new outcome(int.Parse(words[l].Trim()), int.Parse(words[l + 1].Trim()), int.Parse(words[l + 2].Trim()), words[l + 3], bool.Parse(words[l + 4].Trim()), bool.Parse(words[l + 5].Trim()));
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

    /* Generates random numbers between 0 and the number of events in the event list
     * Then generates random numbers between 0 and the number of outcomes for each event
     * Finally sets the text of each option button to the eventName for each gameEvent 
     * and adds an onClick listener delegate for the method "buttonEvent" using the values from
     * the outcome
     * 
     * Fixes: Considering making an object method for outcome that just does the button event
     * using its member variables which would make sense honestly. Might need some finaggling.
     * 
     * Also need to find the best way to make sure the random numbers don't repeat, hoping to find
     * some kind of "sample" method that can find 3 random unique numbers from a range instead of just
     * generating 3 numbers and rerolling until they're all unique.
     */
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
                eventDataPhase1[randomVal1].getOutcome(randomOutcome1).getMoraleVal(), eventDataPhase1[randomVal1].getOutcome(randomOutcome1).getCashVal(), 
                eventDataPhase1[randomVal1].getOutcome(randomOutcome1).getEventText(),
                eventDataPhase1[randomVal1].getOutcome(randomOutcome1).getHasFriend()); });

            optionButton2Text.text = eventDataPhase1[randomVal2].getName();
            optionButton2.onClick.AddListener(delegate {
                buttonEvent(eventDataPhase1[randomVal2].getOutcome(randomOutcome2).getHealthVal(),
                eventDataPhase1[randomVal2].getOutcome(randomOutcome2).getMoraleVal(), eventDataPhase1[randomVal1].getOutcome(randomOutcome1).getCashVal(), 
                eventDataPhase1[randomVal2].getOutcome(randomOutcome2).getEventText(),
                eventDataPhase1[randomVal2].getOutcome(randomOutcome2).getHasFriend());
            });

            optionButton3Text.text = eventDataPhase1[randomVal3].getName();
            optionButton3.onClick.AddListener(delegate {
                buttonEvent(eventDataPhase1[randomVal3].getOutcome(randomOutcome3).getHealthVal(),
                eventDataPhase1[randomVal3].getOutcome(randomOutcome3).getMoraleVal(), eventDataPhase1[randomVal1].getOutcome(randomOutcome1).getCashVal(), 
                eventDataPhase1[randomVal3].getOutcome(randomOutcome3).getEventText(),
                eventDataPhase1[randomVal3].getOutcome(randomOutcome3).getHasFriend());
            });
        }
    }
    
    /* Updates sliders and money using the values passed to it and then toggles off the option buttons
     */
    void buttonEvent(int healthVal, int moraleVal, int moneyVal, string eventText, bool hasFriend)
    {
        Debug.Log("buttonEvent has been called with values: " + healthVal + ", " + moraleVal + ", " + moneyVal + ", " + eventText + ", " + hasFriend);


        UpdateSliders(healthVal, moraleVal);
        UpdateMoney(moneyVal);

        toggleOptionButtons(false);
    }

    /* Switches the option buttons from either active or inactive depending on the bool value passed
     */
    void toggleOptionButtons(bool state)
    {
        optionButton1.interactable = state;
        optionButton2.interactable = state;
        optionButton3.interactable = state;

        //figure out color button change

    }
}


/* Main meat and potatoes of the game -- holds an eventName string and a list of outcome objects
 * can add outcomes, getOutcome at a specific index, and return the number of outcomes it has
 */
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

/* Maybe the ACTUAL meat and potatoes of the game - each outcome has:
 * a healthVal (which is equivalent to "food")
 * a moraleVal
 * a cashVal
 * an eventText
 * a "hasFriend" bool (whether or not the event will grant the player a friend
 * an "isGain" bool (which determines whether or not all of the other member values are
 * gained or lost when the outcome occurs
 * 
 * This means that all events will have either only negative effects or only positive effects
 * 
 * Might want to find a more effective way to figure out how to allow outcomes to have both positive and 
 * negative effects, but given the file reading system I have going I can't read in negative numbers, so I can't
 * just have there be alternating values, and I don't want to just hard-code all the events because the flexibility
 * of the text reading is great. We'll see.
 */
public class outcome
{
    public int healthVal;
    public int moraleVal;
    public int cashVal;
    public string eventText;
    public bool hasFriend;
    public bool isGain;
    

    public outcome(int h, int m, int c, string e, bool hf, bool ig)
    {
        healthVal = h;
        moraleVal = m;
        cashVal = c;
        eventText = e;
        hasFriend = hf;
        isGain = ig;
    }

    public int getHealthVal()
    {
        if (isGain)
        {
            return healthVal;
        }
        else
        {
            return -healthVal;
        }
        
    }

    public int getMoraleVal()
    {
        if (isGain)
        {
            return moraleVal;
        }
        else
        {
            return -moraleVal;
        }
        
    }

    public int getCashVal()
    {
        if (isGain)
        {
            return cashVal;
        }
        else
        {
            return -cashVal;
        }
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
