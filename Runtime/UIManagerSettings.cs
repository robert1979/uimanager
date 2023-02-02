using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = "Data", menuName = "UIManager/UIManagerSettings", order = 1)]
public class UIManagerSettings : ScriptableObject
{
    private static UIManagerSettings instance;
    public static UIManagerSettings GetInstance()
    {
        if(instance==null)
        {
            instance = Load();
        }
        return instance;
    }
    private static UIManagerSettings Load()
    {
        var settings =  Resources.Load<UIManagerSettings>("UIManagerSettings");
        Debug.Assert(settings!=null,"Could not load settings");
        return settings;
    }
    public Object templateFolder;

    public string GetTemplateFolderName()
    {
        var result ="";
        #if UNITY_EDITOR
            if(templateFolder==null) return result;
            var dirInfo = new DirectoryInfo(AssetDatabase.GetAssetPath(templateFolder));
            return dirInfo.Name;
        #else
            return result;
        #endif
    }


    public List<string> GetTemplates()
    {
        var result = new List<string>();
        #if UNITY_EDITOR
        if(templateFolder==null)return result;
        var assetPath = AssetDatabase.GetAssetPath(templateFolder);
        var dirInfo =  new DirectoryInfo(GetFullPath(assetPath));
        result.AddRange(Directory.GetFiles(assetPath, "*.prefab", SearchOption.AllDirectories));
        #endif
        return result;
    }

    public static string GetFullPath(string assetPath)
    {
        return Application.dataPath + assetPath.Replace("Assets","");
    }

    public static string GetAssetPath(string fullPath)
    {
        return fullPath.Replace(Application.dataPath,"Assets");
    }
}
