using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private bool isOpen = false;
    // Start is called before the first frame update //
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && isOpen == false)
        {
            isOpen = true;
            anim.SetTrigger("openChest");
            Messenger.Broadcast(GameEvent.CHEST_OPEN);
        }
    }
}
