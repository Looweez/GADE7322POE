using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI Instance;

    [Header("UI Panel & Texts")]
    public GameObject popupPanel;
    public TMP_Text titleText;
    public TMP_Text statsText;
    public TMP_Text costText;
    public Button upgradeButton;
    public Button closeButton;

    private UpgradableEntity activeEntity;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);

        if (upgradeButton != null)
            upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);

        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePopup);
    }

    public void OpenPopup(UpgradableEntity entity)
    {
        activeEntity = entity;
        if (popupPanel != null)
            popupPanel.SetActive(true);

        UpdatePopupContent();
    }

    public void ClosePopup()
    {
        activeEntity = null;
        if (popupPanel != null)
            popupPanel.SetActive(false);
    }

    public void UpdatePopupContent()
    {
        if (activeEntity == null) return;

        string entityName = activeEntity.gameObject.name.Replace("(Clone)", "").Replace("Tower", "Main Tower");
        titleText.text = $"{entityName}\nLevel {activeEntity.currentLevel} / {activeEntity.maxLevel}";

        if (activeEntity.currentLevel >= activeEntity.maxLevel)
        {
            statsText.text = "Maximum Level Reached!";
            costText.text = "MAX";
            upgradeButton.gameObject.SetActive(false);
            return;
        }

        upgradeButton.gameObject.SetActive(true);
        int cost = activeEntity.GetUpgradeCost();
        costText.text = $"Upgrade Cost: {cost} Sugarcubes";

        // Generate stat text with (+boost) preview preview
        string statsString = "";
        if (activeEntity.GetComponent<TowerHealth>() != null)
        {
            float curHealth;
            float boost = activeEntity.GetNextHealthPreview(out curHealth);
            statsString = $"Max Health: {curHealth} <color=green>(+{boost})</color>";
        }
        else
        {
            float curDmg;
            float boost = activeEntity.GetNextDamagePreview(out curDmg);
            statsString = $"Attack Damage: {curDmg} <color=green>(+{boost})</color>";
        }

        statsText.text = statsString;
    }

    private void OnUpgradeButtonClicked()
    {
        if (activeEntity != null)
        {
            activeEntity.UpgradeUnit();
            UpdatePopupContent(); // Refresh UI to show new stats and next cost
        }
    }
}
