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

    private List<string> firstCharsOfAvailableWords = new List<string>();
    private List<string> lastCharsOfAvailableWords = new List<string>();

    private string typedChar;
    bool completedOnce = false;

    private GameManager manager;

    private void Start()
    {
        string roundType = "";

        if (isForwards)
            roundType = "forward";
        if (isBackwards)
            roundType = "backward";

        manager = GetComponent<GameManager>();
        manager.SetKeyboard(isQwertyKeys, isAzertyKeys, true, isRedWordRound, isMistakeMode, roundType);
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
                        manager.DecreaseScore(decreaseIfMistake);
                    }
                }
            }

            if (characters.Count < 1 && placeholderText != null)
            {
                if (placeholderText.GetComponent<WordBehaviour>().isRedWord)
                {
                    manager.DecreaseScore(decreaseIfRedWordTyped);
                }
                Destroy(placeholderText.gameObject);
                placeholderText = null;
                currentCharacter = null;
                if (isEndless)
                {
                    GetComponent<WordSpawner>().GenerateRandomWords();
                }
                GetComponent<GameManager>().IncreaseNumberOfWordsTyped();
            }
        }

        if (words.Count <= 0 && !isEndless && !completedOnce)
        {
            manager.CompleteLevel();
            completedOnce = true;
        }

#endif

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
                        manager.DecreaseScore(decreaseIfMistake);
                    }
                }
            //}

            if (characters.Count < 1 && placeholderText != null)
            {
                if (placeholderText.GetComponent<WordBehaviour>().isRedWord)
                {
                    manager.DecreaseScore(decreaseIfRedWordTyped);
                }
                Destroy(placeholderText.gameObject);
                placeholderText = null;
                currentCharacter = null;
                if (isEndless)
                {
                    GetComponent<WordSpawner>().GenerateRandomWords();
                }
                GetComponent<GameManager>().IncreaseNumberOfWordsTyped();
            }
        }

        if (words.Count <= 0 && !isEndless && !completedOnce)
        {
            manager.CompleteLevel();
            completedOnce = true;
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
}
