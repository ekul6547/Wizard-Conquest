using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkChatPlayer : NetworkBehaviour {

    public NetworkChat HUD;
    public InputField inputField;
    public LocalPlayerData player;
    public Transform ActiveArea;
    Text inputText;
    bool deactive;
    bool select = false;
    public Text ChatArea;
    float update = 0;

    public struct ChatMessage
    {
        public string playerName;
        public string message;
        public float fade;
        public ChatMessage(string Name, string Message)
        {
            playerName = Name;
            message = Message;
            fade = 255;
        }
    }
    public class SyncListChat : SyncListStruct<ChatMessage>
    {
    }
    [SyncVar]
    public SyncListChat MessageList = new SyncListChat();

    void Start()
    {
        player = GetComponent<LocalPlayerData>();
        GameObject HUDO = GameObject.FindGameObjectWithTag("HUD");
        if (HUDO != null)
        {
            HUD = HUDO.transform.Find("ChatPanel").GetComponent<NetworkChat>();
            ActiveArea = HUD.transform.Find("InGame").Find("InputField");
            inputField = ActiveArea.GetComponent<InputField>();
            inputText = inputField.transform.Find("Text").GetComponent<Text>();
            ChatArea = HUD.transform.Find("InGame").Find("TextArea").Find("Text").GetComponent<Text>();
            ActiveArea.gameObject.SetActive(false);
        }
    }
    [Command]
    void CmdAddMessage (string PlayerName, string Message)
    {
        HUD.SendMessagePacket(new ChatMessage(PlayerName,Message));
    }
    void Update()
    {
        if (!isLocalPlayer || SceneManager.GetActiveScene().name == "TestingWorld")
        {
            return;
        }
        if (deactive)
        {
            ActiveArea.gameObject.SetActive(false);
            deactive = false;
        }
        if (select)
        {
            if (!inputField.isFocused)
            {
                inputField.ActivateInputField();
            }
            if (inputField.isFocused)
            {
                select = false;
            }
        }
        if (Input.GetButtonDown("Submit"))
        {
            if(inputText.text != "" && ActiveArea.gameObject.activeSelf)
            {
                CmdAddMessage(player.PlayerName, inputText.text);
                inputField.text = "";
                deactive = true;
                select = false;
            }
            else
            {
                ActiveArea.gameObject.SetActive(true);
                inputField.text = "";
                inputField.ActivateInputField();
                select = true;

                var c = inputField.textComponent.color;
                c.a = 1;
                inputField.textComponent.color = c;
            }
        }
    }
    void LateUpdate()
    {
        if (!isLocalPlayer || SceneManager.GetActiveScene().name == "TestingWorld")
        {
            return;
        }
        string displayString = "";
        bool first = true;
        int l = 10;
        for (int i = 0; i < l; i++)
        {
            string s = "";
            if (i < MessageList.Count)
            {
                ChatMessage c = MessageList[MessageList.Count - i - 1];
                s = "<" + c.playerName + ">: " + c.message;
            }
            if (first)
            {
                displayString += s;
                first = false;
            }
            else
            {
                displayString = s + "\n" + displayString;
            }
        }
        ChatArea.text = displayString;
        if (update > 0.0000f)
        {
            var c = ChatArea.color;
            c.a = update;
            ChatArea.color = c;
            update -= 1 * Time.deltaTime;
        }
    }
}
