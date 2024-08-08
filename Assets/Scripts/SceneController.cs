using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] PlayerController player;

    private void Awake()
    {
        Messenger.AddListener(GameEvent.RESTART_GAME, OnRestartGame);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.RESTART_GAME, OnRestartGame);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame //
    void Update()
    {
        if (player.transform.position.y < - 7 )
        {
            Messenger.Broadcast(GameEvent.FELL_OFF);
            player.transform.position = new Vector2(player.transform.position.x, -7);
            
        }
    }

    public void OnRestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
