using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkPlayerProperties : NetworkBehaviour {


    public bool l = false;
    public string i;
    public string a;
    public string pName = "InsertName";
    public Rect rect = new Rect(30, 60, 300, 50);

    void Awake()
    {
        i = "[" + pName + "]";
        a = "[Player Name]";
    }

    void OnGUI()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (!l)
        {
            rect = new Rect(rect.x, Screen.height - rect.y, rect.width, rect.height);
            l = true;
        }
        var txtDimI = GUI.skin.label.CalcSize(new GUIContent(i));
        var txtDimA = GUI.skin.label.CalcSize(new GUIContent(a));
        //if(txtDimA.x > txtDimI.x) { rect.width = txtDimA.x + 10; } else { rect.width = txtDimI.x + 10; }
        rect = GUI.Window(11, rect, DragWindow, "Player Name");
    }
    void DragWindow(int windowID)
    {
        GUI.DragWindow(new Rect(0, 0, 10000, 20));
        pName = GUI.TextField(new Rect(0, 23, 200, 23), pName);
        if (GUI.Button(new Rect(200, 23, 100, 23), "Set Name"))
        {
            CmdSetName(pName);
        }
    }
    [Command]
    void CmdSetName(string name)
    {
        pName = name;
    }
}
