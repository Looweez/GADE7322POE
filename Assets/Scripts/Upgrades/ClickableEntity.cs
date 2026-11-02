using UnityEngine;

public class ClickableEntity : MonoBehaviour
{
    private UpgradableEntity upgradableEntity;

    void Awake()
    {
        upgradableEntity = GetComponent<UpgradableEntity>();
        if (upgradableEntity == null)
        {
            upgradableEntity = gameObject.AddComponent<UpgradableEntity>();
        }
    }

    void OnMouseDown()
    {
        // Prevent opening upgrade menu if clicking while placing units or game is paused
        if (Time.timeScale == 0f) return;

        if (UpgradeUI.Instance != null)
        {
            UpgradeUI.Instance.OpenPopup(upgradableEntity);
        }
    }
}
