using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [Header("Use Icons")]
    [SerializeField] Image WeaponMain;
    [SerializeField] Image WeaponSecondary;
    [SerializeField] Image SpellMain;
    [SerializeField] Image SpellSecondary;
    [SerializeField] Image Potion;
    
    [Space]
    [Header("HP Display")]
    [SerializeField] TextMeshProUGUI HPText;
    [SerializeField] UIBar HPBar;
    [SerializeField] Image HPIcon;
    [SerializeField] Sprite[] HPIcons;

    [Space]
    [Header("Stat Display")]
    [SerializeField] TextMeshProUGUI FavorText;
    [SerializeField] TextMeshProUGUI ProwessText;
    [SerializeField] TextMeshProUGUI PhysiqueText;
    [SerializeField] TextMeshProUGUI FinesseText;
    [SerializeField] TextMeshProUGUI CraftText;

    [Space]
    [Header("Overflow Display")]
    [SerializeField] Image OverflowImage;
    [SerializeField] UIBar OverflowBar;
    public OverflowOverlay overflowOverlay;
    [SerializeField] Color c1;
    [SerializeField] Color c2;

    [Space]
    [Header("Game Over Display")]
    [SerializeField] GameObject GOPanel;
    [SerializeField] Image GOPane;
    [SerializeField] Color GOc1;
    [SerializeField] Color GOc2;
    [SerializeField] float GOSequenceLength = 3f;
    [SerializeField] float FallSequenceLength = 3f;
    [SerializeField] float FallHeight = 1.5f;
    [SerializeField] float FallStartDelay = .5f;

    [Space]
    [Header("Popup Display")]
    [SerializeField] SlidingUIPanel infoDisplay;

    float GOTimer = 0;
    bool gameOver = false;
    float fallTarget;
    float fallStart;

    public GameObject collectableText;

    #region SINGLETON
    public static HUDManager instance;
    private void Awake()
    {
        if(instance == null) 
        {
            instance = this;
        }
    }
    #endregion

    void Start()
    {
        Cursor.visible = false;
        EventSystem.OnUpdateStats += UpdateStatDisplay;
        EventSystem.OnTakeDamage += UpdateHPDisplay;
    }

    private void OnDestroy()
    {
        EventSystem.OnUpdateStats -= UpdateStatDisplay;
        EventSystem.OnTakeDamage -= UpdateHPDisplay;
    }
    private void OnDisable()
    {
        EventSystem.OnUpdateStats -= UpdateStatDisplay;
        EventSystem.OnTakeDamage -= UpdateHPDisplay;
    }

    bool pause = false;
    void Update()
    {
        if (gameOver) 
        {
            GOPanel.SetActive(true);
            GOTimer += Time.deltaTime;
            GOPane.color = Color.Lerp(GOc1, GOc2, GOTimer / GOSequenceLength);

            Camera.main.transform.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, Mathf.LerpAngle(0, -90, GOTimer / FallSequenceLength));
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Mathf.Lerp(fallStart, fallTarget, (GOTimer - FallStartDelay) / FallSequenceLength),  Camera.main.transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause) 
            {
                UnpauseGame();
            }
        }
    }

    public void UpdateSpellDisplay(Spell main, Spell secondary) 
    {
        SpellMain.sprite = main.GetDisplaySprite();
        if(secondary!= null) 
        {
            SpellSecondary.enabled = true;
            SpellSecondary.sprite = secondary.GetDisplaySprite();
        } else 
        {
            SpellSecondary.enabled = false;
        }
        
    }

    public void UpdateWeaponDisplay(Weapon main, Weapon secondary)
    {
        WeaponMain.sprite = main.icon;
        WeaponSecondary.sprite = secondary.icon;

    }

    public void InitiateGameOver() 
    {
        gameOver = true;
        fallStart = Camera.main.transform.position.y;
        fallTarget = Camera.main.transform.position.y - FallHeight;
        Destroy(GameObject.Find("Weapon").GetComponent<SpriteRenderer>());
    }

    public void PauseGame() 
    {
        pause = true;
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
    }

    public void UnpauseGame()
    {
        pause = false;
        Time.timeScale = 1;
        infoDisplay.SlidePanelOut();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SendUIPanel(UIPanelContent content) 
    {
        PauseGame();
        infoDisplay.ChangePanelContent(content);
        infoDisplay.SlidePanelIn();
    }

    public void SendUIPanel(List<UIPanelContent> content)
    {
        PauseGame();
        infoDisplay.ChangePanelContent(content);
        infoDisplay.SlidePanelIn();
    }

    public void UpdateOverflowDisplay(float v) 
    {

        OverflowBar.SetValue(v);

        Color c = Color.Lerp(c1, c2, v);
        OverflowImage.color = c;

        overflowOverlay.SetOverflowAmount(v);
    }

    public void UpdateHPDisplay(Transform s, int v) 
    {
        float p = (float)Player.instance.CurrentHP / (float)Player.instance.stats.maxHP;

        if(p > .8f) 
        {
            HPIcon.sprite = HPIcons[0];
        } else if (p > .5f)
        {
            HPIcon.sprite = HPIcons[1];
        } else if (p > .25f)
        {
            HPIcon.sprite = HPIcons[2];
        } else if (p < .05f)
        {
            HPIcon.sprite = HPIcons[3];
        }

        HPBar.SetValue(p);

        HPText.text = Player.instance.CurrentHP.ToString() + "/" + Player.instance.stats.maxHP.ToString();
    }

    public void UpdateHPDisplay(int v) 
    {
        UpdateHPDisplay(null, v);
    }


    public void UpdateStatDisplay(PlayerStats ps, PlayerStats stats) 
    {
        FavorText.text = stats.favor.ToString();
        ProwessText.text = stats.prowess.ToString();
        PhysiqueText.text = stats.physique.ToString();
        FinesseText.text = stats.finesse.ToString();
        CraftText.text = stats.craft.ToString();


        UpdateHPDisplay(Player.instance.CurrentHP);
    }
}
