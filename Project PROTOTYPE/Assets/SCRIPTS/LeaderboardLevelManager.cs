using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardLevelManager : MonoBehaviour {

    public AudioClip[] starSounds;

    public void ExitLeaderboards()
    {
        SceneManager.LoadScene("LevelMain");
    }

    public void PlayStarSound03()
    {
        GetComponent<AudioSource>().volume = 0.2f;
        GetComponent<AudioSource>().PlayOneShot(starSounds[2]);
    }
    public void PlayStarSound02()
    {
        GetComponent<AudioSource>().volume = 0.2f;
        GetComponent<AudioSource>().PlayOneShot(starSounds[1]);
    }
    public void PlayStarSound01()
    {
        GetComponent<AudioSource>().volume = 0.2f;
        GetComponent<AudioSource>().PlayOneShot(starSounds[0]);
    }
}
