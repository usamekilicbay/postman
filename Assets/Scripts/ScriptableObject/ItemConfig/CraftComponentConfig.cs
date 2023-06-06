using UnityEngine;

namespace Merchant.Config.Item
{
    [CreateAssetMenu(fileName = "New CraftComponent Config", menuName = "Configs/Item/Craft Component Config")]
    public class CraftComponentConfig : ItemBaseConfig
    {
        [SerializeField] private ItemCardConfig[] requirerRecipes;
        public ItemCardConfig[] RequirerRecipes => requirerRecipes;
    }
}
