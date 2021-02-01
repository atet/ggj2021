using System;
using System.Collections.Generic;
using KennethDevelops.DataStructures;
using UnityEngine;

namespace KennethDevelops.Events{
    
    [HelpURL("https://github.com/kgazcurra/EventManagerWiki/wiki")]
    public class EventDefinitionsData : ScriptableObject{

        [NonSerialized]
        public List<EventDefinition> eventsToAdd = new List<EventDefinition>();
        public EventDefinitionDictionary events = new EventDefinitionDictionary();

    }
    
    [Serializable]
    public class EventDefinitionDictionary : SerializableDictionary<string, EventDefinition>{ }
}