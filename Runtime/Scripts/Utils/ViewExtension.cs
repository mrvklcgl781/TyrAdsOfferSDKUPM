using System;
using System.Collections.Generic;
using UnityEngine;

namespace TyrDK
{
    public class ViewExtension
    {
        private static List<GameObject> limitGOs = new();
        public static void CreateLimitGO(string name, Vector3 pos, Transform parent, Action<Vector2> posSet)
        {
            GameObject go = new GameObject(name);
            RectTransform rect = go.AddComponent<RectTransform>();
            rect.sizeDelta = Vector2.zero;
            go.transform.position = pos;
            go.transform.SetParent(parent);
            posSet?.Invoke(rect.anchoredPosition);
            limitGOs.Add(go);
        }
        
        public static void ClearLimitGOs()
        {
            for (int i = 0; i < limitGOs.Count; i++)
            {
                GameObject.Destroy(limitGOs[i]);
            }
            limitGOs.Clear();
        }
    }
}