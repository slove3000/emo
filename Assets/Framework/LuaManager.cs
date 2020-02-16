using System;
using System.Collections.Generic;
using UnityEngine;
using xasset;
using XLua;

namespace emo
{
    public class LuaManager
    {
        private static LuaEnv luaEnv = new LuaEnv(); 
        private static List<LuaBehaviour> luaBehaviours = new List<LuaBehaviour>();
        private static Dictionary<string, Asset> assets = new Dictionary<string, Asset>();

        public static void Register(LuaBehaviour lua)
        {
            luaBehaviours.Add(lua);
        }

        public static void Unregister(LuaBehaviour lua)
        {
            luaBehaviours.Remove(lua);
        }

        public static void Init(Action succes)
        {
            Action onSuccess = delegate
            {
                if (Utility.assetBundleMode)
                {
                    luaEnv.AddLoader(ReadBytesFromAssetBundle);
                    OnInited(succes);
                }
                else
                {
                    luaEnv.AddLoader(ReadBytesFromEditor);
                    OnInited(succes);
                }
            };

            Action<string> onError = delegate (string e)
            {
                Debug.LogError(e);
            };

            Assets.Initialize(onSuccess, onError);
        }

        public static void Clear()
        {
            foreach (var item in assets)
            {
                item.Value.Release();
            }
            assets.Clear();
        }

        public static void Dispose()
        {
            Clear();

            foreach (var item in luaBehaviours)
            {
                item.Clear();
            }

            luaEnv.Dispose();
            luaEnv = null;
            Debug.Log("[LuaManager]Dispose");
        }

        public static void OnInited(Action cb)
        { 
            luaEnv.DoString("require 'main'");
            if (cb != null)
            {
                cb.Invoke();
                cb = null;
            }
        }

        public static T GetFunc<T>(string name)
        {
            return luaEnv.Global.Get<T>(name);
        }

        private static byte[] ReadBytesFromAssetBundle(ref string filepath)
        {
            var path = "Assets/Bytes/Lua/" + filepath + ".lua.bytes";
            Asset a; 
            if (!assets.TryGetValue(path, out a))
            {
                a = Assets.Load(path, typeof(TextAsset));
                assets[path] = a;
            }
            var ta = a.asset as TextAsset;
            if (ta != null)
            {
                return ta.bytes;
            }
            return null;
        }

        private static byte[] ReadBytesFromEditor(ref string filename)
        {
            var path = System.Environment.CurrentDirectory
                + "/Lua/"
                + filename
                + ".lua";

            if (!System.IO.File.Exists(path))
            {
                throw new System.IO.FileNotFoundException(path);
            }

            return System.IO.File.ReadAllBytes(path);
        }
    }
}
