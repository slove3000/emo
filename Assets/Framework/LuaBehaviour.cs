using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace emo
{
    public class LuaBehaviour : MonoBehaviour
    {
        Dictionary<int, LuaFunction> funcs = new Dictionary<int, LuaFunction>();

        void ChildComponentSafeDo<T>(string path, System.Action<T> action) where T : Component
        {
            var child = path == null ? transform : transform.Find(path);
            if (child != null)
            {
                var com = child.GetComponent<T>();
                if (com != null)
                {
                    action(com);
                }
            } 
        }

        void AddFunc(int id, LuaFunction func)
        {
            LuaFunction outValue;
            if (funcs.TryGetValue(id, out outValue))
            {
                outValue.Dispose();
            }
            funcs.Add(id, func);
        }

        public void SetButtonClick(string path, LuaFunction func)
        {
            ChildComponentSafeDo(path, delegate (Button com)
            {
                com.onClick.AddListener(delegate
                {
                    func.Call();
                });
                AddFunc(com.GetInstanceID(), func);
            });
        }

        public void SetScrollOnValueChanged(string path, LuaFunction func)
        {
            ChildComponentSafeDo(path, delegate (ScrollRect com)
            {
                com.onValueChanged.AddListener(delegate (Vector2 v)
                {
                    func.Action(v);
                });
                AddFunc(com.GetInstanceID(), func);
            });
        }

        public void SetToggleOnValueChanged(string path, LuaFunction func)
        {
            ChildComponentSafeDo(path, delegate (Toggle com)
            {
                com.onValueChanged.AddListener(delegate (bool v)
                {
                    func.Action(v);
                });
                AddFunc(com.GetInstanceID(), func);
            });
        }

        public void SetInputFieldFunc(string path, LuaFunction func)
        {
            ChildComponentSafeDo(path, delegate (InputField com)
            {
                com.onValueChanged.AddListener(delegate (string v)
                {
                    func.Action(v);
                });

                com.onEndEdit.AddListener(delegate (string v)
                {

                    func.Action(v, true);
                });
                AddFunc(com.GetInstanceID(), func);
            });
        }

        public void SetDropdownFunc(string path, LuaFunction func)
        {
            ChildComponentSafeDo(path, delegate (Dropdown com)
            {
                com.onValueChanged.AddListener(delegate (int v)
                {
                    func.Action(v);
                });
                AddFunc(com.GetInstanceID(), func);
            });
        }

        public void SetText(string text, string path)
        {
            ChildComponentSafeDo(path, delegate (Text com)
            {
                com.text = text;
            });
        }

        public string GetText(string path)
        {
            string text = null;
            ChildComponentSafeDo(path, delegate (Text com)
            {
                text = com.text;
            });
            return text;
        }

        public void SetSlider(float value, string path)
        {
            ChildComponentSafeDo(path, delegate(Slider com)
            {
                com.value = value;
            });
        }

        private void Start()
        {
            LuaManager.Register(this);
        }

        private void OnDestroy()
        {
            Clear();
            LuaManager.Unregister(this);
        } 

        public void Clear()
        {
            if (funcs.Count > 0)
            {
                foreach (var item in funcs)
                {
                    if (item.Value != null)
                    {
                        item.Value.Dispose();
                    }
                }
                funcs.Clear();
            } 
        }
    }
}