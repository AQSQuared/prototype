using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour {

    public string levelName;
    public WinCondition winConditionMessage;
    public int levelNumber;

    private int requiredUnlockLevelNumber;

    public int[] goalsNumbers;
    
    [Header("Type of Level")]
    public bool isForwards;
    public bool isRedWordRound;
    public bool isMistakeMode;
    public string keysTypes;

    private GameObject starsParent;
    private GameObject infoObjectsParent;
    private Text levelNumberText;
    private Image lockImageGraphic;

    private Image[] stars;
    private Image[] infoObjects;

    private int starsEarned;
    private bool completeStatus;
    private float playerMaxScore;

    float oldEnergy;

    private void Start()
    {
        requiredUnlockLevelNumber = levelNumber - 1;

        lockImageGraphic = transform.GetChild(3).GetComponent<Image>();

        if (requiredUnlockLevelNumber <= PlayerPrefs.GetInt("LevelsCompleted"))
        {
            lockImageGraphic.gameObject.SetActive(false);
            this.GetComponent<Button>().interactable = true;

            FindObjectOfType<LevelSelector>().AddCompletedLevelToList(this);
        }

        starsParent = transform.Find("starsEarned").gameObject;
        infoObjectsParent = transform.Find("infoOnjs").gameObject;
        levelNumberText = transform.Find("levelNumber").GetComponent<Text>();

        stars = starsParent.GetComponentsInChildren<Image>();
        infoObjects = infoObjectsParent.GetComponentsInChildren<Image>();
        levelNumberText.text = levelNumber.ToString();

        SetOuterAppearance();
        SetOuterStars();


    }

    private void SetOuterAppearance ()
    {
        if (isForwards)
        {
            infoObjects[0].color = new Color(56 / 255f, 206 / 255f, 1f);
        }
        if (!isForwards)
        {
            infoObjects[0].color = new Color(144f / 255f, 81f / 255f, 1f);
        }

        if (isRedWordRound)
        {
            infoObjects[1].color = new Color(1f, 88f / 255f, 88f / 255f);
        }
        else if (!isRedWordRound)
        {
            infoObjects[1].gameObject.SetActive(false);
        }

        if (isMistakeMode)
        {
            infoObjects[2].color = new Color(1f, 164f / 255f, 81f / 255f);
        }
        else if (!isMistakeMode)
        {
            infoObjects[2].gameObject.SetActive(false);
        }
    }

    private void SetOuterStars()
    {
        starsEarned = PlayerPrefs.GetInt(levelNumber.ToString());
        
        if (starsEarned >= 3)
        {
            stars[2].color = new Color(1f, 203f / 255f, 124f / 255f);
        }
        if (starsEarned >= 2)
        {
            stars[1].color = new Color(1f, 203f / 255f, 124f / 255f);
        }
        if (starsEarned >= 1)
        {
            stars[0].color = new Color(1f, 203f / 255f, 124f / 255f);
        }

        if (starsEarned <= 0)
        {
            stars[0].color = Color.white;
            stars[1].color = Color.white;
            stars[2].color = Color.white;
        }
    }

    public void LoadLevel()
    {
        string completionIndicator = PlayerPrefs.GetString(levelNumber.ToString() + "s");
        if (completionIndicator == levelNumber.ToString() + "COMPLETED")
        {
            completeStatus = true;
        }
        else if (completionIndicator != levelNumber.ToString() + "COMPLETED")
        {
            completeStatus = false;
        }

        playerMaxScore = PlayerPrefs.GetFloat(levelNumber.ToString() + "f");

        GameObject.Find("LevelSelect").GetComponent<Animator>().SetTrigger(Animator.StringToHash("Bring"));
        FindObjectOfType<LevelSelector>().BeginLevel(levelNumber, winConditionMessage, Mathf.FloorToInt(playerMaxScore), isForwards, isRedWordRound, isMistakeMode, keysTypes, completeStatus, goalsNumbers, starsEarned, levelName);

        if (levelNumber >= PlayerPrefs.GetInt("LevelsCompleted"))
        {
            PlayerPrefs.SetFloat("SliderValue", GameObject.Find("Scrollbar Vertical").GetComponent<Scrollbar>().value);
        }
    }

    public void LoseCount ()
    {

    }
}
