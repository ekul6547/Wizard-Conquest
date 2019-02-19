using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LocalPlayerData : NetworkBehaviour {

    public NetworkPlayerProperties PlayerData;

    [SyncVar]
    public string PlayerName = "player";
    public Color col;
    private GameObject cam;
    private EntityProperties prop;
    public Canvas Data;
    Text nameSpace;
    RectTransform HealthArea;
    RectTransform HealthBar;
    movementControls Move;
    CameraOrbitControls camControl;
    PlayerHUD HUD;

    Slider XS;
    Slider YS;
    Slider RS;

    void Start()
    {
        nameSpace = Data.transform.Find("DisplayName").GetComponent<Text>();
        HealthArea = Data.transform.Find("HealthBack").GetComponent<RectTransform>();
        HealthBar = HealthArea.Find("HealthFront").GetComponent<RectTransform>();
        prop = GetComponent<EntityProperties>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        GameObject HUDO = GameObject.FindGameObjectWithTag("HUD");
        if (HUDO != null)
            HUD = HUDO.GetComponent<PlayerHUD>();
        PlayerData = gameObject.GetComponent<NetworkPlayerProperties>();
        Move = gameObject.GetComponent<movementControls>();
        camControl = gameObject.GetComponent<CameraOrbitControls>();
        if (HUD != null)
        {
            XS = HUD.findBarFromID("XSens").obj.transform.Find("XSens").GetComponent<Slider>();
            YS = HUD.findBarFromID("YSens").obj.transform.Find("YSens").GetComponent<Slider>();
            RS = HUD.findBarFromID("XSens").obj.transform.Find("XSens").GetComponent<Slider>();
            XS.value = camControl.sensitivityX;
            YS.value = camControl.sensitivityY;
            RS.value = Move.rotationSpeed;
        }
        GetName();
    }
    void Update()
    {
        if (!isLocalPlayer)
        {
            if (Data != null)
            {
                Data.transform.rotation = Quaternion.LookRotation(Data.transform.position - cam.transform.position);
            }
            if (nameSpace != null)
            {
                GetName();
                nameSpace.text = PlayerName;
                nameSpace.color = TeamControl.getTeamById(prop.team).Colour.color;
            }
            if (HealthArea != null && HealthBar != null)
            {
                Vector3 sca = HealthBar.localScale;
                sca.x = GetHealthPercentage();
                if (sca.x < 0) { sca.x = 0; }
                if (sca.x > 1) { sca.x = 1; }
                HealthBar.localScale = sca;
                var c = HealthBar.gameObject.GetComponent<Image>().color;
                c.a = 255;
                HealthBar.gameObject.GetComponent<Image>().color = c;
                c = HealthArea.gameObject.GetComponent<Image>().color;
                c.a = 255;
                HealthArea.gameObject.GetComponent<Image>().color = c;
            }
        }
        if (nameSpace != null)
        {
            GetName();
            nameSpace.text = PlayerName;
        }
        if(HUD != null)
        {
            camControl.sensitivityX = XS.value;
            camControl.sensitivityY = YS.value;
            Move.rotationSpeed = RS.value;
        }
    }
    /*public bool l = false;
    public Rect rect = new Rect(150, 60, 130, 155);
    void OnGUI()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (!l)
        {
            rect = new Rect(Screen.width - rect.x, rect.y, rect.width, rect.height);
            l = true;
        }
        rect = GUI.Window(206, rect, DragWindow, "Mouse Sensitivity");
    }
    void DragWindow(int windowID)
    {
        GUI.DragWindow(new Rect(0, 0, 10000, 30));
        GUI.Label(new Rect(15, 20, 100, 115), "X Sensitivity");
        camControl.sensitivityX = GUI.HorizontalSlider(new Rect(15, 40, 100, 20), camControl.sensitivityX, 0, 5);
        GUI.Label(new Rect(15,50, 100, 115), "Y Sensitivity");
        camControl.sensitivityY = GUI.HorizontalSlider(new Rect(15, 70, 100, 10), camControl.sensitivityY, 0, 5);
        GUI.Label(new Rect(15, 80, 100, 115), "Rotation Speed");
        Move.rotationSpeed = GUI.HorizontalSlider(new Rect(15, 100, 100, 10), Move.rotationSpeed, 0, 10);
    }*/
    void GetName()
    {
        if (PlayerData != null)
        {
            PlayerName = PlayerData.pName;
        }
    }

    float GetHealthPercentage()
    {
        return prop.health / prop.MaxHealth;
    }
}
