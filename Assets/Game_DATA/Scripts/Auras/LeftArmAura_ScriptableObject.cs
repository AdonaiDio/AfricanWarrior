using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = "LeftArmAuraDATA", menuName = "Character/Aura/New Left Arm Aura Data")]
public class LeftArmAura_ScriptableObject : GenericAura_ScriptableObject
{
    public LeftArmAura_ScriptableObject()
    {
        this.auraPart = AuraParts.LeftArm;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(LeftArmAura_ScriptableObject))]
public class LeftArmAura_ScriptableObjectEditor : Editor
{

    SerializedProperty auraName;
    SerializedProperty animalType;
    SerializedProperty sprite;
    SerializedProperty hp;
    SerializedProperty skill; //
    SerializedProperty description;
    SerializedProperty attack; //
    SerializedProperty attackAPCost; //
    SerializedProperty skillAPCost; //
    SerializedProperty actionPointsPerTurn; //

    public LeftArmAura_ScriptableObject leftArmAura_SO;

    private void OnEnable()
    {
        leftArmAura_SO = target as LeftArmAura_ScriptableObject;

        auraName = serializedObject.FindProperty("auraName");
        animalType = serializedObject.FindProperty("animalType");
        sprite = serializedObject.FindProperty("sprite");
        hp = serializedObject.FindProperty("hp");
        skill = serializedObject.FindProperty("skill");
        description = serializedObject.FindProperty("description");
        attack = serializedObject.FindProperty("attack");
        attackAPCost = serializedObject.FindProperty("attackAPCost");
        skillAPCost = serializedObject.FindProperty("skillAPCost");
        actionPointsPerTurn = serializedObject.FindProperty("actionPointsPerTurn");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.PropertyField(auraName);
        EditorGUILayout.PropertyField(animalType);
        EditorGUILayout.PropertyField(sprite);
        EditorGUILayout.PropertyField(hp);
        EditorGUILayout.PropertyField(skill); //
        EditorGUILayout.PropertyField(description);
        EditorGUILayout.PropertyField(attack); //
        EditorGUILayout.PropertyField(attackAPCost); //
        EditorGUILayout.PropertyField(skillAPCost); //
        //EditorGUILayout.PropertyField(actionPointsPerTurn);

        serializedObject.ApplyModifiedProperties();


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