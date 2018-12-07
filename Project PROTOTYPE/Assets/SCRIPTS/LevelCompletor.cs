using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelCompletor : MonoBehaviour {

    public GameObject levelCompleteParent;
    public Text scoreSlashText;
    public Text levelCompletionIndicatorText;
    public Image nextLevelButton;
    public AudioClip winSound;
    public AudioClip scoreCountSound;

    public GameObject[] thingsToDisable;
    public AudioClip[] loseSounds;

    private Animator anim;
    private Text incestText;

    private void Start()
    {
        anim = levelCompleteParent.GetComponent<Animator>();
    }

    public void OnLevelCompletion (WinCondition _levelWinCondition, float incersion, int stars, int _levelNumber)
    {
        foreach(GameObject go in thingsToDisable)
        {
            go.SetActive(false);
        }

        levelCompleteParent.gameObject.SetActive(true);
        anim.SetTrigger(Animator.StringToHash("Complete"));

        if (stars <= 0)
        {
            GetComponent<AudioSource>().PlayOneShot(loseSounds[0]);
            GetComponent<AudioSource>().PlayOneShot(loseSounds[1]);
            levelCompletionIndicatorText.text = "LEVEL FAILED.";
            nextLevelButton.color = new Color(1f, 113f / 255f, 113f / 255f);
            nextLevelButton.transform.GetChild(0).Find("RetryButton").gameObject.SetActive(true);
            nextLevelButton.transform.GetChild(0).Find("PlayButton").gameObject.SetActive(false);
            Camera.main.backgroundColor = new Color(1f, 68f / 255f, 68f / 255f);

            GetComponent<GameManager>().SetLevelFlag();
        }
        else if (stars > 0)
        {
            GetComponent<AudioSource>().PlayOneShot(winSound);
            levelCompletionIndicatorText.text = "LEVEL COMPLETED.";
            nextLevelButton.color = new Color(113f / 255f, 1f, 243f / 255f);
            nextLevelButton.transform.GetChild(0).Find("PlayButton").gameObject.SetActive(true);
            nextLevelButton.transform.GetChild(0).Find("RetryButton").gameObject.SetActive(false);
            Camera.main.backgroundColor = Color.black;

            if (_levelNumber >= PlayerPrefs.GetInt("LevelsCompleted"))
            {
                PlayerPrefs.SetInt("LevelsCompleted", _levelNumber);
            }
        }

        if (_levelWinCondition == WinCondition.NUMBER_OF_WORDS)
        {
            scoreSlashText.text = "WORDS";
        }
        if (_levelWinCondition == WinCondition.SCORE)
        {
            scoreSlashText.text = "SCORE";
        }

        incestText = scoreSlashText.transform.GetChild(0).GetComponent<Text>();
        StartCoroutine(ScoreIncreasingEffect(incersion, _levelWinCondition, stars));
        
    }

    private void StarDecider (int stars)
    {
        int threeStarWin = Animator.StringToHash("ThreeStars");
        int twoStarWin = Animator.StringToHash("TwoStars");
        int oneStarWin = Animator.StringToHash("OneStar");

        if(stars == 3)
        {
            anim.SetTrigger(threeStarWin);
        }
        if (stars == 2)
        {
            anim.SetTrigger(twoStarWin);
        }
        if (stars == 1)
        {
            anim.SetTrigger(oneStarWin);
        }
    }

    private IEnumerator ScoreIncreasingEffect (float incersion, WinCondition _winCondition, int _stars)
    {
        for (int i = 0; i <= incersion; i++)
        {
            incestText.text = i.ToString();
            if(scoreCountSound != null && _winCondition == WinCondition.SCORE)
            {
                GetComponent<AudioSource>().volume = 0.15f;
                GetComponent<AudioSource>().PlayOneShot(scoreCountSound);
            }
            if (_winCondition == WinCondition.NUMBER_OF_WORDS)
            {
                yield return new WaitForSeconds(0.15f);
            }
            if (_winCondition == WinCondition.SCORE)
            {
                yield return new WaitForSeconds(0.00125f);
            }
            if(Input.touchCount == 1 && i > 30)
            {
                i = Mathf.FloorToInt(incersion);
            }
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space) && i > 30)
            {
                i = Mathf.FloorToInt(incersion);
                incestText.text = i.ToString();
            }
#endif
        }

        StarDecider(_stars);
    }

    public void RestartLevel()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("LevelSelector");
    }

    public void PlayButton ()
    {
        SceneManager.LoadScene("LevelSelector");
    }
}
