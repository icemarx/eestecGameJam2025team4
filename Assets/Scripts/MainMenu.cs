using UnityEngine;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{

    public AudioMixer mixer;


    public static float LinearToDB(float linearValue)
    {
        if (linearValue <= 0f)
        {
            return -80f; // Return a very low value for anything <= 0 to avoid math errors (this is typical for audio)
        }
        return 20f * Mathf.Log10(linearValue);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void UpdateMusicVolume(float value)
    {
        mixer.SetFloat("musicVol", LinearToDB(value));
    }


    public void UpdateSFXVolume(float value)
    {
        mixer.SetFloat("sfxVol", LinearToDB(value));
    }


    public void StartGame()
    {
        // TODO: change scene to main game
        return;
    }

}