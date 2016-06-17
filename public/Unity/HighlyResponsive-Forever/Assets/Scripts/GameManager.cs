using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Timer")]
    public float TimeTillOvertime; // When bullets start raining down

    [Header("Object references")]
    public Player RefPlayer;
    public Ball RefBall;

    [Header("UI references")]
    public GameObject RefGameOverPanel;
    public GameObject RefLifePanel;
    public GameObject RefBombPanel;
    public Text RefHighScore;
    public Text RefCurrentScore;
    public Text RefTimer;

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Not gameover and player not alive, set game over
	    if (!RefGameOverPanel.activeSelf && !RefPlayer.IsAlive())
        {
            EndLevel(false);
        }

        if (RefPlayer.IsAlive() && Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle pause
            Pause();
        }
	}

    #region Buttons

    public void Button_Retry()
    {
        RefGameOverPanel.SetActive(false);
        Pause(false);
        RefPlayer.Reset();
        RefBall.Reset();
    }

    #endregion Buttons

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
}
