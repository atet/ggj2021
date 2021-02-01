using System;
using System.Collections.Generic;
using System.Linq;
using KennethDevelops.Events;
using KennethDevelops.Extensions;
using UnityEditor;
using UnityEngine;

namespace KennethDevelops.Util{
    
    [CustomEditor(typeof(TriggerDispatcher))]
    public class TriggerDispatcherInspector : Editor{

        private TriggerDispatcher _target;

        private Dictionary<DispatchAction, SerializedObject> _serializedObjects = new Dictionary<DispatchAction, SerializedObject>();
        
        #region Styles

        private GUIStyle _titleStyle;
        private GUIStyle _centeredStyle;

        #endregion

        private Color _topColor;
        private Color _botColor;


        private void OnEnable(){
            _target = (TriggerDispatcher) target;
            
            _titleStyle    = new GUIStyle{ fontStyle = FontStyle.Bold, fontSize = 12};
            _centeredStyle = new GUIStyle{ alignment = TextAnchor.MiddleCenter };
            
            _topColor = new Color(20 / 255f, 39 / 255f, 58 / 255f);
            _botColor = new Color(29 / 255f, 55 / 255f, 81 / 255f, .7f);
        }

        public override void OnInspectorGUI(){
            GUILayout.Space(5);

            if (!_target.HasCollider() && !_target.HasCollider2D()){
                EditorGUILayout.HelpBox("There is no Collider in the current GameObject. This means actions will not trigger unless you add a Collider component.", MessageType.Warning);
                GUI.changed = true;
                
                return;
            }

            #region Gizmos

            _target.drawGizmos = (GizmosDrawType) EditorGUILayout.EnumPopup("Draw Gizmos", _target.drawGizmos);

            _target.gizmosFoldout = EditorGUILayout.Foldout(_target.gizmosFoldout, "Gizmos Settings");

            if (_target.gizmosFoldout){
                _target.gizmosOutlineColor = EditorGUILayout.ColorField("Outline Color", _target.gizmosOutlineColor);
                _target.gizmosFillColor    = EditorGUILayout.ColorField("Fill Color", _target.gizmosFillColor);
            }
            
            #endregion

            GUIUtilPro.DrawSeparator();

            #region Trigger Conditions

            EditorGUILayout.LabelField("Trigger on...", _titleStyle);
            GUILayout.Space(10);

            for (var i = 0; i < _target.triggerConditions.Count; i++){
                
                

                var topRect = EditorGUILayout.BeginHorizontal();
                EditorGUI.DrawRect(new Rect(topRect.x - 1, topRect.y - 2, topRect.width + 1, topRect.height + 3),
                                   _topColor);
                
                _target.triggerConditions[i] = (TriggerCondition) EditorGUILayout.EnumPopup(_target.triggerConditions[i]);
                
                GUILayout.FlexibleSpace();

                var removeButtonRect = topRect.SetX(topRect.width - 8).AddY(1).SetWidth(20).SetHeight(13);

                if (GUIUtilPro.DrawRemoveButton(removeButtonRect, Event.current)){
                    _target.triggerConditions.RemoveAt(i);
                    
                    GUI.changed = true;
                }
                    
                EditorGUILayout.EndHorizontal();
                
                GUILayout.Space(5);
            }

            if (GUILayout.Button("+Condition", GUILayout.Width(80), GUILayout.Height(20))){
                _target.triggerConditions.Add(TriggerCondition.OnCollisionEnter);
                GUI.changed = true;
            }

            #endregion
            
            GUIUtilPro.DrawSeparator();

            #region Filters

            EditorGUILayout.LabelField("Filter by...", _titleStyle);
            GUILayout.Space(10);

            #region Layer Filter
            
            var layerRect = EditorGUILayout.BeginHorizontal();
            EditorGUI.DrawRect(new Rect(layerRect.x - 1, layerRect.y - 2, layerRect.width + 1, layerRect.height + 3),
                               _topColor);

            _target.layerFilter = GUIUtilPro.LayerMaskField(" Layer", _target.layerFilter, true);
            
            EditorGUILayout.EndHorizontal();
            
            #endregion
            
            GUILayout.Space(5);

            #region Tags Filter

            EditorGUI.indentLevel = 1;
            
            var tagFoldoutRect = EditorGUILayout.BeginHorizontal();
            EditorGUI.DrawRect(new Rect(tagFoldoutRect.x - 1, tagFoldoutRect.y - 2, tagFoldoutRect.width + 1, tagFoldoutRect.height + 3),
                               _topColor);

            _target.tagFoldout = GUIUtilPro.WhiteFoldout(_target.tagFoldout, "Tag");
            
            EditorGUI.indentLevel = 0;
            
            EditorGUILayout.EndHorizontal();
            
            if (_target.tagFoldout) DrawTagFields(tagFoldoutRect);
            
            #endregion
            
            GUILayout.Space(5);

            #region Name contains
            
            EditorGUI.indentLevel = 1;

            var nameFoldoutRect = EditorGUILayout.BeginHorizontal();
            EditorGUI.DrawRect(new Rect(nameFoldoutRect.x - 1, nameFoldoutRect.y - 2, nameFoldoutRect.width + 1, nameFoldoutRect.height + 3),
                               _topColor);

            _target.colliderNameFoldout = GUIUtilPro.WhiteFoldout(_target.colliderNameFoldout, "Name contains...");
            
            EditorGUI.indentLevel = 0;
            
            EditorGUILayout.EndHorizontal();
            
            if (_target.colliderNameFoldout) DrawColNameFields(nameFoldoutRect);

            #endregion

            #endregion

            GUIUtilPro.DrawSeparator();
            
            #region Actions to perform

            EditorGUILayout.LabelField("Perform action...", _titleStyle);
            GUILayout.Space(10);

            for (var i = 0; i < _target.actions.Count; i++){
                EditorGUI.indentLevel = 1;
                
                var actionRect = EditorGUILayout.BeginHorizontal();
                EditorGUI.DrawRect(new Rect(actionRect.x - 1, actionRect.y - 3, actionRect.width + 1, actionRect.height + 4),
                                   _topColor);


                var actionTypeString = "";
                switch (_target.actions[i].type){
                    case DispatchAction.Type.TriggerEvent:
                        actionTypeString = "Throw event : " + _target.actions[i].definedEvent.eventId;
                        break;
                    case DispatchAction.Type.ExecuteMethod:
                        actionTypeString = "Execute method";
                        break;
                    case DispatchAction.Type.Wait:
                        var addS = _target.actions[i].secondsToWait != 1;
                        actionTypeString = "Wait " + _target.actions[i].secondsToWait + " second" + (addS ? "s" : "");
                        break;
                }

                _target.actions[i].foldout = GUIUtilPro.WhiteFoldout(_target.actions[i].foldout, (i + 1) + " - " + actionTypeString);
                
                GUILayout.FlexibleSpace();
                
                
                var removeButtonRect = actionRect.SetX(actionRect.width - 8).AddY(1).SetWidth(20).SetHeight(13);
                
                var moveDownButtonRect = removeButtonRect.AddX(-25);
                var moveUpButtonRect   = moveDownButtonRect.AddX(-15);
                
                if (GUIUtilPro.DrawMoveUpButton(moveUpButtonRect, Event.current) && i > 0){
                    var aux = _target.actions[i];
                    _target.actions[i]     = _target.actions[i - 1];
                    _target.actions[i - 1] = aux;
                
                    GUI.changed = true;
                    GUIUtility.ExitGUI();
                }

                if (GUIUtilPro.DrawMoveDownButton(moveDownButtonRect, Event.current) && i < _target.actions.Count - 1){
                    var aux = _target.actions[i];
                    _target.actions[i]     = _target.actions[i + 1];
                    _target.actions[i + 1] = aux;

                    GUI.changed = true;
                    GUIUtility.ExitGUI();
                }

                var remove = GUIUtilPro.DrawRemoveButton(removeButtonRect, Event.current);
                

                EditorGUILayout.EndHorizontal();

                if (_target.actions[i].foldout){
                    DrawActionField(actionRect, _target.actions[i]);
                }
                
                EditorGUI.indentLevel = 0;
                
                GUILayout.Space(10);

                if (remove){
                    _target.actions.RemoveAt(i);
                    
                    GUI.changed = true;
                }
                
            }
            
            GUILayout.Space(5);
            
            if (GUILayout.Button("+Action", GUILayout.Width(55), GUILayout.Height(20))){
                _target.actions.Add(CreateInstance<DispatchAction>().Configure(DispatchAction.Type.TriggerEvent));
                
                GUI.changed = true;
            }

            #endregion

            GUILayout.Space(5);

            if (GUI.changed) Repaint();
        }

