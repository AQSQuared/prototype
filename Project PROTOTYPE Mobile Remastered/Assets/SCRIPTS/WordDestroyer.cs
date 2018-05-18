using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordDestroyer : MonoBehaviour {
    
	private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Word")
        {
            other.GetComponent<WordBehaviour>().RemoveWordUniversally();
            Destroy(other.gameObject);
        }
    }
}
