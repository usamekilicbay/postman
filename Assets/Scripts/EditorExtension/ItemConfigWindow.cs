using Merchant.Config.Item;
using System;
using UnityEditor;
using UnityEngine;

namespace Merchant.EditorExtension
{
    public class ItemConfigWindow : EditorWindow
    {
        private ItemBaseConfig _itemConfig;
        private string _itemName;

        [MenuItem("Window/Item Config Editor")]
        public static void OpenWindow()
        {
            var window = GetWindow<ItemConfigWindow>();
            window.titleContent = new GUIContent("Item Config Editor");
            window.Show();
        }

        public static void OpenWindow(ItemBaseConfig itemConfig)
        {
            var window = GetWindow<ItemConfigWindow>();
            window.titleContent = new GUIContent("Item Config Editor");
            window._itemConfig = itemConfig;
            window._itemName = itemConfig.Name;
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Item Config Editor", EditorStyles.boldLabel);

            EditorGUILayout.Space();

            _itemName = EditorGUILayout.TextField("Item Name", _itemName);

            EditorGUILayout.Space();

            if (GUILayout.Button("Load Item Config"))
            {
                _itemConfig = FindItemConfigByName(_itemName);
            }

            EditorGUILayout.Space();

            if (_itemConfig != null)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextField("ID", _itemConfig.Id);
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.ObjectField("Artwork", _itemConfig.Artwork, typeof(Sprite), false);
                EditorGUILayout.TextField("Name", _itemConfig.Name);
                EditorGUILayout.TextArea(_itemConfig.Description);
                EditorGUILayout.FloatField("Weight", _itemConfig.Weight);
                EditorGUILayout.IntField("Value", _itemConfig.Value);
                EditorGUILayout.IntField("Price", _itemConfig.Price);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Acquire Methods");
                EditorGUI.indentLevel++;
                for (var i = 0; i < _itemConfig.AcquireMethods.Length; i++)
                {
                    _itemConfig.AcquireMethods[i] = EditorGUILayout.Toggle($"Method {i}", _itemConfig.AcquireMethods[i]);
                }
                EditorGUI.indentLevel--;

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Curse List");
                EditorGUI.indentLevel++;
                for (var i = 0; i < _itemConfig.CurseList.Count; i++)
                {
                    _itemConfig.CurseList[i] = EditorGUILayout.TextField($"Curse {i}", _itemConfig.CurseList[i]);
                }
                EditorGUI.indentLevel--;

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Bless List");
                EditorGUI.indentLevel++;
                for (var i = 0; i < _itemConfig.BlessList.Count; i++)
                {
                    _itemConfig.BlessList[i] = EditorGUILayout.TextField($"Bless {i}", _itemConfig.BlessList[i]);
                }
                EditorGUI.indentLevel--;

                EditorGUILayout.Space();

                EditorGUILayout.Slider("Curse Possibility", _itemConfig.CursePossibility, 0f, 100f);
                EditorGUILayout.Slider("Bless Possibility", _itemConfig.BlessPossibility, 0f, 100f);
                EditorGUILayout.Slider("Encounter Possibility", _itemConfig.EncounterPossibility, 0f, 100f);

                EditorGUILayout.Space();

                EditorGUILayout.EnumPopup("Rarity", _itemConfig.Rarity);
            }
        }

        private ItemBaseConfig FindItemConfigByName(string name)
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(ItemBaseConfig)}");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                ItemBaseConfig itemConfig = AssetDatabase.LoadAssetAtPath<ItemBaseConfig>(path);
                if (itemConfig.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return itemConfig;
                }
            }

            return null;
        }
    }
}
