using System;
using System.Collections.Generic;
using System.Linq;
using KennethDevelops.Events;
using KennethDevelops.Extensions;
using KennethDevelops.Util;
using UnityEditor;
using UnityEngine;

namespace KennethDevelops{

    [CustomEditor(typeof(EventDefinitionsData))]
    public class EventManagerConfigInspector : Editor{

        private EventDefinitionsData _target;

        private Vector2 _scrollViewPos;

        private GUIStyle _titleStyle;
        private GUIStyle _labelStyle;
        private GUIStyle _nullLabelStyle;
        
        private Color _topColor;
        private Color _botColor;
        private Color _headerColor;
        
        private Texture _headerTexture;

        private Texture _openDocBtnTexture;
        private Texture _openDocBtnRolloverTexture;
        
        private Texture _watchTutoBtnTexture;
        private Texture _watchTutoBtnRolloverTexture;

        private string _message;
        private string _searchString = "";


        void OnEnable(){
            _target = (EventDefinitionsData) target;

            _target.eventsToAdd = _target.events.Select(n => n.Value).ToList();
            foreach (var eventDefinition in _target.eventsToAdd)
                eventDefinition.paramsToAdd = eventDefinition.parameters.Select(n => n.Value.Load()).ToList();


            _titleStyle         = new GUIStyle{fontSize = 12, alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold};
            
            _topColor    = new Color(20 / 255f, 39 / 255f, 58 / 255f);
            _botColor    = new Color(29 / 255f, 55 / 255f, 81 / 255f, .7f);
            _headerColor = new Color(211 / 255f, 116 / 255f, 22 / 255f);
            
            _labelStyle         = new GUIStyle{normal = {textColor = Color.white}};
            _nullLabelStyle     = new GUIStyle{normal = {textColor = Color.cyan}};
            
            LoadTextures();
            
            _message = null;
        }

        private void LoadTextures(){
            _headerTexture               = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Plugins/KennethDevelops/Resources/GUI/EventManagerConfigHeader.png");
            
            _openDocBtnTexture           = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Plugins/KennethDevelops/Resources/GUI/OpenDocumentationBtn.png");
            _openDocBtnRolloverTexture   = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Plugins/KennethDevelops/Resources/GUI/OpenDocumentationBtnRollOver.png");
            
            _watchTutoBtnTexture         = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Plugins/KennethDevelops/Resources/GUI/WatchTutorialsBtn.png");
            _watchTutoBtnRolloverTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Plugins/KennethDevelops/Resources/GUI/WatchTutorialsBtnRollOver.png");
        }

