using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private string buttonHoverSound;

    [SerializeField]
    private string buttonPressSound;
    public void Quit()
    {
        AudioManager.instance.PlaySound(buttonPressSound);
        Application.Quit();
    }

    public void Retry()
    {
        AudioManager.instance.PlaySound(buttonPressSound);
        GameMaster.gm.IsGameover = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void OnMouseHover()
    {
        Debug.Assert(AudioManager.instance != null, "NO AudioManager FOUND");
        Debug.Assert(buttonHoverSound != null, "NO hoverOverSound FOUND");
        AudioManager.instance.PlaySound(buttonHoverSound);
    }
}
