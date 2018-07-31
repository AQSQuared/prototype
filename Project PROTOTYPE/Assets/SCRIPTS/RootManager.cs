using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class RootManager : MonoBehaviour {

    public bool isRoot = true;
    public TextMeshProUGUI titleText;
    public Vector2 letterPauseMinMax;
    public AudioClip[] sounds;
    public string myText = "PROTOTYPE";
    public GameObject[] underThingsToDisable;
    public Button exitButton;

    private Animator anim;
 
    void Start()
    {
        anim = FindObjectOfType<Animator>();
        if (FindObjectOfType<AudioController>() != null)
        {
            FindObjectOfType<AudioController>().GetComponent<AudioSource>().volume = 0.6f;
        }

        if (isRoot)
        {
            StartCoroutine(TypeText());
        }
    }

    IEnumerator TypeText()
    {
        yield return new WaitForSeconds(1.5f);

        if (titleText.text != "")
        {
            foreach (char letter in titleText.text.ToCharArray())
            {
                titleText.text = titleText.text.Remove(titleText.text.Length - 1, 1);
                if (sounds.Length >= 1)
                    GetComponent<AudioSource>().PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
                yield return new WaitForSeconds(Random.Range(letterPauseMinMax.x, letterPauseMinMax.y));
            }
        }

        yield return new WaitForSeconds(1f);

        if (titleText.text == "")
        {
            foreach (char letter in myText.ToCharArray())
            {
                titleText.text += letter;
                if (sounds.Length >= 1)
                    GetComponent<AudioSource>().PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
                yield return new WaitForSeconds((Random.Range(letterPauseMinMax.x, letterPauseMinMax.y)) / Random.Range(1f, 2f));
            }
            yield return new WaitForSeconds(0.75f);
            SceneManager.LoadScene("LevelMain");
        }
    }

    public void PlayButtonRoot()
    {
        anim.SetTrigger(Animator.StringToHash("PlayButtonIs"));
        exitButton.gameObject.SetActive(true);
        foreach(GameObject under in underThingsToDisable)
        {
            under.SetActive(false);
        }
    }

    public void ExitButtonRoot()
    {
        anim.SetTrigger(Animator.StringToHash("ExitButtonIs"));
        exitButton.gameObject.SetActive(false);
        foreach (GameObject under in underThingsToDisable)
        {
            under.SetActive(true);
        }
    }

    public void EndlessButtonRoot()
    {
        SceneManager.LoadScene("LevelConditionary");
        PlayerPrefs.SetInt("EndlessIndic", 1);
    }
    public void LevelsButtonRoot()
    {
        SceneManager.LoadScene("LevelSelector");
    }
    public void MultiplayerButtonRoot()
    {
        anim.SetBool(Animator.StringToHash("ComingSoonIs"), true);
        exitButton.gameObject.SetActive(false);
    }
    public void ComingSoonButtonRoot()
    {
        anim.SetBool(Animator.StringToHash("ComingSoonIs"), false);
        exitButton.gameObject.SetActive(true);
    }

    public void CreditLoader()
    {
        SceneManager.LoadScene("LevelCredits");
    }

    public void LeaderLoader()
    {
        SceneManager.LoadScene("Leaderboards");
    }
}
