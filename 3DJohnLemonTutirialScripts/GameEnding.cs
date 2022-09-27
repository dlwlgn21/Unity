using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1.0f;
    public float displayImageDuration = 1.0f;
    public GameObject player;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    public CanvasGroup caughtBackgroundImageCanvasGroup;

    bool mIsPlayerCaught;
    bool mIsPlayerAtExit;

    float mTimer;

    void Update()
    {
        if (mIsPlayerAtExit)
        {
            EndLevel(exitBackgroundImageCanvasGroup, false);
        }
        else if (mIsPlayerCaught)
        {
            EndLevel(caughtBackgroundImageCanvasGroup, true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            mIsPlayerAtExit = true; 
        }
    }

    public void CaughtPlayer()
    {
        mIsPlayerCaught = true;
    }


    void EndLevel(CanvasGroup imageCanvasGroup, bool isRestart)
    {
        mTimer += Time.deltaTime;
        imageCanvasGroup.alpha = mTimer / fadeDuration;
        if (mTimer > fadeDuration + displayImageDuration)
        {
            if (isRestart)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                Application.Quit();
            }
        }
    }
}
