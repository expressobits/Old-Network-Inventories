using UnityEngine;

namespace ExpressoBits.Inventory
{
    [CreateAssetMenu(fileName = "Category", menuName = "Expresso Bits/Inventory/Category")]
    public class Category : ScriptableObject
    {
        public Color Color => color;

        [SerializeField] private Color color = Color.white;
    }
}

