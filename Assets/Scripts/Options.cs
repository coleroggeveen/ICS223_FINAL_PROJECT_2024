using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : BasePopup
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.enabled == true && Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
        
    }

    public void OnExitGameButton()
    {
        Debug.Log("Exiting Game");
        Application.Quit();
    }

    public void OnResumeGameButton()
    {
        Debug.Log("Resuming Game");
        Close();
    }
}
