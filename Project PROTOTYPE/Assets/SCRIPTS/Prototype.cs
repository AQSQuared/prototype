using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prototype : MonoBehaviour {

    [Header("Type of Level")]
    public bool isForwards;
    public bool isBackwards;
    public bool isEndless;
    public bool isRedWordRound;
    public bool isNumberMode;
    public bool isMistakeMode;
    public bool isQwertyKeys;
    public bool isAzertyKeys;

    [Header("Controls")]
    public float wordFallSpeed;
    public int decreaseIfMistake;
    public int decreaseIfRedWordTyped;
    public List<Text> words = new List<Text>();
    public List<string> characters = new List<string>();
    public Text placeholderText = null;
    public string currentCharacter;
    public Color selectedPlaceholderTextColor;
    public AudioClip[] soundForKeys;
    public AudioClip errorSound;
    public Animator fwbwtranim;

    private List<string> firstCharsOfAvailableWords = new List<string>();
    private List<string> lastCharsOfAvailableWords = new List<string>();

    private string typedChar;
    bool completedOnce = false;
    int numberOfWordsForEndless;

    private GameManager manager;
    private Text fwbwText;

    private void Start()
    {
        string roundType = "";

        if (isForwards)
            roundType = "forward";
        if (isBackwards)
            roundType = "backward";

        manager = GetComponent<GameManager>();
        manager.SetKeyboard(isQwertyKeys, isAzertyKeys, isNumberMode, true, isNumberMode, isMistakeMode, roundType);

        fwbwText = fwbwtranim.gameObject.transform.Find("res").GetComponent<Text>();
        SetAndPlayFWBWTR();
    }

    private void Update()
    {
#if UNITY_EDITOR

        if (isForwards)
        {
            foreach (string firstChars in firstCharsOfAvailableWords)
            {
                if (Input.GetKeyDown(firstChars) && placeholderText == null)
                {
                    placeholderText = GetTextWithFirstLetter(firstChars);
                    if (placeholderText.GetComponent<WordBehaviour>().startFalling)
                    {
                        SelectWord();
                        words.Remove(placeholderText);
                        words.Add(placeholderText);
                    }
                    else
                    {
                        placeholderText = null;
                    }
                    //print(placeholderText.text);
                }
            }
        }

        if (isBackwards)
        {
            foreach (string lastChars in lastCharsOfAvailableWords)
            {
                if (Input.GetKeyDown(lastChars) && placeholderText == null)
                {
                    placeholderText = GetTextWithLastLetter(lastChars);
                    if (placeholderText.GetComponent<WordBehaviour>().startFalling)
                    {
                        SelectWord();
                        words.Remove(placeholderText);
                        words.Add(placeholderText);
                    }
                    else
                    {
                        placeholderText = null;
                    }
                    //print(placeholderText.text);
                }
            }
        }

        if (currentCharacter != null)
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(currentCharacter))
                {
                    GetComponent<AudioSource>().PlayOneShot(soundForKeys[Random.Range(0, soundForKeys.Length)]);

                    characters.Remove(currentCharacter);
                    if (characters.Count != 0)
                    {
                        currentCharacter = characters[0];
                    }
                    if (isForwards)
                    {
                        placeholderText.text = placeholderText.text.Remove(0, 1);
                    }
                    if (isBackwards)
                    {
                        placeholderText.text = placeholderText.text.Remove(placeholderText.text.Length - 1, 1);
                    }
                }
                else
                {
                    if (isMistakeMode)
                    {
                        GetComponent<AudioSource>().PlayOneShot(errorSound);
                        manager.DecreaseScore(decreaseIfMistake);
                    }
                }
            }

            if (characters.Count < 1 && placeholderText != null)
            {
                if (isEndless)
                {
                    numberOfWordsForEndless += 1;
                    GameObject.Find("EndlessWordsCounter").GetComponent<Text>().text = numberOfWordsForEndless.ToString();

                    for (int i = 0; i < manager.difficultyBoard.Length; i++)
                    {
                        if (numberOfWordsForEndless == manager.difficultyBoard[i])
                        {
                            GetComponent<WordSpawner>().IncreaseDifficulty(manager.difficulty);
                        }
                    }
                }
                if (placeholderText.GetComponent<WordBehaviour>().isRedWord)
                {
                    manager.DecreaseScore(decreaseIfRedWordTyped);
                }
                Destroy(placeholderText.gameObject);
                placeholderText = null;
                currentCharacter = null;
                if (isEndless)
                {
                    //GetComponent<WordSpawner>().GenerateRandomWords();
                }
                GetComponent<GameManager>().IncreaseNumberOfWordsTyped();
            }
        }
        

