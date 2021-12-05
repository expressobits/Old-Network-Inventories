using UnityEngine;
using UnityEngine.UI;

namespace ExpressoBits.Inventory
{
    public class RequiredItemUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Text amount;

        public void Set(RequiredItem requiredItem)
        {
            icon.sprite = requiredItem.Item.Icon;
            amount.text = requiredItem.Amount.ToString();
        }
    }
}

