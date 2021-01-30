using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace KennethDevelops.Events{
    
    [Serializable]
    public class DispatchAction : ScriptableObject{

        public Type         type              = Type.TriggerEvent;
        
        public bool         foldout           = true;
        public bool         parametersFoldout = true;
        
        public DefinedEvent definedEvent;
        public float        delay         = 0;
        public float        secondsToWait = 0;
        
        public UnityEvent   onActionTriggered;


        public DispatchAction Configure(Type type){
            this.type = type;
            if (type == Type.TriggerEvent){
                var eventId = EventManager.DefinedEventsList.FirstOrDefault();
                definedEvent = new DefinedEvent(eventId);
            }

            return this;
        }

        public IEnumerator Execute(){
            switch (type){
                case Type.TriggerEvent:
                    definedEvent.Trigger(delay);
                    break;
                case Type.ExecuteMethod:
                    if (onActionTriggered != null) onActionTriggered.Invoke();
                    break;
                case Type.Wait:
                    yield return new WaitForSeconds(secondsToWait);
                    break;
            }

            yield return null;
        }


        public enum Type{
            TriggerEvent,
            ExecuteMethod,
            Wait
        }
    }
    
}