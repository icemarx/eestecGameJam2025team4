using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Canvas upgradeShop;
    public Canvas pauseMenu;

    public AudioMixer mixer;


    public void DisplayPauseMenu()
    {
        pauseMenu.gameObject.SetActive(true);
    }


    public void HidePauseMenu()
    {
        pauseMenu.gameObject.SetActive(false);
    }

    public void TogglePaused()
    {
        GameManager.Instance.TogglePaused();
    }


    public void DisplayUpgradeMenu()
    {
        upgradeShop.gameObject.SetActive(true);
    }


    public void HideUpgradeMenu()
    {
        upgradeShop.gameObject.SetActive(false);
        GameManager.ChangeState(GameManager.GameState.WaveRunning);
    }
    
    public void UpdateMusicVolume(float value)
    {
        mixer.SetFloat("musicVol", LinearToDB(value));
    }


    public void UpdateSFXVolume(float value)
    {
        mixer.SetFloat("sfxVol", LinearToDB(value));
    }


    public static float LinearToDB(float linearValue)
    {
        if (linearValue <= 0f)
        {
            return -80f; // Return a very low value for anything <= 0 to avoid math errors (this is typical for audio)
        }
        return 20f * Mathf.Log10(linearValue);
    }


    public void RestartGame()
    {
        // TODO: change scene to main game
        GameManager.Instance.TogglePaused();
        GameManager.Instance.ResetGame();
        SceneManager.LoadScene("MainScene");
    }

}
