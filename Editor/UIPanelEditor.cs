using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using UnityEngine.Events;
using System.Reflection;

[CanEditMultipleObjects]
[CustomEditor(typeof(UIRectEntity),true)]
public class UIPanelEditor : Editor
{

    private int templateCatIdx;
    private bool subscribed;

    

    private void Awake()
    {
            SceneView.duringSceneGui -=  CustomOnSceneGUI;
            SceneView.duringSceneGui +=  CustomOnSceneGUI;
    }
     
     void CustomOnSceneGUI(SceneView sceneview)
     {
        var panel =(UIPanel)target;
        if(panel.MainRect==null) return;
        Vector3[] corners = new Vector3[4];
        panel.MainRect.GetWorldCorners(corners);

        //Handles.PositionHandle(corners[0], Quaternion.identity);
        var midPoint = corners[1];
        midPoint.y+=100;
        var screenPos = sceneview.camera.WorldToScreenPoint(corners[2]);
        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize =  Mathf.RoundToInt(60f * (1000f/sceneview.cameraDistance));
        
        Handles.Label(midPoint,panel.ID,myStyle);
     }

    public static Vector3 SceneViewWorldToScreenPoint(SceneView sv, Vector2 worldPos)
    {
        var style = (GUIStyle)"GV Gizmo DropDown";
        Vector2 ribbon = style.CalcSize(sv.titleContent);
 
        Vector2 sv_correctSize = sv.position.size;
        sv_correctSize.y -= ribbon.y; //exclude this nasty ribbon
 
        //gives coordinate inside SceneView context.
        // WorldToViewportPoint() returns 0-to-1 value, where 0 means 0% and 1.0 means 100% of the dimension
        Vector3 pointInView = sv.camera.WorldToViewportPoint(worldPos);
        Vector3 pointInSceneView = pointInView * sv_correctSize;
        var p1 = pointInSceneView;
        p1.y = sv.position.height - p1.y;
 
        return p1;
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var panel =(UIPanel)target;

        if(UIManagerSettings.GetInstance()!=null)
        {
            var templatesAssetPaths = UIManagerSettings.GetInstance().GetTemplates();

            var labelList = new List<string>();
            var rootFolderName =UIManagerSettings.GetInstance().GetTemplateFolderName();
            labelList.Add("<Select>");
            foreach(var t in templatesAssetPaths)
            {
                labelList.Add( rootFolderName + t.Split(rootFolderName)[1]);
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Create Element");
            EditorGUI.BeginChangeCheck();
            templateCatIdx = EditorGUILayout.Popup(templateCatIdx, labelList.ToArray());
            if(EditorGUI.EndChangeCheck())
            {
                var settings = UIManagerSettings.GetInstance();
                var assetPath = templatesAssetPaths[templateCatIdx-1];
  
                var button = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(assetPath)) as GameObject;
                button.transform.SetParent(panel.transform);
                button.transform.localPosition = Vector3.zero;
                templateCatIdx =0;
            }
            GUILayout.EndHorizontal();
        }

        var buttons = panel.gameObject.GetComponentsInChildren<UIButtonTransition>();

        if(GUILayout.Button("autoName" ,GUILayout.Width(100)))
        {
            foreach(var o in targets)
            {
                var p = (UIRectEntity)o;
                if(!string.IsNullOrEmpty(p.ID))
                {
                    p.gameObject.name = p.ID+p.NameSuffix;
                }
            }
        }
        GUILayout.Space(30);
        panel.expandedView = EditorGUILayout.BeginFoldoutHeaderGroup(panel.expandedView,"Associated Buttons");
        if(panel.expandedView)
        {
            GUIStyle headStyle = new GUIStyle();
            headStyle.fontStyle = FontStyle.Bold;
            headStyle.normal.textColor = Color.cyan;
            foreach(var b in buttons)
            {
                GUILayout.BeginVertical("Box");
                    var s  = new SerializedObject(b);
                    BeginIndent(0);
                        GUILayout.Label(b.name,headStyle);
                        if(GUILayout.Button("s", GUILayout.Width(30)))
                        {
                            Selection.activeGameObject = b.gameObject;
                        }
                    EndIndent();
                    var idproperty = s.FindProperty("targetPanelId");
                    var eventProperty = s.FindProperty("onClick");

                    EditorGUI.BeginChangeCheck();

                    BeginIndent();
                        EditorGUILayout.PropertyField(idproperty);
                    EndIndent();

                    BeginIndent();
                        EditorGUILayout.PropertyField(eventProperty);
                    EndIndent();

                    if(EditorGUI.EndChangeCheck())
                    {
                        s.ApplyModifiedProperties();
                    }
                GUILayout.EndVertical();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }

    private void BeginIndent(int indent =40)
    {
        GUILayout.BeginHorizontal();

        GUILayout.Space(indent);
    }

    private void EndIndent()
    {
        GUILayout.EndHorizontal();
    }


}
