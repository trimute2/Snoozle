using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStats : Singleton<GameStats>
{
    public bool IsPlayerGhost = false;
    public bool didPlayerWin = false;
    public int hpLeft = 15;

    public void EndGame(bool win)
    {
        didPlayerWin = win;
        SceneManager.LoadSceneAsync("GameResult");
        Time.timeScale = 1;
    }

}
