using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using BhorGames.Mocha;

[CanEditMultipleObjects]
[CustomEditor(typeof(Mocha), true)]
public class MochaEditor : Editor
{
    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();
        GUILayout.Space(10f);
        //DrawDefaultInspector();
        Mocha go = Selection.activeGameObject.GetComponent<Mocha>(); // Gameobject that have Mocha Component When Selected
        go.animationType = (Mocha.ANIM_STYLE)EditorGUILayout.EnumPopup("Animation", go.animationType);
        GUILayout.Space(10f);
        go.animationT = (Mocha.ANIM_TYPE)EditorGUILayout.EnumPopup("Animation Type",go.animationT);
        GUILayout.Space(10f);
        go.curve = EditorGUILayout.CurveField("Animation Curve", go.curve);
        GUILayout.Space(10f);
        go.playOnAwake = EditorGUILayout.Toggle("Play On Awake",go.playOnAwake);
        switch (go.animationType)
        {
            case Mocha.ANIM_STYLE.MOVE:
                GUILayout.Space(10f);
                EditorGUILayout.LabelField("Animation Duration");
                go.move_duration = EditorGUILayout.Slider(go.move_duration, 0F, 10F);
                GUILayout.Space(10f);
                go.move_finalDestination = EditorGUILayout.Vector2Field("Final Position", go.move_finalDestination);
                GUILayout.Space(1f);
                if (GUILayout.Button("Set Current Position", EditorStyles.toolbarButton))
                {
                    go.move_finalDestination = go.gameObject.GetComponent<RectTransform>().anchoredPosition;
                }
                GUILayout.Space(10f);
                EditorGUILayout.LabelField("Is There Event When Animation Ends?");
                GUILayout.Space(10f);
                go.move_EndOfAnimation = EditorGUILayout.Toggle("End Event", go.move_EndOfAnimation);
                if (go.move_EndOfAnimation)
                {
                    EditorGUILayout.PropertyField(this.serializedObject.FindProperty("move_endEvent"), true);
                }
                break;
            case Mocha.ANIM_STYLE.SCALE:
                GUILayout.Space(10f);
                EditorGUILayout.LabelField("Animation Duration");
                go.scale_duration = EditorGUILayout.Slider(go.scale_duration, 0F, 10F);
                GUILayout.Space(10f);
                go.scale_finalScale = EditorGUILayout.Vector3Field("Final Scale", go.scale_finalScale);
                if (GUILayout.Button("Set Current Scale", EditorStyles.toolbarButton))
                {
                    go.scale_finalScale = go.gameObject.GetComponent<RectTransform>().localScale;
                }
                GUILayout.Space(10f);
                EditorGUILayout.LabelField("Is There Event When Animation Ends?");
                GUILayout.Space(10f);
                go.scale_EndOfAnimation = EditorGUILayout.Toggle("End Event", go.scale_EndOfAnimation);
                if (go.scale_EndOfAnimation)
                {
                    EditorGUILayout.PropertyField(this.serializedObject.FindProperty("scale_endEvent"), true);
                }
                break;
            case Mocha.ANIM_STYLE.ROTATE:
                GUILayout.Space(10f);
                EditorGUILayout.LabelField("Animation Duration");
                go.rotate_duration = EditorGUILayout.Slider(go.rotate_duration, 0F, 10F);
                GUILayout.Space(10f);
                go.rotate_finalRotation = EditorGUILayout.Vector3Field("Final Rotation", go.rotate_finalRotation);
                if (GUILayout.Button("Set Current Rotation", EditorStyles.toolbarButton))
                {
                    go.rotate_finalRotation = go.gameObject.GetComponent<RectTransform>().eulerAngles;
                }
                GUILayout.Space(10f);
                EditorGUILayout.LabelField("Is There Event When Animation Ends?");
                GUILayout.Space(10f);
                go.rotate_EndOfAnimation = EditorGUILayout.Toggle("End Event", go.rotate_EndOfAnimation);
                if (go.rotate_EndOfAnimation)
                {
                    EditorGUILayout.PropertyField(this.serializedObject.FindProperty("rotate_endEvent"), true);
                }
                break;    
            case Mocha.ANIM_STYLE.FADE:
                GUILayout.Space(10f);
                EditorGUILayout.LabelField("Animation Duration");
                go.fade_duration = EditorGUILayout.Slider(go.fade_duration, 0F, 10F);
                GUILayout.Space(10f);
                go.fade_finalAlpha = EditorGUILayout.FloatField("Final Alpha",go.fade_finalAlpha);
                if (GUILayout.Button("Set Current Alpha", EditorStyles.toolbarButton))
                {
                    go.fade_finalAlpha = go.gameObject.GetComponent<Image>().color.a;
                }
                GUILayout.Space(10f);
                EditorGUILayout.LabelField("Is There Event When Animation Ends?");
                GUILayout.Space(10f);
                go.fade_endOfAnimation = EditorGUILayout.Toggle("End Event", go.fade_endOfAnimation);
                if (go.fade_endOfAnimation)
                {
                    EditorGUILayout.PropertyField(this.serializedObject.FindProperty("rotate_endEvent"), true);
                }
                break;
            default:
                break;
        }
        GUILayout.Space(20f);
        if (GUILayout.Button("Animate"))
        {
            if (EditorApplication.isPlaying) {
              go.Play();
            } else
            {
                EditorUtility.DisplayDialog("Mocha","Please Use This On Play Mode","OK","");
            }
        }

        GUILayout.Space(10f);
        EditorGUILayout.HelpBox("Animation -> "+ go.animationType.ToString()  + " : This effects gameObject's " + go.animationType.ToString().ToLower() +
        "\n\nAnimation Type -> " + go.animationT.ToString() + 
        ((go.animationT == Mocha.ANIM_TYPE.LOOP) ? " : Animation repeats between its first state and final state" : " : Animation plays one time \n"), 
        MessageType.Info);
        
        this.serializedObject.ApplyModifiedProperties();
    }
}
