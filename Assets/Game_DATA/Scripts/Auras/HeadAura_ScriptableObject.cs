using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "HeadAuraDATA", menuName = "Character/Aura/New Head Aura Data")]
public class HeadAura_ScriptableObject : ScriptableObject
{
    public string auraName;
    public AnimalType animalType;
    public Sprite sprite;
    public int hp;
    public int AP_Cost;
    public Skill skill;
    public string description;
}

#if UNITY_EDITOR
[CustomEditor(typeof(HeadAura_ScriptableObject))]
public class HeadAura_ScriptableObjectEditor : Editor
{
    public HeadAura_ScriptableObject headAura_SO;

    private void OnEnable()
    {
        headAura_SO = target as HeadAura_ScriptableObject;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (headAura_SO.sprite == null)
        {
            return;
        }
        Texture2D texture = AssetPreview.GetAssetPreview(headAura_SO.sprite);
        GUILayout.Label("", GUILayout.Height(80), GUILayout.Width(80));
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);  
    }
}
#endif