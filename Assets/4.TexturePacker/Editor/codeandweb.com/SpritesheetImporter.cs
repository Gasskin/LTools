/*
 *  TexturePacker Importer
 *  (c) CodeAndWeb GmbH, Saalbaustraße 61, 89233 Neu-Ulm, Germany
 *
 *  Use this script to import sprite sheets generated with TexturePacker.
 *  For more information see https://www.codeandweb.com/texturepacker/unity
 *
 */

using UnityEngine;
using UnityEditor;

// Note: TexturePacker Importer with Unity 2021.2 (or newer) requires the "Sprite 2D" package,
//       please make sure that it is part of your Unity project. You can install it using
//       Unity's package manager.

#if UNITY_2021_2_OR_NEWER
using UnityEditor.U2D.Sprites;
using System.Collections.Generic;
using System.IO;
#endif


namespace TexturePackerImporter
{
    public class SpritesheetImporter : AssetPostprocessor
    {
        [InitializeOnLoadMethod]
        static void CloseDeubg()
        {
            Dbg.enabled = false;
        }

        void OnPreprocessTexture()
        {
            TextureImporter importer = assetImporter as TextureImporter;
            SheetInfo sheet = TexturePackerImporter.getSheetInfo(importer);
            if (sheet != null)
            {
                Dbg.Log("Updating sprite sheet " + importer.assetPath);
#if UNITY_2021_2_OR_NEWER
                updateSprites(importer, sheet);
#else
                importer.spritesheet = sheet.metadata;
#endif
            }
        }


#if UNITY_2021_2_OR_NEWER
        private static void updateSprites(TextureImporter importer, SheetInfo sheet)
        {
            var dataProvider = GetSpriteEditorDataProvider(importer);
            var spriteNameFileIdDataProvider = dataProvider.GetDataProvider<ISpriteNameFileIdDataProvider>();
            var oldIds = spriteNameFileIdDataProvider.GetNameFileIdPairs();
            SpriteRect[] rects = sheetInfoToSpriteRects(sheet);
            SpriteNameFileIdPair[] ids = generateSpriteIds(oldIds, rects);

            //
            // if (!importer.assetPath.Contains("Image"))
            // {
            //     var atlasName = Path.GetFileNameWithoutExtension(importer.assetPath).Split("-")[0];
            //     var artPath = $"{TexturePackerEditor.ART_ATLAS_PATH}/{atlasName}";
            //     for (int i = 0; i < rects.Length; i++)
            //     {
            //         var rect = rects[i];
            //         var id = ids[i];
            //         // 不同版本打包这里的名字好像不太一样
            //         var artName = id.name.Replace($"{atlasName}-", "");
            //         var artSpritePath = $"{artPath}/{artName}.png";
            //         var artSprite = AssetDatabase.LoadAssetAtPath<Sprite>(artSpritePath);
            //         if (artSprite != null)
            //         {
            //             rect.border = artSprite.border;
            //         }
            //         else
            //         {
            //             Debug.LogError($"Art目录不存在：{artSpritePath}");
            //         }
            //     }
            // }

            dataProvider.SetSpriteRects(rects);
            spriteNameFileIdDataProvider.SetNameFileIdPairs(ids);
            dataProvider.Apply();
            EditorUtility.SetDirty(importer);
        }


        private static ISpriteEditorDataProvider GetSpriteEditorDataProvider(TextureImporter importer)
        {
            var dataProviderFactories = new SpriteDataProviderFactories();
            dataProviderFactories.Init();
            var dataProvider = dataProviderFactories.GetSpriteEditorDataProviderFromObject(importer);
            dataProvider.InitSpriteEditorDataProvider();
            return dataProvider;
        }


        private static SpriteRect[] sheetInfoToSpriteRects(SheetInfo sheet)
        {
            int spriteCount = sheet.metadata.Length;
            SpriteRect[] rects = new SpriteRect[spriteCount];

            for (int i = 0; i < spriteCount; i++)
            {
                SpriteRect sr = rects[i] = new SpriteRect();
                SpriteMetaData smd = sheet.metadata[i];

                sr.name = smd.name;
                sr.rect = smd.rect;
                sr.pivot = smd.pivot;
                sr.border = smd.border;
                sr.alignment = (SpriteAlignment)smd.alignment;

                // sr.spriteID not yet initialized, this is done in generateSpriteIds()
            }

            return rects;
        }


        private static SpriteNameFileIdPair[] generateSpriteIds(IEnumerable<SpriteNameFileIdPair> oldIds,
            SpriteRect[] sprites)
        {
            SpriteNameFileIdPair[] newIds = new SpriteNameFileIdPair[sprites.Length];

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].spriteID = idForName(oldIds, sprites[i].name);
                newIds[i] = new SpriteNameFileIdPair(sprites[i].name, sprites[i].spriteID);
            }

            return newIds;
        }


        private static GUID idForName(IEnumerable<SpriteNameFileIdPair> oldIds, string name)
        {
            foreach (SpriteNameFileIdPair old in oldIds)
            {
                if (old.name == name)
                {
                    return old.GetFileGUID();
                }
            }
            return GUID.Generate();
        }
#endif
    }
}