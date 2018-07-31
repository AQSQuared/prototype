using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCommons : MonoBehaviour {

    Button thisButton;
    AudioSource source;

    private void Start()
    {
        thisButton = GetComponent<Button>();
        source = GetComponent<AudioSource>();

        thisButton.onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        source.Play();
    }
}