        private void DrawTagFields(Rect tagFoldoutRect){
            var bottomRect = EditorGUILayout.BeginVertical();
            EditorGUI.DrawRect(new Rect(tagFoldoutRect.x - 1, bottomRect.y - 1, tagFoldoutRect.width + 1, bottomRect.height + 1),
                               _botColor);
            
            GUILayout.Space(6);

            for (var i = 0; i < _target.tagFilter.Count; i++){
                    
                var tagRect = EditorGUILayout.BeginHorizontal();
                EditorGUI.DrawRect(new Rect(tagRect.x - 1, tagRect.y - 3, tagRect.width + 1, tagRect.height + 4),
                                   _topColor.Alpha(.7f));
                    
                var removeButtonRect = tagRect.SetX(tagRect.width - 8).AddY(1).SetWidth(20).SetHeight(13);
					
                _target.tagFilter[i] = EditorGUILayout.TagField(_target.tagFilter[i]);
                
                GUILayout.FlexibleSpace();
                
                if (GUIUtilPro.DrawRemoveButton(removeButtonRect, Event.current)){
                    _target.tagFilter.RemoveAt(i);
                    GUI.changed = true;
                }

                EditorGUILayout.EndHorizontal();
                
                GUILayout.Space(4);
            }
			
            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            
            GUILayout.Space(5);
            
            if (GUILayout.Button("+Tag", GUILayout.Width(50), GUILayout.Height(20))){
                _target.tagFilter.Add("");
                
                GUI.changed = true;
            }
                
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Space(5);
            
            EditorGUILayout.EndVertical();
        }
        
