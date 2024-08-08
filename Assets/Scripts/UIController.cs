using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI scoreValue;
    [SerializeField] private TextMeshProUGUI lives;
    private int livesValue = 3;
    private int score = 0;
    private bool dead = false;
    [SerializeField] private Options optionsPopup;
    [SerializeField] private Gameover gameOverPopup;
    [SerializeField] private Win winPopup;

    private int popupsActive = 0;

    private void Awake()
    {
        Messenger.AddListener(GameEvent.TAKE_LIFE, OnTakeLife);
        Messenger<int>.AddListener(GameEvent.PICKUP_COIN, OnPickupCoin);
        Messenger.AddListener(GameEvent.CHEST_OPEN, OnChestOpen);
        Messenger.AddListener(GameEvent.POPUP_OPENED, OnPopupOpened);
        Messenger.AddListener(GameEvent.POPUP_CLOSED, OnPopupClosed);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.TAKE_LIFE, OnTakeLife);
        Messenger<int>.RemoveListener(GameEvent.PICKUP_COIN, OnPickupCoin);
        Messenger.RemoveListener(GameEvent.CHEST_OPEN, OnChestOpen);
        Messenger.RemoveListener(GameEvent.POPUP_OPENED, OnPopupOpened);
        Messenger.RemoveListener(GameEvent.POPUP_CLOSED, OnPopupClosed);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetGameActive(true);
        lives.text = livesValue.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (livesValue < 0 && popupsActive == 0)
        {
            gameOverPopup.Open();
            popupsActive += 1;
        }

        if (popupsActive > 0)
        {
            SetGameActive(false);

        }

        if (Input.GetKeyDown(KeyCode.Escape) && popupsActive == 0)
        {
            optionsPopup.Open();
        }

        if (score == 31 && popupsActive == 0)
        {
            winPopup.Open();
        }
    }

    private void OnTakeLife()
    {
        if (dead == false && livesValue > 0)
        {
            livesValue--;
            lives.text = livesValue.ToString();
            dead = true;
        }
        else if (livesValue == 0)
        {
            livesValue--;
            dead = true;
        }
        StartCoroutine(isDeadCoroutine());
    }

    private void OnPickupCoin(int value)
    {
        score++;
        scoreValue.text = score.ToString();
    }

    private void OnChestOpen()
    {
        score += 3;
        scoreValue.text = score.ToString();
    }

    private IEnumerator isDeadCoroutine()
    {
        yield return new WaitForSeconds(2);
        dead = false;
    }

    public void SetGameActive(bool active)
    {
        if (active)
        {
            Time.timeScale = 1; // unpause the game
        }
        else
        {
            Time.timeScale = 0; // pause the game
        }
    }

    private void OnPopupOpened()
    {
        if (popupsActive == 0)
        {
            SetGameActive(false);
        }

        popupsActive += 1;
    }

    private void OnPopupClosed()
    {
        popupsActive -= 1;

        if (popupsActive == 0)
        {
            SetGameActive(true);
        }
    }
}