#endif
        if (words.Count <= 0 && !isEndless && !completedOnce)
        {
            manager.CompleteLevel();
            completedOnce = true;
            print("gene...");
        }
        if (words.Count <= 0 && isEndless && FindObjectOfType<WordDestroyer>().canSpawnWordsAgain)
        {
            GetComponent<WordSpawner>().GenerateRandomWords();
            print("generating...");
        }

        if (placeholderText != null)
        {
            placeholderText.color = selectedPlaceholderTextColor;
        }
    }

    private void GetCharacter()
    {
        if (isForwards)
        {
            foreach (string firstChars in firstCharsOfAvailableWords)
            {
                if (typedChar == firstChars && placeholderText == null)
                {
                    placeholderText = GetTextWithFirstLetter(firstChars);
                    if (placeholderText.GetComponent<WordBehaviour>().startFalling)
                    {
                        SelectWord();
                        words.Remove(placeholderText);
                        words.Add(placeholderText);
                    }
                    else
                    {
                        placeholderText = null;
                    }
                    //print(placeholderText.text);
                }
            }
        }

        if (isBackwards)
        {
            foreach (string lastChars in lastCharsOfAvailableWords)
            {
                if (typedChar == lastChars && placeholderText == null)
                {
                    placeholderText = GetTextWithLastLetter(lastChars);
                    if (placeholderText.GetComponent<WordBehaviour>().startFalling)
                    {
                        SelectWord();
                        words.Remove(placeholderText);
                        words.Add(placeholderText);
                    }
                    else
                    {
                        placeholderText = null;
                    }
                    //print(placeholderText.text);
                }
            }
        }

        if (currentCharacter != null)
        {
            //if (Input.anyKeyDown)
            //{
            if (typedChar == currentCharacter)
            {
                GetComponent<AudioSource>().PlayOneShot(soundForKeys[Random.Range(0, soundForKeys.Length)]);

                characters.Remove(currentCharacter);
                if (characters.Count != 0)
                {
                    currentCharacter = characters[0];
                }
                if (isForwards)
                {
                    placeholderText.text = placeholderText.text.Remove(0, 1);
                }
                if (isBackwards)
                {
                    placeholderText.text = placeholderText.text.Remove(placeholderText.text.Length - 1, 1);
                }
            }
            else
            {
                if (isMistakeMode)
                {
                    GetComponent<AudioSource>().PlayOneShot(errorSound);

                    manager.DecreaseScore(decreaseIfMistake);
                }
            }
            //}

            if (characters.Count < 1 && placeholderText != null)
            {
                if (isEndless)
                {
                    numberOfWordsForEndless += 1;
                    GameObject.Find("EndlessWordsCounter").GetComponent<Text>().text = numberOfWordsForEndless.ToString();

                    for (int i = 0; i < manager.difficultyBoard.Length; i++)
                    {
                        if (numberOfWordsForEndless == manager.difficultyBoard[i])
                        {
                            GetComponent<WordSpawner>().IncreaseDifficulty(manager.difficulty);
                        }
                    }
                }
                if (placeholderText.GetComponent<WordBehaviour>().isRedWord)
                {
                    manager.DecreaseScore(decreaseIfRedWordTyped);
                }
                Destroy(placeholderText.gameObject);
                placeholderText = null;
                currentCharacter = null;
                if (isEndless)
                {
                    //GetComponent<WordSpawner>().GenerateRandomWords();
                }
                GetComponent<GameManager>().IncreaseNumberOfWordsTyped();
            }
        }

        if (words.Count <= 0 && !isEndless && !completedOnce)
        {
            manager.CompleteLevel();
            completedOnce = true;
        }
        if (words.Count <= 0 && isEndless)
        {
            GetComponent<WordSpawner>().GenerateRandomWords();
        }
    }

    public void RefreshLetters()
    {
        if (isForwards)
        {
            firstCharsOfAvailableWords.Clear();
            foreach (Text word in words)
            {
                if (word.text == "")
                {
                    words.Remove(word);
                }
                firstCharsOfAvailableWords.Add(word.text.Substring(0, 1));
            }
        }

        if (isBackwards)
        {
            lastCharsOfAvailableWords.Clear();
            foreach (Text word in words)
            {
                if (word.text == "")
                {
                    words.Remove(word);
                }
                lastCharsOfAvailableWords.Add(word.text.Substring(word.text.Length - 1, 1));
            }
        }
    }

    Text GetTextWithFirstLetter(string firstChar)
    {
        foreach (Text word in words)
        {
            if (firstChar == word.text.Substring(0, 1))
            {
                return word;
            }
        }
        return null;
    }

    Text GetTextWithLastLetter(string lastChar)
    {
        foreach (Text word in words)
        {
            if (lastChar == word.text.Substring(word.text.Length - 1, 1))
            {
                return word;
            }
        }
        return null;
    }

    private void SelectWord()
    {
        if (isForwards)
        {
            for (int i = 0; i < placeholderText.text.Length; i++)
            {
                characters.Add(placeholderText.text.Substring(i, 1));
                string character = placeholderText.text.Substring(i, 1);
                //print(character);
            }
            currentCharacter = characters[0];
        }

        if (isBackwards)
        {
            for (int i = placeholderText.text.Length - 1; i > -1; i--)
            {
                characters.Add(placeholderText.text.Substring(i, 1));
                string character = placeholderText.text.Substring(i, 1);
                //print(character);
            }
            currentCharacter = characters[0];
        }
    }

    public void SetCharacter (string character)
    {
        typedChar = character;
        GetCharacter();
    } 

    public void DisableWordColors ()
    {
        foreach(Text word in words)
        {
            word.color = new Color(word.color.r, word.color.g, word.color.b, 0);
        }
    }

    public void EnableWordColors()
    {
        foreach (Text word in words)
        {
            word.color = new Color(word.color.r, word.color.g, word.color.b, 1);
        }
    }

    public void SetAndPlayFWBWTR()
    {
        if (isForwards)
        {
            fwbwText.text = "FORWARDS";
        }
        if (isBackwards)
        {
            fwbwText.text = "BACKWARDS";
        }
        fwbwtranim.SetTrigger(Animator.StringToHash("fwbwTr"));
    }
}