        private void DrawColNameFields(Rect nameFoldoutRect){
            
            
            var bottomRect = EditorGUILayout.BeginVertical();
            EditorGUI.DrawRect(new Rect(nameFoldoutRect.x - 1, bottomRect.y - 1, nameFoldoutRect.width + 1, bottomRect.height + 1),
                               _botColor);
            
            GUILayout.Space(6);

            
            for (var i = 0; i < _target.colliderNameFilters.Count; i++){
                    
                var tagRect = EditorGUILayout.BeginHorizontal();
                EditorGUI.DrawRect(new Rect(tagRect.x - 1, tagRect.y - 3, tagRect.width + 1, tagRect.height + 6),
                                   _topColor.Alpha(.7f));
                    
                var removeButtonRect = tagRect.SetX(tagRect.width - 8).AddY(1).SetWidth(20).SetHeight(13);
                
                _target.colliderNameFilters[i] = EditorGUILayout.TextField(_target.colliderNameFilters[i]);
                
                GUILayout.FlexibleSpace();
                
                if (GUIUtilPro.DrawRemoveButton(removeButtonRect, Event.current)){
                    _target.colliderNameFilters.RemoveAt(i);
                    GUI.changed = true;
                }

                EditorGUILayout.EndHorizontal();
                
                GUILayout.Space(4);
            }
			
            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            
            GUILayout.Space(5);
            
            if (GUILayout.Button("+String", GUILayout.Width(55), GUILayout.Height(20))){
                _target.colliderNameFilters.Add("");
                GUI.changed = true;
            }
                
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Space(5);
            
            EditorGUILayout.EndVertical();
            
            EditorGUI.indentLevel = 0;
        }

        private void DrawActionField(Rect topRect, DispatchAction action){
            EditorGUI.indentLevel = 2;
            
            var bottomRect = EditorGUILayout.BeginVertical();
            EditorGUI.DrawRect(new Rect(topRect.x - 1, bottomRect.y - 1, topRect.width + 1, bottomRect.height + 1),
                               _botColor);

            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField("Action Type", GUIUtilPro.WhiteLblStyle, GUILayout.Width(160));
            
            action.type = (DispatchAction.Type) EditorGUILayout.EnumPopup(action.type);
            
            EditorGUILayout.EndHorizontal();
            
            GUIUtilPro.DrawSeparator(3, new Color(.76f,.76f,.76f));

            switch (action.type){
                    case DispatchAction.Type.TriggerEvent:
                        DrawTriggerEventField(action);
                        break;
                    case DispatchAction.Type.ExecuteMethod:
                        DrawExecuteMethodField(action);
                        break;
                    case DispatchAction.Type.Wait:
                        DrawWaitActionField(action);
                        break;
            }
            
            GUILayout.Space(5);
            EditorGUILayout.EndVertical();
            
            
        }

        private void DrawWaitActionField(DispatchAction action){
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField("Seconds", GUIUtilPro.WhiteLblStyle, GUILayout.Width(160));
            action.secondsToWait = EditorGUILayout.FloatField(action.secondsToWait);
            if (action.secondsToWait < 0) action.secondsToWait = 0;
            
            EditorGUILayout.EndHorizontal();
        }

