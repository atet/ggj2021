using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using KennethDevelops.Util;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace KennethDevelops.Events{
    public static class EventManager{

        public delegate void Callback(DefinedEvent eventDefinition);

        //private static Dictionary<string, Callback> _definedEvents = new Dictionary<string, Callback>();
        private static Dictionary<string, OrderedList<Callback>> _definedEvents = new Dictionary<string, OrderedList<Callback>>();
        private static EventDefinitionsData         _eventDefinitions;

        private static Stopwatch _stopwatch;
        private const float MAX_EXECUTION_TIME = 1 / 100f;


        public static void SubscribeToEvent(string eventId, Callback callback, int priority = 100){
            if (!Definitions.events.ContainsKey(eventId)){
                Debug.LogError("[EventManager] Could not find definition for event '" + eventId + "'");
                return;
            }

            if (!_definedEvents.ContainsKey(eventId)) _definedEvents.Add(eventId, null);

            _definedEvents[eventId] += new OrderedElement<Callback>(priority, callback);
        }

        public static void UnsubscribeToEvent(string eventId, Callback callback){
            if (!_definedEvents.ContainsKey(eventId)){
                Debug.LogWarning("[EventManager] UNEXISTENT EVENT: Trying to unsubscribe to event '" + eventId +
                                 "', but nothing has ever subscribed to it");
                return;
            }

            _definedEvents[eventId] -= callback;
        }

        public static void TriggerEvent(string eventId){
            TriggerEvent(eventId, null);
        }

        public static void TriggerEvent(string eventId, DefinedEvent definedEvent){
            if (!_definedEvents.ContainsKey(eventId)){
                Debug.LogWarning("[EventManager] UNEXISTENT EVENT: Trying to trigger event '" + eventId + "', but nothing has ever subscribed to it");
                return;
            }

            if (Definitions.events[eventId].debug){
                var log = "[EventManager] Triggered event '" + eventId + "'";
                if (definedEvent.parameters.Count > 0){
                    log = definedEvent.parameters
                                      .Select(n => n.Value)
                                      .Aggregate(log + " with parameters ",
                                                 (current, parameter) =>
                                                     current +
                                                     ("[ '" + parameter.name + "' = '" + parameter.Value + "' ]"
                                                     ));
                }

                Debug.Log(log);
            }

            if (_definedEvents[eventId] != null)
                CoroutineManager.StartCoroutine(InvokeCallbacks(eventId, definedEvent));
        }

        
        private static IEnumerator InvokeCallbacks(string eventId, DefinedEvent definedEvent){
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
            
            foreach (var callback in _definedEvents[eventId].Select(n => n.element)){
                if (callback != null) callback.Invoke(definedEvent);
                
                if (_stopwatch.ElapsedMilliseconds >= MAX_EXECUTION_TIME){
                    yield return null;
                    _stopwatch.Stop();
                    _stopwatch.Start();
                }
            }
            _stopwatch.Stop();
        }

        public static EventDefinition GetDefinition(string eventId){
            if (!Definitions.events.ContainsKey(eventId)){
                Debug.LogError("[EventManager] Could not find definition for event '" + eventId + "'");
                return null;
            }

            return Definitions.events[eventId];
        }
        
        
        public static List<string> DefinedEventsList{
            get{ return Definitions.events.Keys.Select(n => n).ToList(); }
        }
        
        private static EventDefinitionsData Definitions{
            get{
                if (_eventDefinitions == null){
                    var path = "EventManagerConfig";
                    _eventDefinitions = Resources.Load<EventDefinitionsData>(path);
                    foreach (var parameter in _eventDefinitions.events.SelectMany(n => n.Value.parameters.Select(x => x.Value)))
                        parameter.Load();
                }

                return _eventDefinitions;
            }
        }

    }
}