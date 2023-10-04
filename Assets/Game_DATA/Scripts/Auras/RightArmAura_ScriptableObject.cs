using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "RightArmAuraDATA", menuName = "Character/Aura/New Right Arm Aura Data")]
public class RightArmAura_ScriptableObject : GenericAura_ScriptableObject
{
    public RightArmAura_ScriptableObject()
    {
        this.auraPart = AuraParts.RightArm;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(RightArmAura_ScriptableObject))]
public class RightArmAura_ScriptableObjectEditor : Editor
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

    public RightArmAura_ScriptableObject rightArmAura_SO;

    private void OnEnable()
    {
        rightArmAura_SO = target as RightArmAura_ScriptableObject;

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