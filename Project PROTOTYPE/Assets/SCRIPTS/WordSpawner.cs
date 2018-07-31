using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordSpawner : MonoBehaviour {

    public WordBehaviour wordToBeSpawned;
    public Vector2 spawnRepeatMinMax;
    public int numberOfWordsToSpawn = 4;
    public float spawnTimer = 4f;
    public float requiredIncreaseInSpawns = 2f;
    public bool canSpawn = true;

    private string[] possibleWords =
    {
        "consciousness", "age", "pin", "arrow", "past", "shiver", "teacher", "public", "crossing", "situation",
        "necklace", "scenario", "word", "express", "community", "steam", "preparation", "condition",
        "beg", "gold", "move", "fleet", "flow", "lack", "privilege", "resign", "gap", "recommend", "density", "sick", "lineage", "stop", "decay",
        "face", "diamond", "jewel", "ready", "carriage", "finance", "dump", "bank", "concession",
        "system", "graze", "turn", "symptom", "queue" ,"basic", "fit", "jury", "winner", "wheel", "cruelty", "similar", "fashionable", "temperature",
        "guitar", "beneficiary", "medal", "emergency", "post", "prisoner", "superior", "action", "integrity", "drama", "terminal", "drain", "conference",
        "threat", "mark", "smart", "embarrassment", "suitcase", "standard", "sting", "edge", "theater", "suffering", "effort", "grace",
        "flatware", "mercy", "potential", "permanent", "stain", "chart", "disagreement", "species", "inch", "broken", "constant", "joy",
        "parade", "tray", "river", "taxi", "firefighter", "incredible", "toll"
    };
    private string[] possibleWordsIfBackwards =
    {
        "easy", "peasy", "horrible", "why", "test"
    };

    private GameObject canvasPanel;
    private Prototype prototype;
    private float canvasWidth;
    private float cameraHeight;
    private float spawnRepeatTime;
    private float defaultTimerValue;

    private void Start()
    {
        canvasPanel = GameObject.Find("Canvas");
        canvasWidth = canvasPanel.GetComponent<RectTransform>().rect.width * canvasPanel.GetComponent<RectTransform>().localScale.x;
        cameraHeight = Camera.main.orthographicSize;
        spawnRepeatTime = spawnRepeatMinMax.x;
        defaultTimerValue = spawnTimer;
        prototype = GetComponent<Prototype>();

        GenerateRandomWords();
        //InvokeRepeating("GenerateRandomWords", spawnTimer, spawnRepeatTime);
    }

    public void GenerateRandomWords()
    {
        if (canSpawn)
        {
            if (!prototype.isEndless)
            {
                for (int i = 0; i < numberOfWordsToSpawn; i++)
                {
                    SpawnWords();
                    spawnTimer += requiredIncreaseInSpawns;
                }
            }
            if (prototype.isEndless)
            {
                for (int i = 0; i < Random.Range(8, numberOfWordsToSpawn); i++)
                {
                    SpawnWords();
                    spawnTimer += requiredIncreaseInSpawns;
                }
            }
            spawnTimer = defaultTimerValue;
        }
        else if (!canSpawn)
        {
            DisableSpawning();
        }
    }

    private void SpawnWords()
    {
        WordBehaviour word = (WordBehaviour)Instantiate(wordToBeSpawned, new Vector3(Random.Range(-canvasWidth + canvasWidth / 2, canvasWidth - canvasWidth / 2) / 2, cameraHeight + (wordToBeSpawned.GetComponent<RectTransform>().rect.height) / 144f, 0), Quaternion.identity);

        if(prototype.isForwards)
            word.GetComponent<Text>().text = possibleWords[Random.Range(0, possibleWords.Length)];

        if(prototype.isBackwards)
            word.GetComponent<Text>().text = possibleWordsIfBackwards[Random.Range(0, possibleWordsIfBackwards.Length)];

        word.transform.SetParent(canvasPanel.transform);
        word.startFallingToggleTimer = spawnTimer;
        StartCoroutine(word.ToggleFalling());
        GetComponent<Prototype>().RefreshLetters();
    }

    public void DisableSpawning()
    {
        canSpawn = false;
    }

    public void IncreaseDifficulty (float difficulty)
    {
        if(requiredIncreaseInSpawns > spawnRepeatMinMax.y)
        {
            spawnRepeatTime -= difficulty;
            prototype.wordFallSpeed += difficulty;
            requiredIncreaseInSpawns -= difficulty * 2f;
        }
    }
}
