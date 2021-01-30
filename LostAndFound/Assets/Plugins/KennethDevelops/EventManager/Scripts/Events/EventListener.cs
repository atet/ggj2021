using UnityEngine;
using UnityEngine.Events;

namespace KennethDevelops.Events{

    [HelpURL("https://github.com/kgazcurra/EventManagerWiki/wiki/EventListener")]
    public class EventListener : MonoBehaviour{

        public string     definedEvent;
        public int        priority = 100;
        public UnityEvent methodToExecute;


        void Awake(){
            if (definedEvent != "")
                EventManager.SubscribeToEvent(definedEvent, OnEventTriggered, priority);
        }

        private void OnEventTriggered(DefinedEvent e){
            if (methodToExecute != null) methodToExecute.Invoke();
        }
        
    }
    
}