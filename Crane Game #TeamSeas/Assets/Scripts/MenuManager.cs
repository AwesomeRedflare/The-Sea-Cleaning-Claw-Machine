using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{
    public Animator transition;

    public Toggle postProToggle;
    public static bool hasPostPro;
    public PostProcessVolume postPro;

    public AudioMixer audioMixer;

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
    }

    public void ButtonSound()
    {
        FindObjectOfType<AudioManager>().Play("Button");
    }

    private void Start()
    {
        postPro.enabled = hasPostPro;
        postProToggle.isOn = hasPostPro;
    }

    public void StartButton()
    {
        FindObjectOfType<AudioManager>().Play("Start");
        Invoke("StartGame", 1f);
        Transition();
    }

    void Transition()
    {
        transition.SetTrigger("Close");
    }

    void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void PostProcessingOn(bool tog)
    {
        hasPostPro = tog;

        postPro.enabled = hasPostPro;
    }
}
