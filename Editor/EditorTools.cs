using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorTools
{
    public const string MenuHead = "Y/";

    [MenuItem(MenuHead + "TestMenu")]
    private static void TestMenu()
    {
        UnityEngine.Debug.Log(string.Format("<color=white>{0}</color>", "测试"));
    }

}
