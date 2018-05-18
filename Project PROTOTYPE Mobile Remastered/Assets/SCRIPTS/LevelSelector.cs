using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour {
    
    //Fix Minutes.

    public float lifeRefillTimer = 0;
    public Text timeText;

    #region RestCode
    public int lives;
    public int numberOfLives;
    public float livesFillSpeed = 0.3f;
    public float colorChangeSpeedX = 0.05f;

    public GameObject[] thingsToDisableOnBeginning;

    public AnimationClip backAnimation;
    public Gradient energyGradient;

    public Text levelNumberText;
    public Text levelTypeText;
    public Text maxScore;
    public Image forwardSlashBar;
    public Image redWordBar;
    public Image mistakeBar;
    public Text keyboardTypeText;
    public Text statusText;
    public Image livesImage;

    public Text[] scoreBars;
    public GameObject[] stars;

    [HideInInspector]
    public float highestLevel;

    private Scrollbar verticalSlider;
    private Text livesText;
    private Image livesImg;

    private float lerper;
    private string currentSelectedLevel;
    [Range(0, 180)]
    private float currentLives;
    private float evaluationTimeChange;

    private List<Level> levels = new List<Level>();
    #endregion

    DateTime currentDate;
    DateTime oldDate;

    public string saveLocation;
    public static LevelSelector instance;
    public static int levelsPlayed;
    public static float oldLife;

    float oneMinute;
    float oneSecond;

    private void Awake()
    {
        instance = this;
        saveLocation = "lastSavedDate1";
    }

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        if(PlayerPrefs.GetString("SaveOrNot") == "true")
        {
            SaveDate();
            lifeRefillTimer = oldLife;
            PlayerPrefs.SetInt("Energy", Mathf.FloorToInt(lifeRefillTimer));
        }

        //lifeRefillTimer = lives;
        //lifeRefillTimer -= (lives / numberOfLives) * levelsPlayed;
        lifeRefillTimer = (PlayerPrefs.GetInt("Energy"));
        lifeRefillTimer += CheckDate();

        verticalSlider = GameObject.Find("Scrollbar Vertical").GetComponent<Scrollbar>();
        verticalSlider.value = PlayerPrefs.GetFloat("SliderValue");

        livesText = livesImage.transform.GetChild(0).GetComponent<Text>();
        livesImg = livesImage.transform.GetChild(1).GetComponent<Image>();

        //currentLives = PlayerPrefs.GetInt("Energy");
        livesText.text = (Mathf.FloorToInt(currentLives)).ToString() + "/" + lives.ToString();

        highestLevel = PlayerPrefs.GetInt("LevelsCompleted");

        if (highestLevel == 0)
        {
            StartCoroutine(LerpToZero());
        }

        PlayerPrefs.SetString("SaveOrNot", "false");

        oneSecond = (lives - Mathf.FloorToInt(lifeRefillTimer)) % 60;
        oneMinute = (lives / numberOfLives) / 60;
    }

    private void FixedUpdate()
    {

        if (currentLives < lives)
        {
            lifeRefillTimer += Time.deltaTime;

            if (lifeRefillTimer >= lives)
            {
                //SaveDate();
                //lifeRefillTimer = 0;
                //lifeRefillTimer += CheckDate();
            }

        }

        if (lifeRefillTimer > lives)
        {
            lifeRefillTimer = lives;
            currentLives = lives;
        }

        currentLives = lifeRefillTimer;

        SetTimeText();

        livesText.text = (Mathf.FloorToInt(currentLives) / (lives / numberOfLives)).ToString("f0") + "/" + (lives / (lives / numberOfLives)).ToString("f0");
        evaluationTimeChange += colorChangeSpeedX * Time.deltaTime;
        if (evaluationTimeChange >= 1)
        {
            evaluationTimeChange = 0.0f;
        }
        livesImg.color = energyGradient.Evaluate(evaluationTimeChange);
        livesImg.fillAmount = currentLives / lives;
    }

    public void BeginLevel (int levelNumber, WinCondition type, int score, bool forw, bool redw, bool misw, string keys, bool status, int[] goals, int numberOfStarsEarned, string levelName)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < thingsToDisableOnBeginning.Length; i++)
        {
            thingsToDisableOnBeginning[i].SetActive(false);
        }

        currentSelectedLevel = levelName;
        levelNumberText.text = levelNumber.ToString();
        if(type == WinCondition.NUMBER_OF_WORDS)
        {
            levelTypeText.text = "WORDS";
        }
        if (type == WinCondition.SCORE)
        {
            levelTypeText.text = "SCORE";
        }
        maxScore.text = score.ToString();
        if (forw)
        {
            forwardSlashBar.color = new Color(56 / 255f, 206 / 255f, 1f);
            forwardSlashBar.transform.GetChild(0).GetComponent<Text>().text = "FORWARD";
        }
        else if (!forw)
        {
            forwardSlashBar.color = new Color(144f / 255f, 81f / 255f, 1f);
            forwardSlashBar.transform.GetChild(0).GetComponent<Text>().text = "BACKWARD";
        }

        if (redw)
        {
            redWordBar.color = new Color(1f, 88f / 255f, 88f / 255f);
            redWordBar.transform.GetChild(0).GetComponent<Text>().text = "RED WORD";
        }
        else if (!redw)
        {
            redWordBar.gameObject.SetActive(false);
        }

        if (misw)
        {
            mistakeBar.color = new Color(1f, 164f / 255f, 81f / 255f);
            mistakeBar.transform.GetChild(0).GetComponent<Text>().text = "MISTAKE";
        }
        else if (!misw)
        {
            mistakeBar.gameObject.SetActive(false);
        }

        keyboardTypeText.text = keys;

        if (status == true)
            statusText.text = "COMPLETED";
        if (status == false)
            statusText.text = "NOT COMPLETED";

        for(int i = 0; i < goals.Length; i++)
        {
            scoreBars[i].text = goals[i].ToString();
        }
        
        for(int i = 0; i < numberOfStarsEarned; i++)
        {
            stars[i].gameObject.SetActive(true);
        }
    }

    public void BackButton()
    {
        GameObject.Find("LevelSelect").GetComponent<Animator>().SetTrigger(Animator.StringToHash("Debring"));

        currentSelectedLevel = "";

        for (int i = 0; i < thingsToDisableOnBeginning.Length; i++)
        {
            thingsToDisableOnBeginning[i].SetActive(true);
        }
    }

    public void PlayButton()
    {
        PlayerPrefs.SetString("LevelNameFC", currentSelectedLevel);
        PlayerPrefs.SetString("LevelNumberFC", levelNumberText.text);
        PlayerPrefs.SetString("LevelWinFC", levelTypeText.text);

        if(forwardSlashBar.transform.GetChild(0).GetComponent<Text>().text == "FORWARD")
        {
            PlayerPrefs.SetInt("FWFC", 1);
        }
        if (forwardSlashBar.transform.GetChild(0).GetComponent<Text>().text == "BACKWARD")
        {
            PlayerPrefs.SetInt("FWFC", 0);
        }

        if (redWordBar.gameObject.activeSelf == true)
        {
            PlayerPrefs.SetInt("RWFC", 1);
        }
        if (redWordBar.gameObject.activeSelf == false)
        {
            PlayerPrefs.SetInt("RWFC", 0);
        }

        if (mistakeBar.gameObject.activeSelf == true)
        {
            PlayerPrefs.SetInt("MMFC", 1);
        }
        if (mistakeBar.gameObject.activeSelf == false)
        {
            PlayerPrefs.SetInt("MMFC", 0);
        }

        if(currentLives >= (lives/numberOfLives))
        {
            oldLife = currentLives;
            oldLife -= lives / numberOfLives;
            PlayerPrefs.SetString("SaveOrNot", "true");
            //SaveDate();
            //lifeRefillTimer = oldLife - (lives / numberOfLives);
            //lifeRefillTimer += CheckDate();
            SceneManager.LoadScene("LevelConditionary");
        }
    }

    private IEnumerator LerpToZero ()
    {
        float timeRemaining = 1f;
        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            lerper = Mathf.Lerp(0f, 1f, Mathf.InverseLerp(1f, 0, timeRemaining));
            PlayerPrefs.SetFloat("SliderValue", lerper);
            verticalSlider.value = PlayerPrefs.GetFloat("SliderValue");
            yield return null;
        }
        lerper = 1f;
    }

    public void AddCompletedLevelToList(Level level)
    {
        levels.Add(level);
    }

    public float CheckDate ()
    {
        currentDate = System.DateTime.Now;

        string tempString = PlayerPrefs.GetString(saveLocation, "1");
        long tempLong = Convert.ToInt64(tempString);
        DateTime oldDate = DateTime.FromBinary(tempLong);

        print("Old Date : " + oldDate);

        TimeSpan difference = currentDate.Subtract(oldDate);
        print("Difference : " + difference);

        return (float)difference.TotalSeconds;
    }

    public void SaveDate ()
    {
        PlayerPrefs.SetString(saveLocation, System.DateTime.Now.ToBinary().ToString());
        print("Saving : " + System.DateTime.Now);
    }

    private void SetTimeText ()
    {
        oneSecond -= Time.deltaTime;
        if(oneSecond <= 0)
        {
            oneMinute -= 1;
            oneSecond = 60f;
        }
        if(oneMinute <= 0)
        {
            oneMinute = (lives / numberOfLives) / 60;
        }
        timeText.text = oneMinute.ToString("f0") + ":" + oneSecond.ToString("f0");
    }

}
