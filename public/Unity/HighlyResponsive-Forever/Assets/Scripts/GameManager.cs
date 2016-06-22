using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [Header("Timer")]
    public float TimeTillOvertime; // When bullets start raining down

    [Header("Score")]
    public int InitialCardScore;

    [Header("Data caps")]
    [Tooltip("Number of digits for score")]
    public float ScoreCap;
    [Tooltip("Number of digits for combo")]
    public float ComboCap;

    [Header("Object references")]
    public Player RefPlayer;
    public Ball RefBall;
    public TileMap RefMap;

    [Header("UI references")]
    public GameObject RefGameOverPanel;
    public GameObject RefLifePanel;
    public GameObject RefBombPanel;
    public Text RefHighScore;
    public Text RefCurrentScore;
    public Text RefMaxCombo;
    public Text RefCombo;
    public Text RefTimer;

    // Properties
    public int TileCount
    {
        set
        {
            tileCount = value;
        }

        get
        {
            return tileCount;
        }
    }
    public int DefaultTileCount
    {
        set
        {
            defaultTileCount = value;
        }

        get
        {
            return defaultTileCount;
        }
    }
    public int Score
    {
        set
        {
            score = value;
        }

        get
        { return score;
        }
    }
    public int Highscore
    {
        get
        {
            return highscore;
        }

        set
        {
            highscore = value;
        }
    }
    public int Combo
    {
        get
        {
            return combo;
        }

        set
        {
            combo = value;
        }
    }
    public int MaxCombo
    {
        get
        {
            return maxCombo;
        }

        set
        {
            maxCombo = value;
        }
    }

    private List<GameObject> refLives = new List<GameObject>();
    private List<GameObject> refBombs = new List<GameObject>();
    private CountdownTimer overtimeTimer;

    // Scores and combos
    private int score;
    private int highscore;
    private int combo;
    private int maxCombo;

    // Tile count
    private int tileCount;
    private int defaultTileCount;

    // Game states
    private bool win;
    private bool gameover;

    // Use this for initialization
    void Start()
    {
        win = false;
        gameover = false;

        // Fetch all lives UI object
        if (RefLifePanel)
        {
            var lives = RefLifePanel.GetComponentsInChildren<Transform>(true).ToList();
            lives.Remove(RefLifePanel.transform); // Removes panel object from the lives
            foreach (var life in lives)
            {
                refLives.Add(life.gameObject);
            }
        }

        // Fetch all bombs UI object
        if (RefBombPanel)
        {
            var bombs = RefBombPanel.GetComponentsInChildren<Transform>(true).ToList();
            bombs.Remove(RefBombPanel.transform); // Removes panel object from the lives
            foreach (var bomb in bombs)
            {
                refBombs.Add(bomb.gameObject);
            }
        }

        if (!overtimeTimer)
        {
            overtimeTimer = ScriptableObject.CreateInstance<CountdownTimer>();
        }
        overtimeTimer.Init(TimeTillOvertime, false);

        refreshUI();

        // Load map
        RefMap.Load("1");
    }

    // Update is called once per frame
    void Update()
    {
        // Not gameover and should change to gameover, set game over
        if (!isGameOver() && shouldGameOver())
        {
            EndLevel(win);
        }

        checkEnd();

        // Updates when not gameover
        if (!shouldGameOver())
        {
            overtimeTimer.Update();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Toggle pause
                Pause();
            }

            refreshUI();
        }
    }

    public void AddCount(int add)
    {
        tileCount += add;
    }

    public void ReduceCount(int reduce)
    {
        tileCount -= reduce;

        if (tileCount <= 0)
        {
            win = true;
        }
    }

    #region Buttons

    /// <summary>
    /// Method for retry button in gameover screen
    /// </summary>
    public void Button_Retry()
    {
        RefGameOverPanel.SetActive(false);
        RestartLevel();
    }

    #endregion Buttons

    /// <summary>
    /// Restarts the game level
    /// </summary>
    public void RestartLevel()
    {
        // Reset game state
        win = false;
        gameover = false;

        // Reset win condition
        TileCount = DefaultTileCount;

        // Reset score and combo
        if (highscore < score)
        {
            highscore = score;
        }
        maxCombo = combo = score = 0;

        // Pause game
        Pause(false);

        // Reset key factors of level
        RefPlayer.Reset();
        RefBall.Reset();
        RefMap.Reset();
    }

    /// <summary>
    /// End level and proceed to gameover screen
    /// </summary>
    /// <param name="win">Win or lose</param>
    public void EndLevel(bool win)
    {
        // Set highscore
        if (highscore < score)
        {
            highscore = score;
        }

        Pause(true);
        if (win)
        {
            // Display win screen
            RefGameOverPanel.SetActive(true);
        }
        else
        {
            // Display game over screen
            RefGameOverPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Pauses the game
    /// </summary>
    /// <param name="pause">Indicate the desired state of the game, null for toggling</param>
    public void Pause(bool? pause = null)
    {
        if (pause == null)
        {
            // Invert pause
            if (TimeManager.IsPause())
            {
                TimeManager.Pause(false);
            }
            else
            {
                TimeManager.Pause(true);
            }
        }
        else
        {
            TimeManager.Pause(pause.GetValueOrDefault());
        }


        // Pause or resume gameobjects
        bool pauseState = TimeManager.IsPause(); // Current pause state

        // Pause player
        Pause p = RefPlayer.GetComponent<Pause>();
        if (p)
        {
            p.Toggle(pauseState);
        }

        p = RefBall.GetComponent<Pause>();
        if (p)
        {
            p.Toggle(pauseState);
        }
    }

    public void BallTouchFloor()
    {
        // Reset combo
        if (maxCombo < combo)
        {
            maxCombo = combo;
        }
        combo = 0;
    }

    private void refreshUI()
    {
        // Lives
        if (refLives != null)
        {
            for (int i = 0; i < refLives.Count; ++i)
            {
                var life = refLives[i];
                if (i < RefPlayer.Lives - 1) // Minus 1 as last life should not display anymore lives
                {
                    life.SetActive(true);
                }
                else
                {
                    life.SetActive(false);
                }
            }
        }

        // Bombs
        if (refBombs != null)
        {
            for (int i = 0; i < refBombs.Count; ++i)
            {
                var bomb = refBombs[i];
                if (i < RefPlayer.Bombs)
                {
                    bomb.SetActive(true);
                }
                else
                {
                    bomb.SetActive(false);
                }
            }
        }

        // Timer
        RefTimer.text = ((int)(overtimeTimer.CurrentTime * 10.0f)).ToString();

        // Scores and combos
        RefCurrentScore.text = score.ToString();
        RefHighScore.text = highscore.ToString();
        RefCombo.text = combo.ToString();
        RefMaxCombo.text = maxCombo.ToString();
    }

    private void checkEnd()
    {
        if (gameover)
        {
            return;
        }

        if (!RefPlayer.IsAlive())
        {
            gameover = true;
            win = false;
        }
        
        // Flag for gameover and win
        if (tileCount <= 0)
        {
            gameover = true;
            win = true;
        }
    }

    #region State check functions

    private bool isGameOver()
    {
        return RefGameOverPanel.activeSelf;
    }

    private bool shouldGameOver()
    {
        return gameover;
    }

    #endregion State check functions

    #region Score and Combos

    public void HitTile()
    {
        ++combo;
        score += InitialCardScore + (int)Mathf.Pow(combo - 1, 2) * 20;
    }

    #endregion Score and Combos
}
