using System;
using Procedural;
using Procedural.Terrain;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace GrassFlowDetailRenderer.GrassFlowDetailRenderer.Editor.Procedural
{
  [CustomEditor(typeof(TerrainGrassMapRenderer))]
  public class TerrainGrassMapRendererInspector : UnityEditor.Editor
  {
    private static GUIContent RenderButtonContent;
    private TerrainGrassMapRenderer script;

    private SerializedProperty settings;
    private SerializedProperty terrainObjects;
    private static string iconPrefix => EditorGUIUtility.isProSkin ? "d_" : "";

    private void OnEnable()
    {
      script = (TerrainGrassMapRenderer) target;

      settings = serializedObject.FindProperty("settings");
      terrainObjects = serializedObject.FindProperty("terrainObjects");
      if (terrainObjects.arraySize == 0) terrainObjects.isExpanded = true;

      RenderButtonContent = new GUIContent("  Render",
        EditorGUIUtility.IconContent(iconPrefix + "Animation.Record").image);
    }

    public override void OnInspectorGUI()
    {
      serializedObject.Update();

      EditorGUI.BeginChangeCheck();

      using (new EditorGUILayout.HorizontalScope())
      {
        EditorGUILayout.PropertyField(settings);

        if (GUILayout.Button(
              new GUIContent(" New",
                EditorGUIUtility
                  .IconContent(iconPrefix + "editicon.sml")
                  .image), GUILayout.MaxWidth(70f)))
        {
          settings.objectReferenceValue =
            ProceduralDetailMapSettingsInspector.CreateAndSaveSettings();
        }
      }

      if (!settings.objectReferenceValue)
      {
        EditorGUILayout.HelpBox("No settings assigned", MessageType.Error);
        return;
      }

      EditorGUILayout.Space();

      EditorGUILayout.LabelField("Terrain(s)", EditorStyles.boldLabel);
      EditorGUILayout.PropertyField(terrainObjects, GUIContent.none);

      EditorGUILayout.Space();

      if (GUILayout.Button(RenderButtonContent, GUILayout.Height(30f)))
        ProceduralDetailMapSettingsInspector.Render(script);

      if (EditorGUI.EndChangeCheck())
      {
        serializedObject.ApplyModifiedProperties();
      }
    }
  }
}
