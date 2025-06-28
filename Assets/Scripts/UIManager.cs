using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas pauseMenu;
    public AudioMixer mixer;

    public Canvas upgradeShop;
    public TMP_Text healthText;
    public TMP_Text wealthText;

    public Button[] buttons;

    private void Start()
    {
        buttons[0].onClick.AddListener(() => HealButton());
        buttons[1].onClick.AddListener(() => GameManager.Instance.upgradeManager.PurchaseUpgrade(1));
        buttons[2].onClick.AddListener(() => GameManager.Instance.upgradeManager.PurchaseUpgrade(2));
        buttons[3].onClick.AddListener(() => GameManager.Instance.upgradeManager.PurchaseUpgrade(3));
        buttons[4].onClick.AddListener(() => GameManager.Instance.upgradeManager.PurchaseUpgrade(4));
        buttons[5].onClick.AddListener(() => GameManager.Instance.upgradeManager.PurchaseUpgrade(5));
    }


    /**
     * PAUSE MENU
     * */

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


    /**
     * UPGRADE MENU
     * */

    public void DisplayUpgradeMenu()
    {
        upgradeShop.gameObject.SetActive(true);
        UpdateHealthText(GameManager.curHP, GameManager.maxHP);
        UpdateWealthText(GameManager.Instance.wealth);
        UpdateButtons();
    }

    public void HideUpgradeMenu()
    {
        upgradeShop.gameObject.SetActive(false);
        GameManager.ChangeState(GameManager.GameState.WaveRunning);
    }

    public void UpdateHealthText(int current, int max)
    {
        healthText.text = "Health: " + current + "/" + max;
    }

    public void UpdateWealthText(int wealth)
    {
        wealthText.text = "Wealth: " + wealth;
    }

    public void UpdateButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            // Debug.Log("BUTTON " + i);
            Button b = buttons[i];
            UpgradeManager.Upgrade u = GameManager.Instance.upgradeManager.GetLowestRankingAvailableID(i);

            if (u == null)
            {
                b.GetComponentInChildren<TMP_Text>().text = "ALREADY PURCHASED!";
                b.interactable = false;
            } else
            {
                if (u.rank == -1)
                {
                    b.GetComponentInChildren<TMP_Text>().text = u.title + "\nBUY: " + u.cost + " wealth";
                    b.interactable = u.cost <= GameManager.Instance.wealth || GameManager.curHP == GameManager.maxHP;
                    // b.onClick.AddListener(() => HealButton());
                } else
                {
                    b.GetComponentInChildren<TMP_Text>().text = u.title + "\nRANK: " + u.rank + "\nBUY: " + u.cost + " wealth";
                    b.interactable = u.cost <= GameManager.Instance.wealth;
                    // b.onClick.AddListener(() => GameManager.Instance.upgradeManager.PurchaseUpgrade(u.id, u.rank));
                }
            }
        }
    }

    public void HealButton()
    {
        GameManager.Instance.Heal();
    }
}
