using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int levelNumberToDisplay;

    public Gradient cameraGradient;

    public int scoreToIncreasePerWord;
    public float difficulty = 0.75f;
    public float dampTime = 0.3f;
    public float colorChangeSpeed = 0.01f;
    public bool canChangeColor;

    static float evaluationTimeChange = 0.0f;

    [HideInInspector]
    public int score;
    [HideInInspector]
    public int numberOfWordsTypedInThisGame;

    public int[] difficultyBoard;
    public int[] winStars;
    public GameObject[] stars;
    public Text[] goalScores;
    public List<RectTransform> starBarsRectWidths = new List<RectTransform>();

    [HideInInspector]
    [Range(0, 3)]
    public int starsToGive;

    public WinCondition winCondition;

    [Header("Customs")]
    public Slider[] starBars;
    public GameObject qwertyBoard;
    public GameObject azertyBoard;
    public GameObject regularBoard;
    public GameObject numberBoard;
    public GameObject ins01P;
    public GameObject ins02P;
    public GameObject ins03P;
    public RectTransform starBarect;
    public Text levelScoreText;
    public Button exitButton;

    [SerializeField]
    private GameObject levelCounter;

    private Prototype prototype;
    private WordSpawner spawner;
    private LevelCompletor levelCompletor;
    private Slider curSlider;
    private Image levelCounterColor;
    private Text levelNumberTextForCounter;

    private Color[] colorOfStars = new Color[5];

    private float maxScore;
    private Color defaultColorOfLevelCounter;

    bool levelLost;
    float l;
    float jklm;

    // Use this for initialization
    void Start () {
        prototype = GetComponent<Prototype>();
        spawner = GetComponent<WordSpawner>();
        levelCompletor = GetComponent<LevelCompletor>();
        levelCounterColor = levelCounter.transform.GetChild(0).GetComponent<Image>();
        if (!prototype.isEndless)
        {
            levelNumberTextForCounter = levelCounter.transform.GetChild(1).GetComponent<Text>();
            defaultColorOfLevelCounter = levelCounterColor.color;
            levelNumberTextForCounter.text = levelNumberToDisplay.ToString();
        }

        if (winCondition == WinCondition.NUMBER_OF_WORDS)
        {
            maxScore = spawner.numberOfWordsToSpawn;
            levelScoreText.text = "WORDS";
            levelScoreText.transform.GetChild(0).GetComponent<Text>().text = numberOfWordsTypedInThisGame.ToString();
        }
        if(winCondition == WinCondition.SCORE)
        {
            maxScore = spawner.numberOfWordsToSpawn * scoreToIncreasePerWord;
            levelScoreText.text = "SCORE";
            levelScoreText.transform.GetChild(0).GetComponent<Text>().text = score.ToString();
        }
        SetBars();

        for(int i = 0; i < starBars.Length; i++)
        {
            starBarsRectWidths.Add(starBars[i].GetComponent<RectTransform>());
        }
        SetBarsWidth();

        colorOfStars[0] = stars[0].transform.GetChild(0).GetComponent<Image>().color;
        colorOfStars[1] = stars[1].transform.GetChild(0).GetComponent<Image>().color;
        colorOfStars[2] = stars[1].transform.GetChild(1).GetComponent<Image>().color;
        colorOfStars[3] = stars[2].transform.GetChild(0).GetComponent<Image>().color;
        colorOfStars[4] = stars[2].transform.GetChild(2).GetComponent<Image>().color;

        stars[0].transform.GetChild(0).GetComponent<Image>().color = Color.white;
        stars[1].transform.GetChild(0).GetComponent<Image>().color = Color.white;
        stars[1].transform.GetChild(1).GetComponent<Image>().color = Color.white;
        stars[2].transform.GetChild(0).GetComponent<Image>().color = Color.white;
        stars[2].transform.GetChild(1).GetComponent<Image>().color = Color.white;
        stars[2].transform.GetChild(2).GetComponent<Image>().color = Color.white;

        for(int i = 0; i < goalScores.Length; i++)
        {
            goalScores[i].text = winStars[i].ToString();
        }

    }

    private void Update()
    {
        if (canChangeColor && !prototype.isEndless)
        {
            evaluationTimeChange += colorChangeSpeed * Time.deltaTime;
            if (evaluationTimeChange >= 1)
            {
                evaluationTimeChange = 0.0f;
            }
            for (int i = 0; i < starBars.Length; i++)
            {
                starBars[i].transform.GetChild(0).GetComponent<Outline>().effectColor = cameraGradient.Evaluate(evaluationTimeChange);
            }
            levelCounterColor.color = cameraGradient.Evaluate(evaluationTimeChange);

        }

        else if (!canChangeColor)
        {
            for (int i = 0; i < starBars.Length; i++)
            {
                starBars[i].transform.GetChild(0).GetComponent<Outline>().effectColor = Color.white;
            }

        }

        if(score <= 0 && winCondition == WinCondition.SCORE)
        {
            score = 0;
            levelScoreText.transform.GetChild(0).GetComponent<Text>().text = score.ToString();
        }
                
    }

    public void IncreaseNumberOfWordsTyped ()
    {
        numberOfWordsTypedInThisGame += 1;
        if(winCondition == WinCondition.NUMBER_OF_WORDS)
        {
            levelScoreText.transform.GetChild(0).GetComponent<Text>().text = numberOfWordsTypedInThisGame.ToString();
        }
        if (winCondition == WinCondition.SCORE)
        {
            IncreaseScore(scoreToIncreasePerWord);
        }

        SetScoreForLevel(winCondition);

        prototype.RefreshLetters();
     
    }

    public void SetKeyboard (bool isQwertyBoard, bool isAzertyBoard, bool isNumberBoard, bool ins01, bool ins02, bool ins03, string roundType)
    {
        if (isQwertyBoard && !isAzertyBoard)
        {
            qwertyBoard.SetActive(true);
            azertyBoard.SetActive(false);
            regularBoard.SetActive(false);
            numberBoard.SetActive(false);
        }
        else if (isAzertyBoard && !isQwertyBoard)
        {
            azertyBoard.SetActive(true);
            qwertyBoard.SetActive(false);
            numberBoard.SetActive(false);
            regularBoard.SetActive(false);
        }
        else if(!isQwertyBoard && !isAzertyBoard)
        {
            qwertyBoard.SetActive(false);
            azertyBoard.SetActive(false);
            regularBoard.SetActive(true);
            numberBoard.SetActive(false);
        }

        if (isNumberBoard)
        {
            qwertyBoard.SetActive(false);
            azertyBoard.SetActive(false);
            regularBoard.SetActive(false);
            numberBoard.SetActive(true);
        }

        if (!ins01)
        {
            ins01P.SetActive(false);
        }
        if (!ins02)
        {
            ins02P.SetActive(false);
        }
        if (!ins03)
        {
            ins03P.SetActive(false);
        }

        if (ins01 && roundType == "forward")
        {
            ins01P.GetComponent<Image>().color = new Color(56 / 255f, 206 / 255f, 1f);
            ins01P.GetComponentInChildren<Text>().text = "FORWARD";
        }
        if (ins01 && roundType == "backward")
        {
            ins01P.GetComponent<Image>().color = new Color(144f / 255f, 81f / 255f, 1f);
            ins01P.GetComponentInChildren<Text>().text = "BACKWARD";
        }

        if (ins02)
        {
            ins02P.GetComponentInChildren<Text>().text = "NUMBERS";
        }
        if (ins03)
        {
            ins03P.GetComponentInChildren<Text>().text = "MISTAKE";
        }
    }

    public void VirtualKeyboardKeyController (Button button)
    {
        string characterTyped = button.gameObject.GetComponentInChildren<Text>().text;

        prototype.SetCharacter(characterTyped);
    }

    private void SetScoreForLevel (WinCondition _winCondition)
    {
        for (int i = 0; i < difficultyBoard.Length; i++)
        {
            if (numberOfWordsTypedInThisGame == difficultyBoard[i])
            {
                spawner.IncreaseDifficulty(difficulty);
            }
        }

        if (_winCondition == WinCondition.NUMBER_OF_WORDS)
        {
            SetStarsToGive(numberOfWordsTypedInThisGame);

            ExpressStarRate(numberOfWordsTypedInThisGame);
        }
        if (_winCondition == WinCondition.SCORE)
        {
            SetStarsToGive(score);

            ExpressStarRate(score);
        }

    }

    private void SetStarsToGive (int incresion)
    {
        for(int i = 0; i < starBars.Length; i++)
        {
            if (incresion >= winStars[0])
            {
                starsToGive = 1;
                stars[0].transform.GetChild(0).GetComponent<Image>().color = colorOfStars[0];
                stars[1].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                stars[1].transform.GetChild(1).GetComponent<Image>().color = Color.white;
                canChangeColor = false;
                //starBars[i].fillRect.GetComponent<Image>().color = new Color(43f / 255f, 1f, 194f / 255f);
            }
            if (incresion >= winStars[1])
            {
                starsToGive = 2;
                stars[1].transform.GetChild(0).GetComponent<Image>().color = colorOfStars[1];
                stars[1].transform.GetChild(1).GetComponent<Image>().color = colorOfStars[2];
                stars[2].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                stars[2].transform.GetChild(1).GetComponent<Image>().color = Color.white;
                stars[2].transform.GetChild(2).GetComponent<Image>().color = Color.white;
                canChangeColor = false;
                //starBars[i].fillRect.GetComponent<Image>().color = new Color(114f / 255f, 0, 1f);
            }
            if (incresion >= winStars[2])
            {
                starsToGive = 3;
                stars[2].transform.GetChild(0).GetComponent<Image>().color = colorOfStars[3];
                stars[2].transform.GetChild(1).GetComponent<Image>().color = colorOfStars[3];
                stars[2].transform.GetChild(2).GetComponent<Image>().color = colorOfStars[4];
                canChangeColor = true;
                //starBars[i].fillRect.GetComponent<Image>().color = new Color(1f, 0f, 111f / 255f);
            }
            if (incresion < winStars[0])
            {
                starsToGive = 0;
                stars[0].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                stars[1].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                stars[1].transform.GetChild(1).GetComponent<Image>().color = Color.white;
                stars[2].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                stars[2].transform.GetChild(1).GetComponent<Image>().color = Color.white;
                stars[2].transform.GetChild(2).GetComponent<Image>().color = Color.white;
                canChangeColor = false;
                //starBars[i].fillRect.GetComponent<Image>().color = new Color(188f / 255f, 1f, 0f);
            }
        }
    }

    public void IncreaseScore (int scoreToIncrease)
    {
        if(score < maxScore)
        {
            score += scoreToIncrease;
            //levelScoreText.transform.GetChild(0).GetComponent<Text>().text = score.ToString();
            StartCoroutine(ScoreIncreasingEffect(score - scoreToIncrease, score));
        }
    }

    public void DecreaseScore (int scoreToDecrease)
    {
        if(score > 0)
        {
            score -= scoreToDecrease;
            SetScoreForLevel(winCondition);
            levelScoreText.transform.GetChild(0).GetComponent<Text>().text = score.ToString();
        }
    }

    public void CompleteLevel ()
    {
        exitButton.gameObject.SetActive(false);

        int earnedStars = starsToGive;
        float incersion = 0;

        if(winCondition == WinCondition.NUMBER_OF_WORDS)
        {
            incersion = numberOfWordsTypedInThisGame;
        }
        if(winCondition == WinCondition.SCORE)
        {
            incersion = score;
        }

        levelCompletor.OnLevelCompletion(winCondition, incersion, earnedStars, levelNumberToDisplay);

        if(earnedStars >= PlayerPrefs.GetInt(levelNumberToDisplay.ToString()))
        {
            PlayerPrefs.SetInt(levelNumberToDisplay.ToString(), earnedStars);
        }
        if(incersion >= PlayerPrefs.GetFloat(levelNumberToDisplay.ToString() + "f"))
        {
            PlayerPrefs.SetFloat(levelNumberToDisplay.ToString() + "f", incersion);
        }
        if (!levelLost)
        {
            PlayerPrefs.SetString(levelNumberToDisplay.ToString() + "s", levelNumberToDisplay.ToString() + "COMPLETED");
        }
    }

    private void SetBars ()
    {
        starBars[3].maxValue = maxScore - winStars[2];
        starBars[2].maxValue = winStars[2] - winStars[1];
        starBars[1].maxValue = winStars[1] - winStars[0];
        starBars[0].maxValue = winStars[0];

        foreach(Slider starBar in starBars)
        {
            starBar.value = 0;
        }
    }

    private void ExpressStarRate (int stat)
    {
        if (starBars[0].value <= starBars[0].maxValue)
        {
            //starBars[0].value = stat;
            curSlider = starBars[0];
            StartCoroutine(Valuer(starBars[0].value, stat, dampTime, new Color(188f / 255f, 1f, 0f)));
        }

        if (starBars[2].value >= starBars[2].maxValue && starBars[3].value < starBars[3].maxValue)
        {
            starBars[2].value = starBars[2].maxValue;
            //starBars[3].value = stat - (starBars[2].maxValue + starBars[1].maxValue + starBars[0].maxValue);
            curSlider = starBars[3];
            StartCoroutine(Valuer(starBars[3].value, stat - (starBars[2].maxValue + starBars[1].maxValue + starBars[0].maxValue), dampTime, new Color(1f, 0f, 111f / 255f)));
        }

        if (starBars[1].value >= starBars[1].maxValue && starBars[2].value < starBars[2].maxValue)
        {
            starBars[1].value = starBars[1].maxValue;
            //starBars[2].value = stat - (starBars[1].maxValue + starBars[0].maxValue);
            curSlider = starBars[2];
            StartCoroutine(Valuer(starBars[2].value, stat - (starBars[1].maxValue + starBars[0].maxValue), dampTime, new Color(136f / 255f, 39f / 255f, 1f)));
        }
        
        if (starBars[0].value >= starBars[0].maxValue && starBars[1].value < starBars[1].maxValue)
        {
            starBars[0].value = starBars[0].maxValue;
            //starBars[1].value = stat - starBars[0].maxValue;
            curSlider = starBars[1];
            StartCoroutine(Valuer(starBars[1].value, stat - starBars[0].maxValue, dampTime, new Color(43f / 255f, 1f, 194f / 255f)));
        }        

        if (starBars[1].value <= starBars[1].maxValue)
        {
            starBars[1].value = stat - starBars[0].maxValue;
        }

        if (starBars[2].value <= starBars[2].maxValue)
        {
            starBars[2].value = stat - (starBars[0].maxValue + starBars[1].maxValue);
        }

        if (starBars[3].value <= starBars[3].maxValue)
        {
            starBars[3].value = stat - (starBars[0].maxValue + starBars[1].maxValue + starBars[2].maxValue);
        }


    }

    public IEnumerator Valuer(float startRange, float endRange, float duration, Color barColor)
    {
        if (!prototype.isEndless)
        {
            float timeRemaining = duration;
            while (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                curSlider.value = Mathf.Lerp(startRange, endRange, Mathf.InverseLerp(duration, 0, timeRemaining));
                for (int i = 0; i < starBars.Length; i++)
                {
                    starBars[i].fillRect.GetComponent<Image>().color = Color.Lerp(starBars[i].fillRect.GetComponent<Image>().color, barColor, Mathf.InverseLerp(duration, 0, timeRemaining));
                    levelCounterColor.color = barColor;
                }
                yield return null;
            }
            curSlider.value = endRange;
            for (int i = 0; i < starBars.Length; i++)
            {
                starBars[i].fillRect.GetComponent<Image>().color = barColor;
            }
        }
    }

    public void ExitButton ()
    {
        if (levelLost)
        {
            SceneManager.LoadScene("LevelSelector");
        }
        else if (!levelLost)
        {
        }

        spawner.canSpawn = false;
        for (int i = 0; i < prototype.words.Count; i++)
        {
            Destroy(prototype.words[i].gameObject);
        }
        CompleteLevel();
    }

    private IEnumerator ScoreIncreasingEffect(int incest, float incersion)
    {
        for (int i = incest; i <= incersion; i++)
        {
            levelScoreText.transform.GetChild(0).GetComponent<Text>().text = i.ToString();
            yield return new WaitForSeconds(0.002f);
        }
    }

    public void SetLevelFlag()
    {
        levelLost = true;
    }

    public void PlayButtonForEndlessControl ()
    {
        SceneManager.LoadScene("LevelMain");
    }

    private void SetBarsWidth()
    {
        if (winCondition == WinCondition.NUMBER_OF_WORDS)
        {
            l = starBarect.rect.width / (winStars[0] + (winStars[1] - winStars[0]) + (winStars[2] - winStars[1]) + (spawner.numberOfWordsToSpawn - winStars[2]));
        }
        if (winCondition == WinCondition.SCORE)
        {
            l = starBarect.rect.width / (winStars[0] + (winStars[1] - winStars[0]) + (winStars[2] - winStars[1]) + ((spawner.numberOfWordsToSpawn * scoreToIncreasePerWord) - winStars[2]));
        }

        starBarsRectWidths[0].sizeDelta = new Vector2(winStars[0] * l, starBarsRectWidths[0].sizeDelta.y);
        starBarsRectWidths[1].sizeDelta = new Vector2((winStars[1] - winStars[0]) * l, starBarsRectWidths[1].sizeDelta.y);
        starBarsRectWidths[2].sizeDelta = new Vector2((winStars[2] - winStars[1]) * l, starBarsRectWidths[2].sizeDelta.y);
        
        if(winCondition == WinCondition.NUMBER_OF_WORDS)
        {
            starBarsRectWidths[3].sizeDelta = new Vector2((spawner.numberOfWordsToSpawn - winStars[2]) * l, starBarsRectWidths[3].sizeDelta.y);
        }
        if (winCondition == WinCondition.SCORE)
        {
            starBarsRectWidths[3].sizeDelta = new Vector2(((spawner.numberOfWordsToSpawn * scoreToIncreasePerWord) - winStars[2]) * l, starBarsRectWidths[3].sizeDelta.y);
        }

        //starBar01Rect.width = 
    }

    public void SetTextToIncreaseNOW()
    {
        if (winCondition == WinCondition.NUMBER_OF_WORDS)
        {
            levelScoreText.transform.GetChild(0).GetComponent<Text>().text = numberOfWordsTypedInThisGame.ToString();
        }
    }
}

public enum WinCondition
{
    NUMBER_OF_WORDS,
    SCORE,
}
