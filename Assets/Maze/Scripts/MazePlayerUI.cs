using HappyFunTimes;
using UnityEngine;
using System.Collections;
using System;

public class MazePlayerUI : MonoBehaviour {

    public Transform nameTransform;

    // this is the base color of the avatar.
    // we need to know it because we need to know what color
    // the avatar will become after its hsv has been adjusted
    // so we can color the controller and the names above
    // the player.
    public Color baseColor;

    private Rigidbody2D m_rigidbody2d;
    private Material m_material;
    private HFTGamepad m_gamepad;
    private GUIStyle m_guiStyle = new GUIStyle();
    private GUIContent m_guiName = new GUIContent("");
    private Rect m_nameRect = new Rect(0,0,0,0);
    private string m_playerName;
	private NetPlayer m_netPlayer;

    public PlayerScore score = new PlayerScore();
    
    public bool visible
    {
        set
        {
            visibility.visible = value;
        }
        get
        {
            return visibility.visible;
        }
    }

    private PlayerVisibility visibility;

    // Use this for initialization
    void Start ()
    {
        m_rigidbody2d = GetComponent<Rigidbody2D>();
        m_material = GetComponent<Renderer>().material;
        m_gamepad = GetComponent<HFTGamepad>();

        visibility = gameObject.GetComponent<PlayerVisibility>();

        int playerNumber = PlayerManager.AddPlayer(this);
        SetColor(playerNumber-1);
        score.chasing = (playerNumber % 2 == 0);
		m_netPlayer.SendCmd ("customText", new CustomTextParcel (score.chasing ? "Chase!" : "Hide!"));
		m_netPlayer.SendCmd ("showGif", new CustomTextParcel("Tom is lazy"));
        SetName(m_gamepad.Name);

        // Notify us if the name changes.
        m_gamepad.OnNameChange += ChangeName;

        score.catchPlayerEvent += delegate() {
            visibility.StartBlinking();
            //refreshName();
        };
        score.caughtEvent += delegate ()
        {
            visibility.PermanentOn();
            //refreshName();
        };
        if (score.chasing)
        {
            visibility.PermanentOn();
        }
        else
        {
            visibility.StartBlinking();
        }

        //refreshName();
    }

    void Update()
    {
        score.Update();
        Action refreshName = delegate ()
        {
            SetName(score.ToString());
        };
        refreshName();
    }

    void SetName(string name)
    {
        m_playerName = name;
        gameObject.name = "Player-" + m_playerName;
        m_guiName = new GUIContent(m_playerName);
        Vector2 size = m_guiStyle.CalcSize(m_guiName);
        m_nameRect.width  = size.x + 12;
        m_nameRect.height = size.y + 5;
    }

    void SetColor(int colorNdx) {
        // Pick a color
        float hueAdjust = (((colorNdx & 0x01) << 5) |
                           ((colorNdx & 0x02) << 3) |
                           ((colorNdx & 0x04) << 1) |
                           ((colorNdx & 0x08) >> 1) |
                           ((colorNdx & 0x10) >> 3) |
                           ((colorNdx & 0x20) >> 5)) / 64.0f;
        float valueAdjust = (colorNdx & 0x20) != 0 ? -0.5f : 0.0f;
        float satAdjust   = (colorNdx & 0x10) != 0 ? -0.5f : 0.0f;

        // get the hsva for the baseColor
        Vector4 hsva = HFTColorUtils.ColorToHSVA(baseColor);

        // adjust that base color by the amount we picked
        hsva.x += hueAdjust;
        hsva.y += satAdjust;
        hsva.z += valueAdjust;

        // now get the adjusted color.
        Color playerColor = HFTColorUtils.HSVAToColor(hsva);

        // Tell the gamepad to change color
        m_gamepad.Color = playerColor;

        // Create a 1 pixel texture for the OnGUI code to draw the label
        Color[] pix = new Color[1];
        pix[0] = playerColor;
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixels(pix);
        tex.Apply();
        m_guiStyle.normal.background = tex;

        // Set the HSVA material of the character to the color adjustments.
        m_material.SetVector("_HSVAAdjust", new Vector4(hueAdjust, satAdjust, valueAdjust, 0.0f));
    }

    void OnGUI()
    {
        if (!visible)
        {
            return;
        }
        // If someone knows a better way to do
        // names in Unity3D please tell me!
        Vector2 size = m_guiStyle.CalcSize(m_guiName);
        Vector3 coords = Camera.main.WorldToScreenPoint(nameTransform.position);
        m_nameRect.x = coords.x - size.x * 0.5f - 5f;
        m_nameRect.y = Screen.height - coords.y;
        m_guiStyle.normal.textColor = Color.white;
        m_guiStyle.contentOffset = new Vector2(4, 2);
        GUI.Box(m_nameRect, m_playerName, m_guiStyle);
    }

    // Called when the user changes their name.
    void ChangeName(object sender, System.EventArgs e)
    {
        SetName(m_gamepad.Name);
    }
	private class CustomTextParcel:MessageCmdData {
		public CustomTextParcel(String _text)
		{
			text=_text;
		}
		public string text;
	}
	void InitializeNetPlayer(SpawnInfo spawnInfo)
	{
		m_netPlayer = spawnInfo.netPlayer;
		score.caughtEvent += delegate (){spawnInfo.netPlayer.SendCmd ("customText", new CustomTextParcel(score.chasing ? "Seeker" : "Zach"));};
		score.catchPlayerEvent += delegate (){spawnInfo.netPlayer.SendCmd ("customText", new CustomTextParcel(score.chasing ? "Seeker" : "Zach"));};


	}
}
