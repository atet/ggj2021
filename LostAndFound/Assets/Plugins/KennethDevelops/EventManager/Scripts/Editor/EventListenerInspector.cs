using KennethDevelops.Events;
using UnityEditor;
using UnityEngine;

namespace KennethDevelops.Util{
    
    [CustomEditor(typeof(EventListener))]
    public class EventListenerInspector : Editor{

        private EventListener _target;

        private SerializedProperty _methodToExecute;
        

        private void OnEnable(){
            _target          = (EventListener) target;
            
            _methodToExecute = serializedObject.FindProperty("methodToExecute");
        }

        public override void OnInspectorGUI(){
            GUILayout.Space(10);
            
            _target.definedEvent = GUIUtilPro.ListPopupField("Event to Subscribe to", _target.definedEvent, EventManager.DefinedEventsList);
            _target.priority = EditorGUILayout.IntField("Priority", _target.priority);

            GUILayout.Space(5);

            EditorGUILayout.PropertyField(_methodToExecute);

            if (GUI.changed)
                serializedObject.ApplyModifiedProperties();
        }
    }
    
}