        public override void OnInspectorGUI(){
            GUILayout.Space(5);

            #region Header
            
            var rect = EditorGUILayout.BeginVertical();
            
            if (_headerTexture == null) LoadTextures();
            
            EditorGUI.DrawRect(rect.AddY(-4).SetHeight(_headerTexture.height + 8), _headerColor);
            
            rect = rect.AddX(rect.width / 2 - _headerTexture.width / 2)
                       .SetWidth(_headerTexture.width)
                       .SetHeight(_headerTexture.height);
            
            GUI.DrawTexture(rect, _headerTexture);
            
            EditorGUILayout.EndVertical();
            
            GUILayout.Space(_headerTexture.height + 10);

            #endregion

            #region Header Buttons

            rect = EditorGUILayout.BeginHorizontal();
            
            

            var openDocRect = rect.SetX(4).AddY(1);

            
            if (GUIUtilPro.DrawCustomButton(openDocRect, Event.current, _openDocBtnTexture,
                                            _openDocBtnRolloverTexture)){
                Application.OpenURL("https://github.com/kgazcurra/EventManagerWiki/wiki");
            }
            
            var watchTutoRect = rect.SetX(openDocRect.x + _openDocBtnTexture.width + 10).AddY(1);

            if (GUIUtilPro.DrawCustomButton(watchTutoRect, Event.current, _watchTutoBtnTexture,
                                            _watchTutoBtnRolloverTexture)){
                Application.OpenURL("https://www.youtube.com/watch?v=VS76Eoe5ltY&list=PLfp08H40lhEAKZl66_DX-_Fygs93IcpV5");
            }

            GUILayout.Space(watchTutoRect.x + _watchTutoBtnTexture.width + 10);

            #region Search Bar

            EditorGUILayout.BeginVertical();
            
            GUILayout.Space(15);
            
            _searchString = EditorGUILayout.TextField(_searchString);
            EditorGUILayout.LabelField("Search by name", GUILayout.Width(100));
            
            EditorGUILayout.EndVertical();

            #endregion
            
            EditorGUILayout.EndHorizontal();
            
            #endregion

            GUILayout.Space(5);
            GUIUtilPro.DrawSeparator(10);

            _scrollViewPos = EditorGUILayout.BeginScrollView(_scrollViewPos);

            #region Events
            
            for (var i = 0; i < _target.eventsToAdd.Count; i++){
                var eventConfig = _target.eventsToAdd[i];

                //Ensures that we are only showing event definitions that matches the _searchString
                if (!String.IsNullOrEmpty(_searchString) && !eventConfig.name.ToLower().Contains(_searchString.ToLower()))
                    continue;
                
                GUILayout.Space(5);

                var topRect = EditorGUILayout.BeginVertical();
                EditorGUI.DrawRect(new Rect(topRect.x - 1, topRect.y - 1, topRect.width + 1, topRect.height + 1),
                                   _topColor);

                EditorGUI.indentLevel = 1;

                var hRect = EditorGUILayout.BeginHorizontal();
                eventConfig.foldout = GUIUtilPro.WhiteFoldout(eventConfig.foldout, eventConfig.name);
                GUILayout.FlexibleSpace();

                var removeButtonRect = hRect.SetX(hRect.width - 24).AddY(1).SetWidth(20).SetHeight(13);
                
                var moveDownButtonRect = removeButtonRect.AddX(-25);
                var moveUpButtonRect   = moveDownButtonRect.AddX(-15);
            
                if (GUIUtilPro.DrawMoveUpButton(moveUpButtonRect, Event.current) && i > 0){
                    var aux = _target.eventsToAdd[i];
                    _target.eventsToAdd[i]     = _target.eventsToAdd[i - 1];
                    _target.eventsToAdd[i - 1] = aux;
                
                    GUI.changed = true;
                    GUIUtility.ExitGUI();
                }

                if (GUIUtilPro.DrawMoveDownButton(moveDownButtonRect, Event.current) && i < _target.eventsToAdd.Count - 1){
                    var aux = _target.eventsToAdd[i];
                    _target.eventsToAdd[i]     = _target.eventsToAdd[i + 1];
                    _target.eventsToAdd[i + 1] = aux;

                    GUI.changed = true;
                    GUIUtility.ExitGUI();
                }
                
                if (GUIUtilPro.DrawRemoveButton(removeButtonRect, Event.current))
                    RemoveEvent(eventConfig);
                

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();

                if (!eventConfig.foldout) continue;

                var bottomRect = EditorGUILayout.BeginVertical();
                EditorGUI.DrawRect(new Rect(bottomRect.x - 1, bottomRect.y - 1, bottomRect.width + 1, bottomRect.height + 1),
                                   _botColor);

                GUILayout.Space(5);

                EditorGUI.indentLevel = 2;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Name", _labelStyle, GUILayout.Width(110));
                eventConfig.name      = EditorGUILayout.TextField(eventConfig.name);
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Debug", _labelStyle, GUILayout.Width(110));
                eventConfig.debug     = EditorGUILayout.Toggle(eventConfig.debug);
                EditorGUILayout.EndHorizontal();

                #region Test Event
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Test", _labelStyle, GUILayout.Width(140));
                
                EditorGUI.BeginDisabledGroup(!EditorApplication.isPlaying);
                
                if (GUILayout.Button("Trigger Event")) 
                    new DefinedEvent(eventConfig.name).Trigger();
                
                EditorGUI.EndDisabledGroup();
                
                EditorGUILayout.EndHorizontal();

                #endregion
                
                eventConfig.foldoutParameters = GUIUtilPro.WhiteFoldout(eventConfig.foldoutParameters, "Parameters");
                if (eventConfig.foldoutParameters){
                    EditorGUI.indentLevel = 3;

                    for (var x = 0; x < eventConfig.paramsToAdd.Count; x++){
                        GUILayout.Space(3);
                        DrawParameter(eventConfig, eventConfig.paramsToAdd[x], eventConfig.paramsToAdd, x);
                    }

                    GUILayout.Space(5);

                    var buttonRect = EditorGUILayout.BeginVertical();
                    if (GUI.Button(buttonRect.AddX(32).SetWidth(82).SetHeight(19), "+Parameter"))
                        AddParameter(eventConfig);

                    GUILayout.Space(19);

                    EditorGUILayout.EndVertical();
                }

                GUILayout.Space(5);

                EditorGUILayout.EndVertical();
            }            
            
            #endregion
            
            GUILayout.Space(5);

            if (GUILayout.Button("+Event", GUILayout.Width(55)))
                AddEvent();

            GUILayout.FlexibleSpace();

            EditorGUILayout.EndScrollView();
            
            if (_message != null){
                EditorGUILayout.HelpBox(_message, MessageType.Error);
            }

            if (GUILayout.Button("Save changes", GUILayout.Height(40)))
                SaveChanges();
                


            if (GUI.changed) Repaint();
        }

