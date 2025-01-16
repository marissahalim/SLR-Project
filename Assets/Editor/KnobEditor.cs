using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[CustomEditor(typeof(KnobController))] 
public class KnobEditor : Editor
{
    public override void OnInspectorGUI() //2
    {
        KnobController knob = (KnobController) target;
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("isInput"));
		GUILayout.Space(20f);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("valueType"));
        if (knob.valueType == KnobController.KnobValueTypes.Ticks) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("minValue"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxValue"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("tickSize"));
            GUILayout.Space(20f);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("currentValue"));
        } else if (knob.valueType == KnobController.KnobValueTypes.Continuous) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("minValue"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxValue"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("decimals"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isPercent"));
            GUILayout.Space(20f);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("currentValue"));
        } else if (knob.valueType == KnobController.KnobValueTypes.Labels) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("stringValues"));
            GUILayout.Space(20f);
            GUILayout.Label("Current value represents an index in the labels array");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("currentValue"));
        }

        GUILayout.Space(20f); //2
        EditorGUILayout.PropertyField(serializedObject.FindProperty("knobColor"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("startAngle"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("endAngle"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("flip"));
        if (knob.isInput) {
            GUILayout.Space(20f);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onValueChanged"));
        }
        serializedObject.ApplyModifiedProperties();
    }
}
