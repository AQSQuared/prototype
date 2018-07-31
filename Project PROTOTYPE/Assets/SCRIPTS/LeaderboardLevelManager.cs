using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardLevelManager : MonoBehaviour {
    
    public void ExitLeaderboards()
    {
        SceneManager.LoadScene("LevelMain");
    }
}
