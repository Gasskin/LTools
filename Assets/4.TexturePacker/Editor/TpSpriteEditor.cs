using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(TpSprite))]
public class TpSpriteEditor : ImageEditor
{
    private SerializedProperty _atlas;
    private SerializedProperty _spriteName;

    protected override void OnEnable()
    {
        base.OnEnable();
        _atlas = serializedObject.FindProperty("_atlas");
        _spriteName = serializedObject.FindProperty("_spriteName");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_atlas);
        if (EditorGUI.EndChangeCheck())
            _spriteName.stringValue = "";

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_spriteName);
        if (EditorGUI.EndChangeCheck())
            ((TpSprite)serializedObject.targetObject).SetSprite(_spriteName.stringValue);

        if (GUILayout.Button("select sprite"))
        {
            var tpSprite = ((TpSprite)serializedObject.targetObject);
            if (_atlas.objectReferenceValue != null)
            {
                TextureSelectorWindow.Show((TpSpriteAtlas)_atlas.objectReferenceValue, (s =>
                {
                    tpSprite.SetSprite(s);
                    serializedObject.ApplyModifiedProperties();
                    tpSprite.SetNativeSize();
                }));
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    public class TextureSelectorWindow : EditorWindow
    {
        private List<Sprite> _sprites = new List<Sprite>();
        private Vector2 _scrollPos;
        private Action<string> _onSelectCallback;
        private string _searchKeyword = "";


        public static void Show(TpSpriteAtlas tpAtlas, Action<string> onSelect)
        {
            TextureSelectorWindow window = CreateInstance<TextureSelectorWindow>();
            window.titleContent = new GUIContent("选择贴图");
            window._onSelectCallback = onSelect;
            window._scrollPos = Vector2.zero;
            window.LoadTpAtlasTextures(tpAtlas);
            window.ShowUtility();
        }

        private void LoadTpAtlasTextures(TpSpriteAtlas tpAtlas)
        {
            _sprites.Clear();
            for (int i = 0; i < int.MaxValue; i++)
            {
                var sprite = tpAtlas.GetSpriteByIndex(i);
                if (sprite == null) 
                    break;
                _sprites.Add(sprite);
            }
        }


        private void OnGUI()
        {
            _searchKeyword = GUILayout.TextField(_searchKeyword);
            var filterSprite = _sprites
                .Where(sprite => string.IsNullOrEmpty(_searchKeyword) || sprite.name.ToLower().Contains(_searchKeyword.ToLower()))
                .ToList();

            _scrollPos = GUILayout.BeginScrollView(_scrollPos);
            
            foreach (var sprite in filterSprite)
            {
                GUILayout.BeginHorizontal("box");
                GUILayout.BeginVertical();


                Texture2D texture = sprite.texture;
                Rect spriteRect = sprite.rect;

                // 计算 UV 坐标
                Rect uv = new Rect(
                    spriteRect.x / texture.width,
                    spriteRect.y / texture.height,
                    spriteRect.width / texture.width,
                    spriteRect.height / texture.height
                );

                // 计算显示尺寸（保持原始宽高比）
                float aspect = spriteRect.width / spriteRect.height;
                float displayWidth = Mathf.Min(100f, spriteRect.width);
                float displayHeight = displayWidth / aspect;

                // 为贴图预留布局空间
                Rect drawRect = GUILayoutUtility.GetRect(displayWidth, displayHeight, GUILayout.ExpandWidth(false));

                // 实际绘制
                GUI.DrawTextureWithTexCoords(drawRect, texture, uv);

                GUILayout.Label(sprite.name);

                if (GUILayout.Button("选择", GUILayout.Width(60)))
                {
                    _onSelectCallback?.Invoke(sprite.name);
                    Close();
                }


                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
        }
    }
}