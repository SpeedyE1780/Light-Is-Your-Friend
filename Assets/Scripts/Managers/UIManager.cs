using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Menues UI")]
    public GameObject MainMenuUI;
    public GameObject WelcomeScreenUI;
    public GameObject TutorialUI;
    public GameObject CreditsUI;
    public GameObject OptionsUI;
    public GameObject GameUI;
    public GameObject GameOverUI;

    [Header("Dialogues")]
    public Transform StartDialogue;
    public Transform MidDialogue;
    public Transform EndDialogue;

    [Header("Game UI")]
    public Image WeaponImage;
    public Transform HealthBar;
    public Image DeodorantImage;
    public Image BatteryBar;
    public Sprite FilledBattery;
    public Sprite EmptyBattery;
    public GameObject InteractionParent;
    public Text InteractionText;

    //readonly string showDialogueTrigger = "ShowDialogue";
    readonly string hideDialogueTrigger = "HideDialogue";

    private static UIManager _instance;
    public static UIManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        //Make sure health bar is full
        foreach (Transform child in HealthBar)
            child.gameObject.SetActive(true);

        //Make sure battery is full
        foreach(Transform child in BatteryBar.transform)
        {
            child.gameObject.SetActive(true);
            child.GetComponent<Image>().color = Color.yellow;
        }

        //Hide the interaction text
        InteractionParent.SetActive(false);

        //Start the game on the correct screen
        MainMenuUI.SetActive(true);
        WelcomeScreenUI.SetActive(true);
        TutorialUI.SetActive(false);
        OptionsUI.SetActive(false);
        CreditsUI.SetActive(false);
        GameUI.SetActive(false);
        GameOverUI.SetActive(false);

        //Hide deodorant image
        DeodorantImage.fillAmount = 0;

        EventManager.setDeodorant += SetDeodorant;
        EventManager.firstZombie += FirstZombieDialogue;
    }

    //Show dialogue at the beginning of the game
    public void BeginDialogue()
    {
        StartCoroutine(nameof(DialogueCoroutine), StartDialogue);
    }

    //Show the first zombie dialogue
    void FirstZombieDialogue()
    {
        StartCoroutine(nameof(DialogueCoroutine), MidDialogue);
    }

    //Show the end game dialogue
    public void EndGameDialogue()
    {
        StartCoroutine(nameof(DialogueCoroutine), EndDialogue);
    }

    IEnumerator DialogueCoroutine(Transform parent)
    {
        //End game dialogue wait 1 second
        if (parent == EndDialogue)
        {
            yield return new WaitForSecondsRealtime(1f);
        }

        //Pause the time
        Time.timeScale = 0;

        //Loop through all the dialogue bubbles
        foreach(Transform child in parent)
        {
            //Set dialogue active
            child.gameObject.SetActive(true);

            //Wait for animation to end
            yield return new WaitForSecondsRealtime(0.25f);

            //Wait until user press space to show next bubble
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            yield return new WaitForEndOfFrame();

            //Hide the bubble
            child.GetComponent<Animator>().SetTrigger(hideDialogueTrigger);
            yield return new WaitForSecondsRealtime(0.25f);

            //Turn off the gameobject
            child.gameObject.SetActive(false);
        }

        //Reset the timescale
        Time.timeScale = 1;

        //If game didn't end reset the player movement else show gameover screen
        if (parent != EndDialogue)
        {
            EventManager.toggleCharacters.Invoke(1);
        }
        else
        {
            GameOver();
        }
    }

    //Hide all screen and show gameover
    public void GameOver()
    {
        MainMenuUI.SetActive(false);
        GameUI.SetActive(false);
        GameOverUI.SetActive(true);
    }

    //Set the current weapon image
    public void SetWeaponImage(Sprite weapon)
    {
        WeaponImage.gameObject.SetActive(true);
        WeaponImage.sprite = weapon;
        WeaponImage.SetNativeSize();
    }

    //Show the interaction text
    public void SetInteractionText(string text)
    {
        InteractionParent.SetActive(true);
        InteractionText.text = text;
    }

    public void HideInteractionText()
    {
        InteractionParent.SetActive(false);
    }

    public void UpdateHealthBar(int tier)
    {
        //Turn off all bars
        foreach (Transform child in HealthBar)
            child.gameObject.SetActive(false);

        //Turn bars based on the tier we're in
        for (int i = 0; i < tier; i++)
        {
            HealthBar.GetChild(i).gameObject.SetActive(true);
        }

        Image firstBar = HealthBar.GetChild(0).GetComponent<Image>();

        //If only one bar left set its color to red else white
        if (tier < 2)
        {
            firstBar.color = Color.red;
        }
        else
        {
            firstBar.color = Color.white;
        }
    }

    public void UpdateBatteryBar(int tier)
    {
        //Turn off all bars
        foreach (Transform child in BatteryBar.transform)
            child.gameObject.SetActive(false);

        //Turn bars based on the tier we're in
        for (int i = 0; i < tier; i++)
        {
            BatteryBar.transform.GetChild(i).gameObject.SetActive(true);
        }

        Image firstBar = BatteryBar.transform.GetChild(0).GetComponent<Image>();
        
        //Battery is empty swap its sprite
        if (tier <= 0)
        {
            BatteryBar.sprite = EmptyBattery;
        }

        //If only one bar left set its color to red else yellow
        else if (tier < 2)
        {
            firstBar.color = Color.red;
            BatteryBar.sprite = FilledBattery;
        }
        else
        {
            firstBar.color = Color.yellow;
            BatteryBar.sprite = FilledBattery;
        }
    }

    //Show deodorant image and start updating it
    public void SetDeodorant(float duration)
    {
        DeodorantImage.fillAmount = 1;
        StartCoroutine(nameof(UpdateDeodorant), duration);
    }

    IEnumerator UpdateDeodorant(float duration)
    {
        //Get the decay/second
        float decay = 1 / duration;

        while (DeodorantImage.fillAmount > 0)
        {
            DeodorantImage.fillAmount -= decay * Time.deltaTime;
            yield return null;
        }
    }
}