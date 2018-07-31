using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelConditionSetter : MonoBehaviour {

    public Gradient alphaGR;
    public GameObject[] thingsToDisableWhenEndless;
    public GameObject endlessUI;
    public GameObject exitButtonCon;

    public Image fW;
    public Image rW;
    public Image mM;
    public Text levelNameText;
    public Text loadingText;
    public Button playButton;

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
    private Animator anim;
    bool fw; bool rw; bool mm;
    bool isEndless;

    public float loadingTime = 4f;
    float evaluationTimeChange = 0.0f;

    private void Awake()
    {
        if(PlayerPrefs.GetInt("EndlessIndic") == 0)
        {
            isEndless = false;
        }
        if(PlayerPrefs.GetInt("EndlessIndic") == 1)
        {
            isEndless = true;
        }

        if (!isEndless)
        {
            endlessUI.gameObject.SetActive(false);
            exitButtonCon.gameObject.SetActive(false);

            fwbwTitleText = fW.transform.GetChild(0).GetComponent<Text>();
            rwTitleText = rW.transform.GetChild(0).GetComponent<Text>();
            mmTitleText = mM.transform.GetChild(0).GetComponent<Text>();

            fwbwDesText = fwbwTitleText.transform.GetChild(0).GetComponent<Text>();
            rwDesText = rwTitleText.transform.GetChild(0).GetComponent<Text>();
            mmDesText = mmTitleText.transform.GetChild(0).GetComponent<Text>();
        }
        if (isEndless)
        {
            foreach(GameObject go in thingsToDisableWhenEndless)
            {
                go.SetActive(false);
            }

            endlessUI.gameObject.SetActive(true);
            exitButtonCon.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        anim = FindObjectOfType<Animator>();
        if (FindObjectOfType<AudioController>() != null)
            FindObjectOfType<AudioController>().GetComponent<AudioSource>().volume = 0.12f;

        if (!isEndless)
        {
            playButton.gameObject.SetActive(false);
            loadingText.gameObject.SetActive(true);

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
    }

    private void Update()
    {
        if (!isEndless)
        {
            loadingTime -= Time.deltaTime;
            if (loadingTime <= 0)
            {
                playButton.gameObject.SetActive(true);
                loadingText.gameObject.SetActive(false);
            }

            evaluationTimeChange += 0.5f * Time.deltaTime;
            if (evaluationTimeChange >= 1)
            {
                evaluationTimeChange = 0.0f;
            }
            loadingText.color = alphaGR.Evaluate(evaluationTimeChange);
        }
    }

    private void SetLevelConditions ()
    {
        if (!isEndless)
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
    }

    public void BackButton ()
    {
        SceneManager.LoadScene("LevelSelector");
    }

    public void NextButton ()
    {
        SceneManager.LoadScene(levelName);
    }

    public void ExitButtonForEndlessConditionary()
    {
        SceneManager.LoadScene("LevelMain");
    }

    public void EndlessPlayButton()
    {
        SceneManager.LoadScene("LevelEndless");
    }
    public void LeaderboardsButton()
    {
        SceneManager.LoadScene("Leaderboards");
    }
    public void PremiumButton()
    {
        anim.SetBool(Animator.StringToHash("ComingSoonAnim"), true);
        endlessUI.SetActive(false);
        exitButtonCon.gameObject.SetActive(false);
        //coming soon animation.
    }

    public void ComingSoonConButtonTrig()
    {
        anim.SetBool(Animator.StringToHash("ComingSoonAnim"), false);
        endlessUI.SetActive(true);
        exitButtonCon.gameObject.SetActive(true);
        //coming soon animation.
    }
}
