using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Object references")]
    public Player RefPlayer;
    public Ball RefBall;


    [Header("UI references")]
    public GameObject RefGameOverPanel;

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
	}

    #region Buttons

    public void Button_Retry()
    {
        RefGameOverPanel.SetActive(false);
        RefPlayer.Reset();
        RefBall.Reset();
    }

    #endregion Buttons

    public void EndLevel(bool win)
    {
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
    }
}
