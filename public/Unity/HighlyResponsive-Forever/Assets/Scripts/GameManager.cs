using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [Header("Timer")]
    public float TimeTillOvertime; // When bullets start raining down

    [Header("Data caps")]
    [Tooltip("Number of digits for score")]
    public float ScoreCap;
    [Tooltip("Number of digits for combo")]
    public float ComboCap;

    [Header("Object references")]
    public Player RefPlayer;
    public Ball RefBall;

    [Header("UI references")]
    public GameObject RefGameOverPanel;
    public GameObject RefLifePanel;
    public GameObject RefBombPanel;
    public Text RefHighScore;
    public Text RefCurrentScore;
    public Text RefMaxCombo;
    public Text RefCombo;
    public Text RefTimer;
    
    private List<GameObject> refLives = new List<GameObject>();
    private List<GameObject> refBombs = new List<GameObject>();
    private CountdownTimer overtimeTimer;

    // Use this for initialization
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        // Not gameover and should change to gameover, set game over
        if (!isGameOver() && shouldGameOver())
        {
            EndLevel(false);
        }

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
        Pause(false);
        RefPlayer.Reset();
        RefBall.Reset();
    }

    /// <summary>
    /// End level and proceed to gameover screen
    /// </summary>
    /// <param name="win">Win or lose</param>
    public void EndLevel(bool win)
    {
        Pause(true);
        if (win)
        {
            // Display win screen
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
    }

    #region State check functions

    private bool isGameOver()
    {
        return RefGameOverPanel.activeSelf;
    }

    private bool shouldGameOver()
    {
        return !RefPlayer.IsAlive();
    }

    private bool shouldGameOver(ref bool win)
    {
        win = false;
        if (!RefPlayer.IsAlive())
        {
            // Check if all tiles or boss is cleared
        }

        return shouldGameOver();
    }

    #endregion State check functions
}
