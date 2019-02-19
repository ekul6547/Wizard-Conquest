using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkIpView : MonoBehaviour {
    bool l = false;
    string i;
    public Rect rect = new Rect(100, 60, 80, 50);

    void Awake()
    {
        i = "[" + Network.player.ipAddress + "]";
    }

	void OnGUI()
    {
        if (!l)
        {
            rect = new Rect(Screen.width - rect.x, Screen.height - rect.y, rect.width, rect.height);
            l = true;
        }
        var textDimensions = GUI.skin.label.CalcSize(new GUIContent(i));
        rect.width = textDimensions.x+10;
        rect = GUI.Window(10,rect,DragWindow,"IP Address");
    }
    void DragWindow(int windowID)
    {
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        GUI.Label(new Rect(0, 23, 10000, 20), "  "+Network.player.ipAddress);
    }
    
}
