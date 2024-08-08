using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{

    private int value = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 rotation = Vector3.up * 180 * Time.deltaTime;
        //transform.Rotate(rotation, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Messenger<int>.Broadcast(GameEvent.PICKUP_COIN, value);
            Destroy(this.gameObject);
        }
    }
}
