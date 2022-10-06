using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private string buttonHoverSound;

    [SerializeField]
    private string buttonPressSound;

    public void StartGame()
    {
        AudioManager.instance.PlaySound(buttonPressSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        AudioManager.instance.PlaySound(buttonPressSound);
        Debug.Log("MainMenu::QuitGame()");
        Application.Quit();
    }

    public void OnMouseHover()
    {
        Debug.Assert(AudioManager.instance != null, "NO AudioManager FOUND");
        Debug.Assert(buttonHoverSound != null, "NO hoverOverSound FOUND");
        AudioManager.instance.PlaySound(buttonHoverSound);
    }

}
