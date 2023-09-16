using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "RightArmAuraDATA", menuName = "Character/Aura/New Right Arm Aura Data")]
public class RightArmAura_ScriptableObject : ScriptableObject
{
    public string auraName;
    public AnimalType animalType;
    public Sprite sprite;
    public int hp;
    //public Skill
    public string skillDescription;
    public int attack;
    public int attackAPCost;
    public int skillAPCost;
}

#if UNITY_EDITOR
[CustomEditor(typeof(RightArmAura_ScriptableObject))]
public class RightArmAura_ScriptableObjectEditor : Editor
{
    public RightArmAura_ScriptableObject rightArmAura_SO;

    private void OnEnable()
    {
        rightArmAura_SO = target as RightArmAura_ScriptableObject;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (rightArmAura_SO.sprite == null)
        {
            return;
        }
        Texture2D texture = AssetPreview.GetAssetPreview(rightArmAura_SO.sprite);
        GUILayout.Label("", GUILayout.Height(80), GUILayout.Width(80));
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
    }
}
#endif