using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangeArea : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "MainArea")
        {
            Vector3 playerPosition = new Vector3(-1.12f, 1.19f, -19);
            player.transform.position = playerPosition;
        }
    }
}
