using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelConditionSetter : MonoBehaviour {

    public Image fW;
    public Image rW;
    public Image mM;
    public Text levelNameText;

    public Color[] colorsToFill;
    public Color[] differentColorsForLevelName;
    public string[] descriptions;

    private Text fwbwTitleText;
    private Text rwTitleText;
    private Text mmTitleText;
    private Text fwbwDesText;
    private Text rwDesText;
    private Text mmDesText;

    private bool isForwardForLevel;
    private bool isRedwordForLevel;
    private bool isMistakeForLevel;
    private string levelName;
    private string levelNumber;

    private WinCondition winCondition;
    bool fw; bool rw; bool mm;

    private void Awake()
    {
        fwbwTitleText = fW.transform.GetChild(0).GetComponent<Text>();
        rwTitleText = rW.transform.GetChild(0).GetComponent<Text>();
        mmTitleText = mM.transform.GetChild(0).GetComponent<Text>();

        fwbwDesText = fwbwTitleText.transform.GetChild(0).GetComponent<Text>();
        rwDesText = rwTitleText.transform.GetChild(0).GetComponent<Text>();
        mmDesText = mmTitleText.transform.GetChild(0).GetComponent<Text>();
    }

    private void Start()
    {
        SetLevelConditions();

        //levelNameText.color = differentColorsForLevelName[Random.Range(0, differentColorsForLevelName.Length)];
        levelNameText.color = Color.black;
        levelNameText.text = "LEVEL " + levelNumber.ToString() + " INSTRUCTIONS";

        if (isForwardForLevel)
        {
            fW.color = colorsToFill[0];
            fwbwTitleText.text = "FORWARD";
            fwbwDesText.text = descriptions[0];
        }
        if (!isForwardForLevel)
        {
            fW.color = colorsToFill[1];
            fwbwTitleText.text = "BACKWARD";
            fwbwDesText.text = descriptions[1];
        }

        if (isRedwordForLevel && winCondition == WinCondition.NUMBER_OF_WORDS)
        {
            rW.gameObject.SetActive(true);
            rwTitleText.text = "RED WORDS (WORDS)";
            rwDesText.text = descriptions[2];
        }
        else if (isRedwordForLevel && winCondition == WinCondition.SCORE)
        {
            rW.gameObject.SetActive(true);
            rwTitleText.text = "RED WORDS (SCORE)";
            rwDesText.text = descriptions[3];
        }
        else if (!isRedwordForLevel)
        {
            rW.gameObject.SetActive(false);
        }

        if (isMistakeForLevel && winCondition == WinCondition.NUMBER_OF_WORDS)
        {
            mM.gameObject.SetActive(true);
            mmTitleText.text = "MISTAKE (WORDS)";
            mmDesText.text = descriptions[4];
        }
        else if (isMistakeForLevel && winCondition == WinCondition.SCORE)
        {
            mM.gameObject.SetActive(true);
            mmTitleText.text = "MISTAKE (SCORE)";
            mmDesText.text = descriptions[5];
        }
        else if (!isMistakeForLevel)
        {
            mM.gameObject.SetActive(false);
        }
    }

    private void SetLevelConditions ()
    {
        if (PlayerPrefs.GetInt("FWFC") == 1)
        {
            fw = true;
        }
        if (PlayerPrefs.GetInt("FWFC") == 0)
        {
            fw = false;
        }
        if (PlayerPrefs.GetInt("RWFC") == 1)
        {
            rw = true;
        }
        if (PlayerPrefs.GetInt("RWFC") == 0)
        {
            rw = false;
        }
        if (PlayerPrefs.GetInt("MMFC") == 1)
        {
            mm = true;
        }
        if (PlayerPrefs.GetInt("MMFC") == 0)
        {
            mm = false;
        }

        if (PlayerPrefs.GetString("LevelWinFC") == "WORDS")
        {
            winCondition = WinCondition.NUMBER_OF_WORDS;
        }
        if (PlayerPrefs.GetString("LevelWinFC") == "SCORE")
        {
            winCondition = WinCondition.SCORE;
        }

        isForwardForLevel = fw;
        isRedwordForLevel = rw;
        isMistakeForLevel = mm;

        levelName = PlayerPrefs.GetString("LevelNameFC");
        levelNumber = PlayerPrefs.GetString("LevelNumberFC");
    }

    public void BackButton ()
    {
        SceneManager.LoadScene("LevelSelector");
    }

    public void NextButton ()
    {
        SceneManager.LoadScene(levelName);
    }
}