        private void DrawExecuteMethodField(DispatchAction action){
            if (!_serializedObjects.Any()){
                _serializedObjects = _target.actions.ToDictionary(n => n, n => new SerializedObject(n));

                GUI.changed = true;
            }
            
            _serializedObjects[action].Update();
            EditorGUILayout.PropertyField(_serializedObjects[action].FindProperty("onActionTriggered"));
            _serializedObjects[action].ApplyModifiedProperties();
        }
        
        private void DrawTriggerEventField(DispatchAction action){
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField("Event", GUIUtilPro.WhiteLblStyle, GUILayout.Width(160));
            
            var eventId = GUIUtilPro.ListPopupField(action.definedEvent.eventId, EventManager.DefinedEventsList);
            if (eventId != action.definedEvent.eventId){
                action.definedEvent = new DefinedEvent(eventId);
                
                GUI.changed = true;
            }
            
            GUILayout.Space(2);
            
            EditorGUILayout.EndHorizontal();
            
            
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField("Delay", GUIUtilPro.WhiteLblStyle, GUILayout.Width(160));
            action.delay = EditorGUILayout.FloatField(action.delay);
            if (action.delay < 0) action.delay = 0;
            
            GUILayout.Space(2);
            
            EditorGUILayout.EndHorizontal();

            #region Parameters

            action.parametersFoldout = GUIUtilPro.WhiteFoldout(action.parametersFoldout, "Parameters");

            if (action.parametersFoldout){
                EditorGUI.indentLevel = 3;
                foreach (var parameter in action.definedEvent.parameters.Values)
                    DrawParameter(parameter);
            }

            #endregion

        }

        private void DrawParameter(DefinedParameter parameter){
            GUILayout.Space(3);
            var topRect = EditorGUILayout.BeginVertical();

            EditorGUI.DrawRect(new Rect(topRect.x - 1 + 32, topRect.y - 1, topRect.width + 1 - 32, topRect.height + 1),
                               _topColor);

            var hRect = EditorGUILayout.BeginHorizontal();
            parameter.foldout = GUIUtilPro.WhiteFoldout(parameter.foldout, parameter.name);
            
            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            if (!parameter.foldout) return;

            var bottomRect = EditorGUILayout.BeginVertical();
            EditorGUI.DrawRect(new Rect(topRect.x - 1 + 32, bottomRect.y - 1, topRect.width + 1 - 32, bottomRect.height + 1),
                               _botColor);

            GUILayout.Space(5);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Type", GUIUtilPro.WhiteLblStyle, GUILayout.Width(96));
            EditorGUILayout.LabelField(parameter.type.ToString(), GUIUtilPro.CyanLblStyle);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Value", GUIUtilPro.WhiteLblStyle, GUILayout.Width(96));
            
            switch (parameter.type){
                case ParameterDefinition.Type.Bool:
                    if (!(parameter.Value is bool))
                        parameter.Value = default(bool);

                    parameter.Value = GUIUtilPro.BooleanField((bool) parameter.Value);
                    break;
                case ParameterDefinition.Type.Int:
                    if (!(parameter.Value is int))
                        parameter.Value = default(int);

                    parameter.Value = EditorGUILayout.IntField((int) parameter.Value);
                    break;
                case ParameterDefinition.Type.Float:
                    if (!(parameter.Value is float))
                        parameter.Value = default(float);

                    parameter.Value =
                        EditorGUILayout.FloatField((float) parameter.Value);
                    break;
                case ParameterDefinition.Type.String:
                    if (!(parameter.Value is string))
                        parameter.Value = default(string);

                    parameter.Value =
                        EditorGUILayout.TextField((string) parameter.Value);
                    break;
                case ParameterDefinition.Type.GameObject:
                    if (!(parameter.Value is GameObject))
                        parameter.Value = default(GameObject);

                    parameter.Value =
                        EditorGUILayout.ObjectField((GameObject) parameter.Value, typeof(GameObject), true);
                    break;
                case ParameterDefinition.Type.Custom:
                    parameter.Value = ""; //null
                    EditorGUILayout.LabelField("null", GUIUtilPro.CyanLblStyle);
                    break;
                case ParameterDefinition.Type.Vector2:
                    if (!(parameter.Value is Vector2))
                        parameter.Value = default(Vector2);
                    
                    parameter.Value = EditorGUILayout.Vector2Field("", (Vector2) parameter.Value);
                    break;
                case ParameterDefinition.Type.Vector3:
                    if (!(parameter.Value is Vector3))
                        parameter.Value = default(Vector3);

                    parameter.Value = EditorGUILayout.Vector3Field("", (Vector3) parameter.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);
            EditorGUILayout.EndHorizontal();
        }
        
        
    }
    
}