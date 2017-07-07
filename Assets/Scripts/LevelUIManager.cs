using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Level user interface manager.
/// </summary>
public class LevelUIManager : AbstractUIManager
{
    /// <summary>
    /// LevelUIManager instance.
    /// </summary>
    public static LevelUIManager instance;
    /// <summary>
    /// Whether the player can pause or not. Once the player has paused he can't redo it until pause menu ha finished tweening.
    /// </summary>
    public bool canPause;
    /// <summary>
    /// The timer for round countdown.
    /// </summary>
    public UILabel timer;
    /// <summary>
    /// The players infos.
    /// </summary>
    public UIPanel[] playersInfos;
    /// <summary>
    /// The players icons.
    /// </summary>
    public UISprite[] playersIcons;
    /// <summary>
    /// The players icons backgrounds.
    /// </summary>
    public UISprite[] playersIconsBackgrounds;
    /// <summary>
    /// Players scores.
    /// </summary>
    public UILabel[] scores;
    /// <summary>
    /// The best player symbols.
    /// </summary>
    public UISprite[] bestPlayers;
    /// <summary>
    /// The pause menu.
    /// </summary>
    public UIPanel pauseMenu;
    /// <summary>
    /// The pause menu tween alpha.
    /// </summary>
    public TweenAlpha pauseMenuTweenAlpha;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        instance = this;
    }

    public void SetCanPauseTrue()
    {
        canPause = true;
    }

    /// <summary>
    /// Sets the timer.
    /// </summary>
    /// <param name="timer">Timer.</param>
    public void SetTimer(float timer)
    {
        int minutes;
        int seconds;

        minutes = Mathf.FloorToInt(timer / 60f);
        seconds = Mathf.FloorToInt(timer - minutes * 60);
        this.timer.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// Sets a player stress.
    /// </summary>
    /// <param name="stress">Level of stress (a value between 0 and 1).</param>
    /// <param name="playerId">Player Id (a 0-based index).</param>
    public void SetStress(float stress, int playerId)
    {
        //stressLevels [playerId].value = Mathf.Clamp (stress, 0, 1);
    }

    /// <summary>
    /// Sets a player score.
    /// </summary>
    /// <param name="score">Player's score.</param>
    /// <param name="playerId">Player Id (a 0-based index).</param>
    public void SetScore(int score, int playerId)
    {
        scores[playerId].text = score.ToString();
    }

    /// <summary>
    /// Sets the best player.
    /// </summary>
    /// <param name="playerId">Player identifier.</param>
    private void SetBestPlayer(int playerId)
    {
        int playerIndex;

        for (playerIndex = 0; playerIndex < bestPlayers.Length; playerIndex++)
        {
            bestPlayers[playerIndex].enabled = (playerIndex == playerId);
        }
    }

    /// <summary>
    /// Inits the UI.
    /// </summary>
    public void InitUI()
    {
        GameObject[] players;
        SpriteRenderer playerIdentifierRenderer = null;
        string playerIcon;
        int playerIndex;

        players = GameManager.instance.GetPlayers();
        for (playerIndex = 0; playerIndex < players.Length; playerIndex++)
        {
            scores[playerIndex].text = "0";
            if (players[playerIndex] != null)
            {
                //Debug.Log(playerIndex);
                playerIcon = Utility.GetPlayerIcon(players[playerIndex]);
                Statistics.instance.SetPlayerIcon(playerIndex, playerIcon);
                playersIcons[playerIndex].spriteName = playerIcon;
                playersIconsBackgrounds[playerIndex].color = Configuration.instance.playersColors[playerIndex];
                playerIdentifierRenderer = players[playerIndex].GetComponentInChildren<SpriteRenderer>();

                if (playerIdentifierRenderer != null)
                {
                    playerIdentifierRenderer.color = Configuration.instance.playersColors[playerIndex];
                }
                bestPlayers[playerIndex].enabled = false;
                playersInfos[playerIndex].enabled = true;
            }
        }
        for (; playerIndex < playersInfos.Length; playerIndex++)
        {
            playersInfos[playerIndex].enabled = false;
        }
    }

    /// <summary>
    /// Sets the pause menu visible.
    /// </summary>
    /// <param name="visible">If set to <c>true</c> visible.</param>
    public void SetPauseMenuVisible(bool visible)
    {
        if (visible)
        {
            //Debug.Log("Minchia di lato");
            Utility.OpenPopup(pauseMenu, pauseMenuTweenAlpha);
        }
        else
        {
            //Debug.Log("Lurido pezzo di merda");
            Utility.FadeOut(pauseMenuTweenAlpha, this, "ClosePauseMenu");
        }
    }

    /// <summary>
    /// Closes the pause menu.
    /// </summary>
    private void ClosePauseMenu()
    {
        //Debug.Log("EEEEEEEEEEEEEE");
        pauseMenu.gameObject.SetActive(false);
        GameManager.instance.SetPause(false);
        pauseMenuTweenAlpha.onFinished.Clear();
        Time.timeScale = 1;
    }

    /// <summary>
    /// Clicking on resume button
    /// </summary>
    public void OnClickResumeButton()
    {
        SetPauseMenuVisible(false);
    }

    /// <summary>
    /// Clicking on restart button
    /// </summary>
    public void OnClickRestartButton()
    {
        SceneController.instance.LoadActiveScene();
    }

    /// <summary>
    /// Clicking on quit button
    /// </summary>
    public void OnClickQuitButton()
    {
        SceneController.instance.OnClickQuitButton();
    }

    /// <summary>
    /// Loads menu scene.
    /// </summary>
    public void OnClickBackToMenu()
    {
        SceneController.instance.LoadSceneByName("Menu");
    }
}
