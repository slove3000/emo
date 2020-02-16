using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Text;
using xasset.editor;

namespace emo.editor
{
    public class Packager
    {
        private const string LuaVeresionsPath = "versions_lua.txt";

        static public void CopyLua2Bytes()
        {
            var path = System.Environment.CurrentDirectory + "/Lua";
            var files = Directory.GetFiles(path, "*.lua", System.IO.SearchOption.AllDirectories);


            Dictionary<string, string> versions = new Dictionary<string, string>();
            if (File.Exists(LuaVeresionsPath))
            {
                var lines = File.ReadAllLines(LuaVeresionsPath);
                foreach (var item in lines)
                {
                    if (string.IsNullOrEmpty(item))
                    {
                        continue;
                    }
                    var fields = item.Split('=');
                    if (fields.Length > 1)
                    {
                        versions[fields[0]] = fields[1];
                    }
                }
            }

            List<string> newFiles = new List<string>();
            List<string> delFiles = new List<string>();

            Dictionary<string, string> map = new Dictionary<string, string>();

            foreach (var item in files)
            {
                var dest = item.Replace(System.Environment.CurrentDirectory, Application.dataPath + "/Bytes") + ".bytes";
                var dir = Path.GetDirectoryName(dest);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                File.Copy(item, dest, true); 
                map[dest] = item;
                var info = new FileInfo(item);
                string ver = null;
                versions.TryGetValue(item, out ver);
                string cver = info.LastWriteTime.ToFileTime().ToString(); 
                if (!cver.Equals(ver) || !File.Exists(dest))
                {
                    versions[item] = cver;
                    newFiles.Add(dest);
                }
            } 

            var bytesFiles = Directory.GetFiles(Application.dataPath + "/Bytes/Lua", "*.bytes", System.IO.SearchOption.AllDirectories);
            foreach (var item in bytesFiles)
            { 
                if(! map.ContainsKey(item)) {
                    delFiles.Add(item);
                }
            } 

            if (delFiles.Count > 0)
            {
                for (int i = 0; i < delFiles.Count; i++)
                {
                    var item = delFiles[i];
                    File.Delete(item);
                }
            }

            if (newFiles.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var item in versions)
                {
                    sb.AppendLine(item.Key + "=" + item.Value);
                }
                if (File.Exists(LuaVeresionsPath))
                {
                    File.Delete(LuaVeresionsPath);
                }
                File.WriteAllText(LuaVeresionsPath, sb.ToString()); 
            } 

            Debug.Log(newFiles.Count + " lua files has update.");
        }

        [MenuItem("Assets/AssetBundles/Build")]
        static void Build()
        {
            CopyLua2Bytes();
            AssetDatabase.Refresh();  
            MarkLuaBytes();
            MarkPrefabs();
            BuildScript.BuildManifest();
            BuildScript.BuildAssetBundles();
        }

        private static void MarkPrefabs()
        {
            var path = "Assets/Framework/Prefabs";
            var searchPattern = "*.prefab";
            MarkAssetsByRelativeDirWith(path, searchPattern);
        }

        private static void MarkAssetsByRelativeDirWith(string path, string searchPattern)
        {
            var subIndex = path.LastIndexOf('/') + 1;
            var dirs = Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories);
            foreach (var item in dirs)
            {
                var im = AssetImporter.GetAtPath(item);
                var assetName = Path.GetFileNameWithoutExtension(item);
                var dir = Path.GetDirectoryName(item);
                var relativeDir = dir.Substring(subIndex);
                var assetBundleName = (relativeDir + "/" + assetName).ToLower();
                if (!assetBundleName.Equals(im.assetBundleName))
                {
                    im.assetBundleName = assetBundleName;
                }
            }
        }

        private static void MarkLuaBytes()
        { 
            var path = "Assets/Bytes";
            var dirs = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
            foreach (var item in dirs)
            {
                var im = AssetImporter.GetAtPath(item);
                var assetBundleName = item.Substring(path.Length + 1).ToLower() + "_g";
                if (!assetBundleName.Equals(im.assetBundleName))
                {
                    im.assetBundleName = assetBundleName;
                }
            }
        }
    }
}
