using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using System;

public class GameManager : MonoBehaviour
{
    public Animator transition;
    public Animator camAn;

    public PostProcessVolume postPro;

    public Claw claw;

    private bool stopWatchActive = true;
    private  float currentTime;
    public Text resultTime;

    private void Start()
    {
        postPro.enabled = MenuManager.hasPostPro;
    }

    private void Update()
    {
        if (stopWatchActive == true)
        {
            currentTime = currentTime + Time.deltaTime;
        }

        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        resultTime.text = "You cleared all the trash in " + time.ToString(@"mm\:ss\:ff");
    }

    void Transition()
    {
        transition.SetTrigger("Close");
    }

    public void RetryButton()
    {
        FindObjectOfType<AudioManager>().Play("Start");
        Invoke("RetryGame", 1f);
        Transition();
    }

    void RetryGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void MenuButton()
    {
        FindObjectOfType<AudioManager>().Play("Button");
        Invoke("GoToMenu", 1f);
        Transition();
    }

    void GoToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void WinScreen()
    {
        FindObjectOfType<AudioManager>().Play("Win");
        stopWatchActive = false;
        claw.canMove = false;
        camAn.SetTrigger("Win");
    }
}
