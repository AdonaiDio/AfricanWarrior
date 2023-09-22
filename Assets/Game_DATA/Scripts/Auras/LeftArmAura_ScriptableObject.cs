using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = "LeftArmAuraDATA", menuName = "Character/Aura/New Left Arm Aura Data")]
public class LeftArmAura_ScriptableObject : ScriptableObject
{
    public string auraName;
    public AnimalType animalType;
    public Sprite sprite;
    public int hp;
    public Skill skill;
    public string description;
    public int attack;
    public int attackAPCost;
    public int skillAPCost;
}

#if UNITY_EDITOR
[CustomEditor(typeof(LeftArmAura_ScriptableObject))]
public class LeftArmAura_ScriptableObjectEditor : Editor
{
    public LeftArmAura_ScriptableObject leftArmAura_SO;

    private void OnEnable()
    {
        leftArmAura_SO = target as LeftArmAura_ScriptableObject;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (leftArmAura_SO.sprite == null)
        {
            return;
        }
        Texture2D texture = AssetPreview.GetAssetPreview(leftArmAura_SO.sprite);
        GUILayout.Label("", GUILayout.Height(80), GUILayout.Width(80));
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
    }
}
#endif