        private void DrawParameter(EventDefinition eventDefinition, ParameterDefinition parameter, List<ParameterDefinition> paramsToAdd, int index){
            var topRect = EditorGUILayout.BeginVertical();

            EditorGUI.DrawRect(new Rect(topRect.x - 1 + 32, topRect.y - 1, topRect.width + 1 - 32, topRect.height + 1),
                               _topColor);

            var hRect = EditorGUILayout.BeginHorizontal();
            parameter.foldout = GUIUtilPro.WhiteFoldout(parameter.foldout, parameter.name);
            
            GUILayout.FlexibleSpace();
            
            var removeButtonRect = hRect.SetX(hRect.width - 24).AddY(1).SetWidth(20).SetHeight(13);

            var moveDownButtonRect = removeButtonRect.AddX(-25);
            var moveUpButtonRect   = moveDownButtonRect.AddX(-15);
            
            if (GUIUtilPro.DrawMoveUpButton(moveUpButtonRect, Event.current) && index > 0){
                var aux = paramsToAdd[index];
                paramsToAdd[index] = paramsToAdd[index - 1];
                paramsToAdd[index - 1] = aux;
                
                GUI.changed = true;
                GUIUtility.ExitGUI();
            }

            if (GUIUtilPro.DrawMoveDownButton(moveDownButtonRect, Event.current) && index < paramsToAdd.Count - 1){
                var aux = paramsToAdd[index];
                paramsToAdd[index]     = paramsToAdd[index + 1];
                paramsToAdd[index + 1] = aux;
                
                GUI.changed = true;
                GUIUtility.ExitGUI();
            }


            if (GUIUtilPro.DrawRemoveButton(removeButtonRect, Event.current))
                RemoveParameter(eventDefinition, parameter);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            if (!parameter.foldout) return;

            var bottomRect = EditorGUILayout.BeginVertical();
            EditorGUI.DrawRect(new Rect(topRect.x - 1 + 32, bottomRect.y - 1, topRect.width + 1 - 32, bottomRect.height + 1),
                               _botColor);

            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Name", _labelStyle, GUILayout.Width(96));
            parameter.name = EditorGUILayout.TextField(parameter.name);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Type", _labelStyle, GUILayout.Width(96));
            parameter.type = (ParameterDefinition.Type) EditorGUILayout.EnumPopup(parameter.type);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Default Value", _labelStyle, GUILayout.Width(96));
            
            switch (parameter.type){
                case ParameterDefinition.Type.Bool:
                    if (!(parameter.DefaultValue is bool))
                        parameter.DefaultValue = default(bool);

                    parameter.DefaultValue = GUIUtilPro.BooleanField((bool) parameter.DefaultValue);
                    break;
                case ParameterDefinition.Type.Int:
                    if (!(parameter.DefaultValue is int))
                        parameter.DefaultValue = default(int);

                    parameter.DefaultValue = EditorGUILayout.IntField((int) parameter.DefaultValue);
                    break;
                case ParameterDefinition.Type.Float:
                    if (!(parameter.DefaultValue is float))
                        parameter.DefaultValue = default(float);

                    parameter.DefaultValue =
                        EditorGUILayout.FloatField((float) parameter.DefaultValue);
                    break;
                case ParameterDefinition.Type.String:
                    if (!(parameter.DefaultValue is string))
                        parameter.DefaultValue = default(string);

                    parameter.DefaultValue =
                        EditorGUILayout.TextField((string) parameter.DefaultValue);
                    break;
                case ParameterDefinition.Type.GameObject:
                case ParameterDefinition.Type.Custom:
                    parameter.DefaultValue = ""; //null
                    EditorGUILayout.LabelField("null", _nullLabelStyle);
                    break;
                case ParameterDefinition.Type.Vector2:
                    if (!(parameter.DefaultValue is SerializableVector2))
                        parameter.DefaultValue = default(SerializableVector2);
                    
                    parameter.DefaultValue = (SerializableVector2) EditorGUILayout.Vector2Field("", (SerializableVector2) parameter.DefaultValue);
                    break;
                case ParameterDefinition.Type.Vector3:
                    if (!(parameter.DefaultValue is SerializableVector3))
                        parameter.DefaultValue = default(SerializableVector3);

                    parameter.DefaultValue = (SerializableVector3) EditorGUILayout.Vector3Field("", (SerializableVector3) parameter.DefaultValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);
            EditorGUILayout.EndHorizontal();
        }

        private void AddEvent(){
            var eventConfig = new EventDefinition{name = "New Event"};
            _target.eventsToAdd.Add(eventConfig);

            GUI.changed = true;
        }

        private void RemoveEvent(EventDefinition eventDefinition){
            _target.eventsToAdd.Remove(eventDefinition);
            GUI.changed = true;
        }

        private void AddParameter(EventDefinition eventDefinition){
            var parameter = new ParameterDefinition{name = "New Parameter"};
            eventDefinition.paramsToAdd.Add(parameter);

            GUI.changed = true;
        }

        private void RemoveParameter(EventDefinition eventDefinition, ParameterDefinition parameter){
            eventDefinition.paramsToAdd.Remove(parameter);

            GUI.changed = true;
        }

        private void SaveChanges(){
            if (HasDuplicatedKeys()) return;

            //Save in the dictionary
            _target.events = new EventDefinitionDictionary();
            foreach (var eventDefinition in _target.eventsToAdd){
                eventDefinition.parameters = new ParameterDefinitionDictionary();
                foreach (var parameter in eventDefinition.paramsToAdd)
                    eventDefinition.parameters.Add(parameter.name, parameter.Save());

                _target.events.Add(eventDefinition.name, eventDefinition);
            }

            GUIUtilPro.SaveChanges(_target);
        }

        private bool HasDuplicatedKeys(){
            var hasDuplicatedEvents = _target.eventsToAdd
                                             .Select(n => n.name)
                                             .GroupBy(n => n)
                                             .Any(n => n.Count() > 1);
            if (hasDuplicatedEvents){
                _message    = "Found Events with the same name. Please rename them and try again to save changes";
                GUI.changed = true;
                return true;
            }

            foreach (var eventDefinition in _target.eventsToAdd){
                var hasDuplicatedParams = eventDefinition.paramsToAdd
                                                         .Select(n => n.name)
                                                         .GroupBy(n => n)
                                                         .Any(n => n.Count() > 1);
                if (hasDuplicatedParams){
                    _message = "Found Parameters with the same name on event '" + eventDefinition.name +
                               "'. Please rename them and try again to save changes";
                    GUI.changed = true;
                    return true;
                }
            }

            return false;
        }
    }
}