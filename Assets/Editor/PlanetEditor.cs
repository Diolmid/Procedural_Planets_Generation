using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    private Planet _planet;
    private Editor _shapeEditor;
    private Editor _clourEditor;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if(check.changed )
            {
                _planet.GeneratePlanet();
            }
        }

        if(GUILayout.Button("Generate Planet"))
        {
            _planet.GeneratePlanet();
        }

        DrawSettingEditor(_planet.shapeSettings, _planet.OnShapeSettingUpdated, ref _planet.shapeSettingsFoldout, ref _shapeEditor);
        DrawSettingEditor(_planet.colourSettings, _planet.OnColourSettingUpdated, ref _planet.colourSettingsFoldout, ref _clourEditor);
    }

        private void OnEnable()
    {
        _planet = target as Planet;
    }

    private void DrawSettingEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
    {
        if(settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldout)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();

                    if(check.changed)
                    {
                        if(onSettingsUpdated != null)
                        {
                            onSettingsUpdated();
                        }
                    }
                }
            }
        }
    }
}