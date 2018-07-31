using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour {

    AudioSource aud;

    private static AudioController playerInstance;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);

        if (playerInstance == null)
        {
            playerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        aud = GetComponent<AudioSource>();

    }
}
