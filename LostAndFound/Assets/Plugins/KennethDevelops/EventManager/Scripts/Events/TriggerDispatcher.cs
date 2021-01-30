using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KennethDevelops.ProLibrary.Extensions;
using KennethDevelops.ProTrigger.Util;
using KennethDevelops.Util;
using UnityEngine;

namespace KennethDevelops.Events{
    
    [HelpURL("https://github.com/kgazcurra/EventManagerWiki/wiki/TriggerDispatcher")]
    public class TriggerDispatcher : MonoBehaviour{

        public  List<TriggerCondition> triggerConditions  = new List<TriggerCondition>();
        
        public  GizmosDrawType         drawGizmos         = GizmosDrawType.Always;
        public  Color                  gizmosOutlineColor = new Color(0,.86f,1f);
        public  Color                  gizmosFillColor    = new Color(0,.86f,1f,.3f);
        public  bool                   gizmosFoldout;
        
        public  LayerMask              layerFilter = ~0;
        
        public  List<string>           tagFilter   = new List<string>();
        public  bool                   tagFoldout;
        
        public  List<string>           colliderNameFilters = new List<string>();
        public  bool                   colliderNameFoldout;
        
        public  List<DispatchAction>   actions = new List<DispatchAction>();
        
        private Collider[]             _colliders;
        private Collider2D[]           _colliders2D;


        public bool HasCollider(){
            _colliders = GetComponents<Collider>();

            return _colliders.Any();
        }
        
        public bool HasCollider2D(){
            _colliders2D = GetComponents<Collider2D>();

            return _colliders2D.Any();
        }

        #region 3D Collision/Triggers

        private void OnTriggerEnter(Collider other){
            if (triggerConditions.All(t => t != TriggerCondition.OnTriggerEnter)) return;

            StartCoroutine(PerformAction(other.gameObject));

        }

        private void OnTriggerStay(Collider other){
            if (triggerConditions.All(t => t != TriggerCondition.OnTriggerStay)) return;
            
            StartCoroutine(PerformAction(other.gameObject));
        }
        
        private void OnTriggerExit(Collider other){
            if (triggerConditions.All(t => t != TriggerCondition.OnTriggerExit)) return;
            
            StartCoroutine(PerformAction(other.gameObject));
        }

        private void OnCollisionEnter(Collision other){
            if (triggerConditions.All(t => t != TriggerCondition.OnCollisionEnter)) return;
            
            StartCoroutine(PerformAction(other.gameObject));
        }
        
        private void OnCollisionStay(Collision other){
            if (triggerConditions.All(t => t != TriggerCondition.OnCollisionStay)) return;
            
            StartCoroutine(PerformAction(other.gameObject));
        }
        
        private void OnCollisionExit(Collision other){
            if (triggerConditions.All(t => t != TriggerCondition.OnCollisionExit)) return;
            
            StartCoroutine(PerformAction(other.gameObject));
        }

        #endregion
        

        #region 2D Collision/Triggers
        
        private void OnTriggerEnter2D(Collider2D other){
            if (triggerConditions.All(t => t != TriggerCondition.OnTriggerEnter)) return;

            StartCoroutine(PerformAction(other.gameObject));
        }
        
        private void OnTriggerStay2D(Collider2D other){
            if (triggerConditions.All(t => t != TriggerCondition.OnTriggerStay)) return;
            
            StartCoroutine(PerformAction(other.gameObject));
        }
        
        private void OnTriggerExit2D(Collider2D other){
            if (triggerConditions.All(t => t != TriggerCondition.OnTriggerExit)) return;
            
            StartCoroutine(PerformAction(other.gameObject));
        }

        private void OnCollisionEnter2D(Collision2D other){
            if (triggerConditions.All(t => t != TriggerCondition.OnCollisionEnter)) return;
            
            StartCoroutine(PerformAction(other.gameObject));
        }
        
        private void OnCollisionStay2D(Collision2D other){
            if (triggerConditions.All(t => t != TriggerCondition.OnCollisionStay)) return;
            
            StartCoroutine(PerformAction(other.gameObject));
        }
        
        private void OnCollisionExit2D(Collision2D other){
            if (triggerConditions.All(t => t != TriggerCondition.OnCollisionExit)) return;
            
            StartCoroutine(PerformAction(other.gameObject));
        }
        
        #endregion


        private IEnumerator PerformAction(GameObject other){
            if (colliderNameFilters.Any() && !colliderNameFilters.Any(n => other.name.Contains(n))) yield break;
            
            if (tagFilter.Any() && !tagFilter.Contains(other.tag)) yield break;
            
            if (!layerFilter.IncludesLayer(other.layer)) yield break;

            foreach (var action in actions){
                var enumerator = action.Execute();
                if (enumerator != null) yield return enumerator;
            }
        }

        #region Gizmos

        private void OnDrawGizmos(){
            if (drawGizmos != GizmosDrawType.Always) return;

            _colliders = GetComponents<Collider>();

            foreach (var col in _colliders)
                GizmosUtil.DrawCollider(transform, col, gizmosOutlineColor, gizmosFillColor);

            
            _colliders2D = GetComponents<Collider2D>();

            foreach (var col in _colliders2D)
                GizmosUtil.DrawCollider2D(transform, col, gizmosOutlineColor, gizmosFillColor);
        }

        private void OnDrawGizmosSelected(){
            if (drawGizmos != GizmosDrawType.OnSelected) return;

            _colliders = GetComponents<Collider>();

            foreach (var col in _colliders)
                GizmosUtil.DrawCollider(transform, col, gizmosOutlineColor, gizmosFillColor);
            
            
            _colliders2D = GetComponents<Collider2D>();

            foreach (var col in _colliders2D)
                GizmosUtil.DrawCollider2D(transform, col, gizmosOutlineColor, gizmosFillColor);
        }
        
        #endregion
    }
    
}