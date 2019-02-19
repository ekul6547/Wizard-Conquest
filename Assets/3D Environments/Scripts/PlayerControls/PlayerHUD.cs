using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class PlayerHUD : MonoBehaviour {
    [System.Serializable]
    public class barSize
    {
        public string ID = "PartName";
        public RectTransform obj;
        public bool altSize = false;
        [Range(0, 1)]
        public float x;
        [Range(0, 1)]
        public float y;
        [Range(1f, 0)]
        public float width;
        [Range(1f, 0)]
        public float height;
        public bool TieToEscape = false;
    }
    LobbyTopPanel EscMenu;
    bool display = false;
    public RectTransform HUD;
    Vector3 bottomLeft;
    Vector2 HUDSize;
    Vector3 HUDScale;
    public EntityProperties prop;
    public barSize[] Parts;

    void Awake()
    {
    }

	void Start ()
    {
        GameObject l = GameObject.Find("LobbyManager");
        if(l != null)
            EscMenu = l.transform.Find("TopPanel").GetComponent<LobbyTopPanel>();
	}
    void PropData()
    {
        barSize HealthBar = findBarFromID("Health");
        barSize ManaBar = findBarFromID("Mana");
        float sca = 0.0f; ;
        if (HealthBar != null)
        {
            sca = HealthBar.obj.Find("Show").GetComponent<RectTransform>().localScale.x;
            if (prop.MaxHealth > 0)
            {
                sca = prop.health / prop.MaxHealth;
                if (sca < 0) { sca = 0; }
                if (sca > 1) { sca = 1; }
            }
            else { sca = 0; }
            HealthBar.obj.Find("Show").GetComponent<RectTransform>().localScale = new Vector3(sca, 1, 1);
            HealthBar.obj.Find("Text").GetComponent<Text>().text = "HP: " + prop.health.ToString() + "/" + prop.MaxHealth.ToString();
        }
        if (ManaBar != null)
        {
            sca = ManaBar.obj.Find("Show").GetComponent<RectTransform>().localScale.x;
            if (prop.MaxMana > 0)
            {
                sca = prop.Mana / prop.MaxMana;
                if (sca < 0) { sca = 0; }
                if (sca > 1) { sca = 1; }
            }
            else { sca = 0; }
            ManaBar.obj.Find("Show").GetComponent<RectTransform>().localScale = new Vector3(sca, 1, 1);
            ManaBar.obj.Find("Text").GetComponent<Text>().text = "MP: " + prop.Mana.ToString() + "/" + prop.MaxMana.ToString();
        }
    }
    public barSize findBarFromID(string ID)
    {
        barSize ret = null;
        foreach(barSize bar in Parts)
        {
            if(bar.ID == ID)
            {
                ret = bar;
            }
        }

        return ret;
    }
    void HudSizes()
    {
        HUDSize = new Vector2(HUD.rect.width, HUD.rect.height);
        HUDScale = HUD.localScale;
    }
    RectTransform parentSize(RectTransform r)
    {
        return r.parent.GetComponent<RectTransform>();
    }
    void Update () {
        HudSizes();
        foreach (barSize bar in Parts)
        {
            var p = parentSize(bar.obj);
            if (!bar.altSize)
            {
                bar.obj.anchoredPosition = new Vector2(p.rect.xMin + (bar.x * p.rect.width), p.rect.yMin + (bar.y * p.rect.height));
                bar.obj.sizeDelta = new Vector3(bar.width * p.rect.width*-1, bar.height * p.rect.height*-1, 1);
            }
            else
            {
                bar.obj.rect.Set(bar.x, bar.y, bar.width, bar.height);
            }
            if (bar.TieToEscape && EscMenu != null)
            {
                bar.obj.gameObject.SetActive(EscMenu.isDisplay());
            }
        }
        if (prop != null)
        {
            PropData();
        }
    }
}
