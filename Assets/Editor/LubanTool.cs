using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// LuBan生成工具类，它会调用XlsData/gen脚本，然后将数据全部合并到一个二进制文件中
/// </summary>
public class LubanTool
{
    // [MenuItem("菜单路径/子菜单")]
    [MenuItem("Luban/Build Combined Binary")]
    public static void BuildCombinedBinary()
    {
        // 1. 配置路径（建议使用相对路径）
        string inputDir = Path.Join(Application.dataPath, "XlsData");
        string outputFile = Path.Join(Application.dataPath, "AllConfigs.bytes");

        // 确保输出目录存在
        string outDir = Path.GetDirectoryName(outputFile);
        if (!Directory.Exists(outDir)) Directory.CreateDirectory(outDir);

        // 2. 执行合并逻辑
        try
        {
            MergeBinaries(inputDir, outputFile);

            // 3. 刷新 AssetDatabase，让 Unity 识别新生成的文件
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Luban", "合并二进制文件成功！", "确定");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"合并失败: {e.Message}");
            EditorUtility.DisplayDialog("错误", "合并过程中发生异常，请检查控制台。", "确定");
        }
    }

    private static void MergeBinaries(string inputDir, string outputFile)
    {

        Debug.Log($"开始合并 {inputDir} 下的所有 .bin 文件到 {outputFile}");

        string[] files = Directory.GetFiles(inputDir, "*.bytes");
        if (files.Length == 0) return;

        using (FileStream outStream = new FileStream(outputFile, FileMode.Create))
        using (BinaryWriter writer = new BinaryWriter(outStream))
        {
            // 写入文件数量
            writer.Write(files.Length);

            // 预留索引区空间并记录起始位置
            long indexPos = outStream.Position;
            foreach (var file in files)
            {
                writer.Write(Path.GetFileNameWithoutExtension(file)); // 文件名
                writer.Write(0L); // 预留 Offset
                writer.Write(0);  // 预留 Size
            }

            long dataStartPos = outStream.Position;
            List<(long offset, int size)> infos = new List<(long, int)>();

            // 写入数据体
            foreach (var file in files)
            {
                byte[] data = File.ReadAllBytes(file);
                infos.Add((outStream.Position - dataStartPos, data.Length));
                writer.Write(data);
            }

            // 回到索引区填充真实偏移
            outStream.Seek((int)indexPos, SeekOrigin.Begin);
            for (int i = 0; i < files.Length; i++)
            {
                // writer.ReadString(); // 跳过已写的名
                writer.Write(infos[i].offset);
                writer.Write(infos[i].size);
            }
        }
    }
}