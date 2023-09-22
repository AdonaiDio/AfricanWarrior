using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "TorsoAuraDATA", menuName = "Character/Aura/New Torso Aura Data")]
public class TorsoAura_ScriptableObject : ScriptableObject
{
    public string auraName;
    public AnimalType animalType;
    public Sprite sprite;
    public int hp;
    public string description;
    public int actionPointsPerTurn;
}

#if UNITY_EDITOR
[CustomEditor(typeof(TorsoAura_ScriptableObject))]
public class TorsoAura_ScriptableObjectEditor : Editor
{
    public TorsoAura_ScriptableObject torsoAura_SO;

    private void OnEnable()
    {
        torsoAura_SO = target as TorsoAura_ScriptableObject;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (torsoAura_SO.sprite == null)
        {
            return;
        }
        Texture2D texture = AssetPreview.GetAssetPreview(torsoAura_SO.sprite);
        GUILayout.Label("", GUILayout.Height(80), GUILayout.Width(80));
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
    }
}
#endif