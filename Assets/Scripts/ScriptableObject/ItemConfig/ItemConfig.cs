using UnityEngine;

namespace Merchant.Config.Item
{
    [CreateAssetMenu(fileName = "New Item Config", menuName = "Configs/Item/Item Config")]
    public class ItemConfig : ItemBaseConfig
    {
        [SerializeField] private CraftComponentConfig[] requiredComponents;
        public CraftComponentConfig[] RequiredComponents => requiredComponents;
    }
}
