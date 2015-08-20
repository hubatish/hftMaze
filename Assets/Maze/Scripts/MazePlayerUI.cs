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

    // Current color of player & phone
    public Color color
    {
        get
        {
            return m_gamepad.Color;
        }
    }

    private Rigidbody2D m_rigidbody2d;
    private Material m_material;
    private HFTGamepad m_gamepad;
    private GUIStyle m_guiStyle = new GUIStyle();
    private GUIContent m_guiName = new GUIContent("");
    private Rect m_nameRect = new Rect(0,0,0,0);
    public string playerName;
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

        PlayerManager.AddPlayer(this);
        SetChasing(PlayerManager.ShouldISeek());
        int playerNumber = PlayerManager.NumberPlayers;
        SetColor(playerNumber-1);
        SetName(m_gamepad.Name);

        // Notify us if the name changes.
        m_gamepad.OnNameChange += ChangeName;

        score.catchPlayerEvent += delegate() {
            //visibility.StartBlinking();
            //refreshName();
            m_netPlayer.SendCmd("showGif", new CustomTextParcel("Tom is lazy"));
        };
        score.caughtEvent += delegate ()
        {
            //visibility.PermanentOn();
            m_netPlayer.SendCmd("showGif", new CustomTextParcel("Tom is lazy"));
            m_netPlayer.SendCmd("customText", new CustomTextParcel("You've been caught! Wait till next round."));
            gameObject.AddComponent<PlayerStartScreen>();

            PlayerManager.NumberCaught++;

            //See if the game ends!
            if (PlayerManager.NumberHiding == PlayerManager.NumberCaught)
            {
                RoundManager.Instance.EndGame(true); //seekers win
            }
        };

        //refreshName();
    }

    protected bool alreadyDecreased = false;

    public void SetChasing(bool chasing)
    {
        score.chasing = chasing;
        m_netPlayer.SendCmd("customText", new CustomTextParcel(GetPhoneChasingText()));

        if (score.chasing)
        {
            visibility.PermanentOn();
        }
        else
        {
            visibility.StartBlinking();
        }
    }

    public void Reset()
    {
        score.score = 0;
        PlayerManager.CeaseSeeking(this);
        alreadyDecreased = false;
        SetChasing(PlayerManager.ShouldISeek());
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
        playerName = name;
        gameObject.name = "Player-" + playerName;
        m_guiName = new GUIContent(playerName);
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
        GUI.Box(m_nameRect, playerName, m_guiStyle);
    }

    void OnDestroy()
    {
        PlayerManager.RemovePlayer(this);
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

    protected string GetPhoneChasingText()
    {
        return score.chasing ? "Seeker. Chase the birds!" : "Hider. Eat the fish!";
    }

	void InitializeNetPlayer(SpawnInfo spawnInfo)
	{
		m_netPlayer = spawnInfo.netPlayer;
		score.caughtEvent += delegate (){spawnInfo.netPlayer.SendCmd ("customText", new CustomTextParcel(GetPhoneChasingText()));};
		score.catchPlayerEvent += delegate (){spawnInfo.netPlayer.SendCmd ("customText", new CustomTextParcel(GetPhoneChasingText()));};
	}
}
