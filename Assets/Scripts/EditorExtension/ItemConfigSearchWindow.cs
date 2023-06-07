using Merchant.Config.Item;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;

namespace Merchant.EditorExtension
{
    public class ItemConfigSearchWindow : EditorWindow
    {
        private string _searchQuery = string.Empty;
        private ItemBaseConfig[] _matchingConfigs;

        private double _searchDelay = 0.5f; // Delay in seconds before triggering search
        private double _lastSearchTime; // Last time search was triggered

        [MenuItem("Window/Item Config Search")]
        public static void OpenWindow()
        {
            var window = GetWindow<ItemConfigSearchWindow>();
            window.titleContent = new GUIContent("Item Config Search");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Item Config Search", EditorStyles.boldLabel);

            EditorGUILayout.Space();

            _searchQuery = EditorGUILayout.TextField("Search Query", _searchQuery);

            EditorGUILayout.Space();

            DisplayMatchingConfigs();
        }

        private void OnEnable() 
        {
            EditorApplication.update += OnEditorUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
        }

        private void OnEditorUpdate()
        {
            // Check if enough time has passed since the last search
            if (EditorApplication.timeSinceStartup - _lastSearchTime >= _searchDelay)
            {
                SearchItemConfigs();
                _lastSearchTime = EditorApplication.timeSinceStartup;
                Repaint(); // Repaint the window to update the GUI
            }
        }

        private void SearchItemConfigs()
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(ItemBaseConfig)}");
            var matchingConfigs = new List<ItemBaseConfig>();
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                ItemBaseConfig itemConfig = AssetDatabase.LoadAssetAtPath<ItemBaseConfig>(path);
                if (string.IsNullOrEmpty(_searchQuery) 
                    || itemConfig.Name.Equals(_searchQuery, StringComparison.OrdinalIgnoreCase)
                    || itemConfig.Name.ToLowerInvariant().Contains(_searchQuery.ToLowerInvariant()))
                {
                    matchingConfigs.Add(itemConfig);
                }
            }
            _matchingConfigs = matchingConfigs.ToArray();
        }

        private void DisplayMatchingConfigs()
        {
            if (_matchingConfigs == null || _matchingConfigs.Length == 0)
            {
                EditorGUILayout.HelpBox("No matching Item Configs found.", MessageType.Info);
                return;
            }

            foreach (var itemConfig in _matchingConfigs)
            {
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                if (GUILayout.Button(itemConfig.Name, GUILayout.ExpandWidth(true)))
                {
                    // Handle the click event for the clickable element
                    ItemConfigWindow.OpenWindow(itemConfig);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
