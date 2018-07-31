using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WordDestroyer : MonoBehaviour {

    public float livesForEndless = 8;
    public float adsTime = 12;
    public float dampTimeForLivesEndless = 0.2f;
    public Slider livesSlider;
    public GameObject exitButton01;
    public GameObject adsMerchantVars;
    public bool endlessIsEnded;
    public Text nameTxt;
    public AudioClip clipShit;
    public Text hst;

    public GameObject[] thingsToDisableOnAds;

    [HideInInspector]
    public bool countToAds = false;
    [HideInInspector]
    public bool canSpawnWordsAgain = true;

    private Animator anim;
    private float oldLife;
    private Image adRadialCounterImage;
    private Text timeText;
    private bool alreadyShowedAd = false;
    private bool canDownCount = true;
    private int highScoreShit;

    private void Start()
    {
        if (FindObjectOfType<Prototype>().isEndless)
        {
            anim = GameObject.Find("Panelm").GetComponent<Animator>();
            adRadialCounterImage = adsMerchantVars.transform.Find("timer").GetComponent<Image>();
            timeText = adRadialCounterImage.transform.GetChild(0).GetComponent<Text>();

            livesSlider.maxValue = livesForEndless;
            livesSlider.value = livesSlider.maxValue;
        }

        if(PlayerPrefs.GetString("NameTypedOnce") != "")
        {
            nameTxt.GetComponentInParent<InputField>().interactable = false;
            nameTxt.color = Color.white;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Word")
        {
            other.GetComponent<WordBehaviour>().RemoveWordUniversally();
            Destroy(other.gameObject);
            if (FindObjectOfType<Prototype>().isEndless)
            {
                if(livesForEndless > 0)
                {
                    GetComponent<AudioSource>().PlayOneShot(clipShit);
                    oldLife = livesForEndless;
                    livesForEndless -= 1;
                    StartCoroutine(Subtactor(oldLife, livesForEndless, dampTimeForLivesEndless));

                    if (livesForEndless <= 0)
                    {

                        if (Application.internetReachability == NetworkReachability.NotReachable || alreadyShowedAd)
                            StartCoroutine(EndlessCompletion());

                        if (Application.internetReachability != NetworkReachability.NotReachable && !alreadyShowedAd)
                        {
                            alreadyShowedAd = true;
                            StartCoroutine(ShowAds());
                            canSpawnWordsAgain = false;
                        }
                    }
                }
            }
        }
    }

    private IEnumerator ShowAds()
    {
        FindObjectOfType<Prototype>().DisableWordColors();
        FindObjectOfType<WordSpawner>().canSpawn = false;
        anim.SetBool("ShowAdMerchant", true);
        foreach (GameObject go in thingsToDisableOnAds)
        {
            go.SetActive(false);
        
        }
        GetComponent<MeshRenderer>().enabled = false;
        for (int i = Mathf.FloorToInt(adsTime); i >= 0; i--)
        {
            adRadialCounterImage.fillAmount -= (1 / adsTime);
            timeText.text = i.ToString("f0");
            yield return new WaitForSeconds(1f);
            if (i <= 0 && !countToAds)
            {
                StartCoroutine(EndlessCompletion());
                EnableDisabledEndlessComponents();
                
            }
        }
    }

    private IEnumerator EndlessCompletion ()
    {
        endlessIsEnded = true;
        FindObjectOfType<Prototype>().DisableWordColors();
        StartCoroutine(Subtactor(livesSlider.value, 0, dampTimeForLivesEndless));
        anim.SetTrigger(Animator.StringToHash("IsHighscore"));
        FindObjectOfType<WordSpawner>().canSpawn = false;

        highScoreShit = FindObjectOfType<GameManager>().numberOfWordsTypedInThisGame;
        if (PlayerPrefs.GetInt("HighS") < highScoreShit)
        {
            PlayerPrefs.SetInt("HighS", highScoreShit);
        }
        hst.text = PlayerPrefs.GetInt("HighS").ToString();

        yield return new WaitForSeconds(1f);

        thingsToDisableOnAds[3].gameObject.SetActive(false);
        exitButton01.SetActive(false);
        gameObject.SetActive(false);
    }

    private IEnumerator Subtactor(float startRange, float endRange, float duration)
    {
        float timeRemaining = duration;
        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            livesSlider.value = Mathf.Lerp(startRange, endRange, Mathf.InverseLerp(duration, 0, timeRemaining));
            yield return null;
        }
    }

    public void ExitButtonControllerEndless ()
    {
        livesForEndless = 0;
        StartCoroutine(EndlessCompletion());
    }

    public void EnableDisabledEndlessComponents()
    {
        foreach (GameObject go in thingsToDisableOnAds)
        {
            go.SetActive(true);
        }
    }

    public void RestoreLostLives ()
    {
        canSpawnWordsAgain = true;
        livesForEndless = 8;
        GameObject.Find("Panelm").GetComponent<Animator>().SetBool(Animator.StringToHash("ShowAdMerchant"), false);
        EnableDisabledEndlessComponents();
        GetComponent<MeshRenderer>().enabled = true;
        FindObjectOfType<WordSpawner>().canSpawn = true;
        //FindObjectOfType<WordSpawner>().GenerateRandomWords();
        livesSlider.value = FindObjectOfType<WordDestroyer>().livesForEndless;
        FindObjectOfType<Prototype>().EnableWordColors();
    }

    public void CancelButtonForAds()
    {
        EnableDisabledEndlessComponents();
        StartCoroutine(EndlessCompletion());
    }

    public void SubmitHighscoreTrigButton()
    {
        anim.SetBool(Animator.StringToHash("SubHS"), true);
        nameTxt.GetComponentInParent<InputField>().text = PlayerPrefs.GetString("NameTypedOnce").ToString();
        print(PlayerPrefs.GetString("NameTypedOnce"));
    }

    public void SubmitHighscore()
    {
        Highscores.AddNewHighscore(nameTxt.text, FindObjectOfType<GameManager>().numberOfWordsTypedInThisGame);
        if (PlayerPrefs.GetString("NameTypedOnce") == "")
        {
            PlayerPrefs.SetString("NameTypedOnce", nameTxt.text);
        }
        SceneManager.LoadScene("Leaderboards");       
    }
}
