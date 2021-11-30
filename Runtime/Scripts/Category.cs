using UnityEngine;

namespace ExpressoBits.Inventory
{
    [CreateAssetMenu(fileName = "Category", menuName = "Expresso Bits/Inventory/Category")]
    public class Category : ScriptableObject
    {
        public Color Color => color;
        public Sprite Icon => icon;

        [SerializeField] private Color color = Color.white;
        [SerializeField] private Sprite icon;
    }
}

