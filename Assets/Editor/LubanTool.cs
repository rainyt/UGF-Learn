using System.IO;
using UnityEditor;
using UnityEngine;
using System.Diagnostics;

/// <summary>
/// LuBan生成工具类，它会调用XlsData/gen脚本，然后将数据全部合并到一个二进制文件中
/// </summary>
public class LubanTool
{
    // [MenuItem("菜单路径/子菜单")]
    [MenuItem("Luban/Build Combined Binary")]
    public static void BuildCombinedBinary()
    {

        UnityEngine.Debug.Log($"当前目录: {Directory.GetCurrentDirectory()}, 当前平台: {Application.platform}");
        // 先运行指定的命令
        // 判断是否为Windows系统
        Process process = null;

        ProcessStartInfo info = new ProcessStartInfo();
        info.WorkingDirectory = "./XlsData";
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            info.FileName = "cmd.exe";
            info.Arguments = "gen.bat";
        }
        else
        {
            info.FileName = "/bin/bash";
            info.EnvironmentVariables.Add("DOTNET_ROLL_FORWARD", "Major");
            info.Arguments = "-l gen.sh";
        }
        info.UseShellExecute = false;
        info.RedirectStandardError = true;
        info.RedirectStandardOutput = true;

        process = Process.Start(info);
        string stdOutput = process.StandardOutput.ReadToEnd();
        string stdError = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            UnityEngine.Debug.LogError($"生成二进制文件失败，退出码: {process.ExitCode}");

            UnityEngine.Debug.LogError($"标准输出: {stdOutput}");
            UnityEngine.Debug.LogError($"标准错误: {stdError}");

            EditorUtility.DisplayDialog("错误", "生成二进制文件失败，请检查控制台。", "确定");
            return;
        }
        else
        {
            UnityEngine.Debug.Log($"LuBan生成二进制文件成功，退出码: {process.ExitCode}");
        }

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

            // 合并完毕后，删除原始文件
            Directory.Delete(inputDir, true);

            // 3. 刷新 AssetDatabase，让 Unity 识别新生成的文件
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Luban", "合并二进制文件成功！", "确定");
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError($"合并失败: {e.Message}");
            EditorUtility.DisplayDialog("错误", "合并过程中发生异常，请检查控制台。", "确定");
        }
    }

    private static void MergeBinaries(string inputDir, string outputFile)
    {

        UnityEngine.Debug.Log($"开始合并 {inputDir} 下的所有 .bin 文件到 {outputFile}");

        string[] files = Directory.GetFiles(inputDir, "*.bytes");
        if (files.Length == 0) return;

        using (FileStream outStream = new FileStream(outputFile, FileMode.Create))
        using (BinaryWriter writer = new BinaryWriter(outStream))
        {

            // 数据格式：名字长度 + 名字 + 数据长度 + 数据

            foreach (var file in files)
            {
                byte[] data = File.ReadAllBytes(file);
                string fileName = Path.GetFileNameWithoutExtension(file);
                UnityEngine.Debug.Log($"合并文件: {fileName}, 大小: {data.Length}");
                writer.Write(fileName);
                writer.Write7BitEncodedInt32(data.Length);
                writer.Write(data);
            }
        }

    }
}