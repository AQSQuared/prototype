using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordBehaviour : MonoBehaviour {

    private Prototype functionality;
    private float wordFallSpeed;

    [HideInInspector]
    public bool startFalling = false;
    [HideInInspector]
    public float startFallingToggleTimer;
    [HideInInspector]
    public bool isRedWord;

    // Use this for initialization
    void Start()
    {
        functionality = FindObjectOfType<Prototype>();

        functionality.words.Add(this.GetComponent<Text>());
        functionality.RefreshLetters();

        wordFallSpeed = Random.Range(0.5f, functionality.wordFallSpeed);

        isRedWord = RedWordIndicator(functionality.isRedWordRound);

        if (isRedWord)
        {
            GetComponent<Text>().color = Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (startFalling)
            transform.Translate(Vector3.down * wordFallSpeed * Time.deltaTime);
    }

    public IEnumerator ToggleFalling()
    {
        yield return new WaitForSeconds(startFallingToggleTimer);
        startFalling = true;
        functionality.RefreshLetters();
    }

    public void RemoveWordUniversally()
    {
        functionality.words.Remove(this.GetComponent<Text>());
        functionality.RefreshLetters();

        if (GetComponent<Text>() == functionality.placeholderText)
        {
            functionality.characters.Clear();
            functionality.currentCharacter = null;
        }
    }

    private bool RedWordIndicator (bool isRedWordRound)
    {
        if (isRedWordRound)
        {
            float randomNumberBasis = Random.Range(0, 4);

            if (randomNumberBasis == 0)
            {
                return true;
            }

        }
        return false;
    }
}
