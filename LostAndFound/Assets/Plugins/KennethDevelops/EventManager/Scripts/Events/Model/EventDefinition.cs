using System;
using System.Collections.Generic;
using KennethDevelops.Events;
using KennethDevelops.DataStructures;

namespace KennethDevelops{
    [Serializable]
    public class EventDefinition{
        
        public string name;
        public bool debug;
        
        public ParameterDefinitionDictionary parameters = new ParameterDefinitionDictionary();

        [NonSerialized] public List<ParameterDefinition> paramsToAdd = new List<ParameterDefinition>();
        
        public bool foldout           = true;
        public bool foldoutParameters = true;
        

        public void SaveParameters(){
            foreach (var parameter in paramsToAdd)
                parameters[parameter.name] = parameter;
        }
    }

    [Serializable]
    public class ParameterDefinitionDictionary : SerializableDictionary<string, ParameterDefinition>{ }
}