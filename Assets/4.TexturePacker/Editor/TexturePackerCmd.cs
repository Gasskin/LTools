using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TexturePackerImporter;
using UnityEditor;
using Debug = UnityEngine.Debug;

public class TexturePackerCmd
{
    // 裁切算法
    public enum AlgorithmType
    {
        // 无裁切
        None,

        // 正方形裁切，保持位置
        RectTrimKeepPos,

        // 正方形裁切，丢弃位置
        RectTrimDisPos,

        // 多边形裁切
        // PolygonTrim,
    }

    private const string TEXTURE_PACKER_PATH = @".\Tools\TexturePacker\TexturePacker\bin\TexturePacker.exe";

    public string PackName { get; private set; }


    public void PackFolder(string targetFolder, string savePath, AlgorithmType type)
    {
        PackName = Path.GetFileName(targetFolder);
        
        var files = Directory.GetFiles(savePath);
        foreach (var file in files)
        {
            if (file.Contains(PackName) && (file.Contains(".png")|| file.Contains(".tpsheet")))
            {
                Debug.Log($"Delete：{file}");
                File.Delete(file);
            }
        }
        Debug.Log($"TargetFolderPath: {savePath}");
        Debug.Log($"PackName: {PackName}");
        var arguments = $"\"{savePath}\"";
        arguments += " --texture-format png";
        arguments += $" --sheet \"{Path.Combine(savePath, PackName + "-{n}.png")}\"";
        arguments += $" --data \"{Path.Combine(savePath, PackName + "-{n}.tpsheet")}\"";
        arguments += " --format unity-texture2d";
        switch (type)
        {
            case AlgorithmType.None:
                arguments += " --algorithm MaxRects";
                arguments += " --trim-mode None";
                break;
            case AlgorithmType.RectTrimDisPos:
                arguments += " --algorithm MaxRects";
                arguments += " --trim-mode Crop";
                break;
            case AlgorithmType.RectTrimKeepPos:
                arguments += " --algorithm MaxRects";
                arguments += " --trim-mode CropKeepPos";
                break;
            // case AlgorithmType.PolygonTrim:
            //     arguments += " --algorithm Polygon";
            //     arguments += " --trim-mode Polygon";
            //     break;
        }
        arguments += " --max-width 256";
        arguments += " --max-height 256";
        arguments += " --shape-padding 2";
        arguments += " --border-padding 0";
        arguments += " --extrude 0";
        arguments += " --size-constraints AnySize";
        arguments += " --multipack";

        Debug.Log($"Cmd: {arguments}");

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = TEXTURE_PACKER_PATH,
            Arguments = arguments,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        using Process process = new Process();
        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            string error = process.StandardError.ReadToEnd();
            Debug.LogError($"TexturePacker执行失败: {error}");
            return;
        }
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    public void PackOneImagePolygonTrim(string targetImagePath)
    {
        Debug.Log("===== Pack Start =====");
        Debug.Log($"TargetImage: {targetImagePath}");
        var arguments = $"\"{targetImagePath}\"";
        arguments += " --texture-format png";
        arguments += $" --sheet \"{targetImagePath.Replace(".png","") + "-polygon.png"}\"";
        arguments += $" --data \"{targetImagePath.Replace(".png","") + "-polygon.tpsheet"}\"";
        arguments += " --format unity-texture2d";
        arguments += " --algorithm Polygon";
        arguments += " --trim-mode Polygon";
        arguments += " --max-width 2048";
        arguments += " --max-height 2048";
        arguments += " --shape-padding 2";
        arguments += " --border-padding 0";
        arguments += " --extrude 0";
        arguments += " --size-constraints AnySize";
        // arguments += " --multipack";

        Debug.Log(arguments);

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = TEXTURE_PACKER_PATH,
            Arguments = arguments,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        using Process process = new Process();
        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            string error = process.StandardError.ReadToEnd();
            Debug.LogError($"TexturePacker执行失败: {error}");
            return;
        }
        Debug.Log("===== Pack End =====");
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        
        File.Delete(targetImagePath.Replace(".png", "") + "-polygon.tpsheet");
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}