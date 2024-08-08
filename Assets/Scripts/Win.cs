using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : BasePopup
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnExitGameButton()
    {
        Debug.Log("Exiting Game");
        Application.Quit();
    }

    public void OnRestartGameButton()
    {
        Messenger.Broadcast(GameEvent.RESTART_GAME);
    }
}
