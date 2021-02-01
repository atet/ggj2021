using System.IO;
using KennethDevelops.Events;
using UnityEditor;
using UnityEngine;

namespace KennethDevelops{
    public static class EventManagerConfigMenu{
        
        private const string PATH = "Assets/Plugins/KennethDevelops/Resources/EventManagerConfig.asset";
        
        [MenuItem("KennethDevelops/Event Manager Config")]
        public static void OpenConfig(){
            

            if (!File.Exists(Application.dataPath + "/Plugins/KennethDevelops/Resources/EventManagerConfig.asset")){
                var instance = ScriptableObject.CreateInstance<EventDefinitionsData>();
                AssetDatabase.CreateAsset(instance, PATH);
            }
            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(PATH);
        }
        
    }
}