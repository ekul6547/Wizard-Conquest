using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkChat : NetworkBehaviour
{
    GameObject[] clients;

    public void SendMessagePacket(NetworkChatPlayer.ChatMessage msg)
    {
        clients = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject c in clients)
        {
            c.GetComponent<NetworkChatPlayer>().MessageList.Add(msg);
        }
    }
}
