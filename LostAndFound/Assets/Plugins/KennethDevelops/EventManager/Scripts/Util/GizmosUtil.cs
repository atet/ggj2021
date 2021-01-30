using UnityEngine;

namespace KennethDevelops.ProTrigger.Util{
    
    internal static class GizmosUtil{

        public static void DrawCollider(Transform transform, Collider collider, Color outlineColor, Color fillColor){
            if (collider is BoxCollider)
                DrawBoxCollider(transform, (BoxCollider) collider, outlineColor, fillColor);
            else if (collider is SphereCollider)
                DrawSphereCollider(transform, (SphereCollider) collider, outlineColor, fillColor);
            else if (collider is CapsuleCollider)
                DrawCapsuleCollider(transform, (CapsuleCollider) collider, outlineColor, fillColor);
        }

        public static void DrawCollider2D(Transform transform, Collider2D collider, Color outlineColor,
                                          Color     fillColor){
            
            var rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            var matrix         = Gizmos.matrix;
            Gizmos.matrix = rotationMatrix;
            
            if (collider is BoxCollider2D){
                var boxCollider = (BoxCollider2D) collider;
                
                Gizmos.color = fillColor;
                
                Gizmos.DrawCube(boxCollider.offset, boxCollider.size);
            
                Gizmos.color = outlineColor;
                Gizmos.DrawWireCube(boxCollider.offset, boxCollider.size);
            }
            else if (collider is CircleCollider2D){
                var circleCollider = (CircleCollider2D) collider;
                Gizmos.color = fillColor;
                
                Gizmos.DrawSphere(circleCollider.offset, circleCollider.radius);
            
                Gizmos.color = outlineColor;
                Gizmos.DrawWireSphere(circleCollider.offset, circleCollider.radius);
            }
            
            Gizmos.matrix = matrix;
                
        }
        
        private static void DrawBoxCollider(Transform transform, BoxCollider collider, Color outlineColor, Color fillColor){
            var rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            var matrix         = Gizmos.matrix;
            Gizmos.matrix = rotationMatrix;
            
            Gizmos.color = fillColor;
            Gizmos.DrawCube(collider.center, collider.size);
            
            Gizmos.color = outlineColor;
            Gizmos.DrawWireCube(collider.center, collider.size);
            
            Gizmos.matrix = matrix;
        }

        private static void DrawSphereCollider(Transform transform, SphereCollider collider, Color outlineColor,
                                               Color     fillColor){
            var rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            var matrix         = Gizmos.matrix;
            Gizmos.matrix = rotationMatrix;
            
            Gizmos.color = fillColor;
            Gizmos.DrawSphere(collider.center, collider.radius);
            
            Gizmos.color = outlineColor;
            Gizmos.DrawWireSphere(collider.center, collider.radius);
            
            Gizmos.matrix = matrix;
        }

        private static void DrawCapsuleCollider(Transform transform, CapsuleCollider collider, Color outlineColor,
                                                Color     fillColor){
            var rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            var matrix         = Gizmos.matrix;
            Gizmos.matrix = rotationMatrix;

            #region Top Sphere

            var topCenter = collider.center + (collider.height / 2 - collider.radius) * Vector3.up;
            
            //Gizmos.color = fillColor;
            //Gizmos.DrawSphere(topCenter, collider.radius);
            
            Gizmos.color = outlineColor;
            Gizmos.DrawWireSphere(topCenter, collider.radius);
            
            #endregion
            
            #region Bottom Sphere
            
            var bottomCenter = collider.center + (collider.height / 2 - collider.radius) * Vector3.down;
            
            //Gizmos.color = fillColor;
            //Gizmos.DrawSphere(bottomCenter, collider.radius);
            
            
            Gizmos.color = outlineColor;
            Gizmos.DrawWireSphere(bottomCenter, collider.radius);
            
            #endregion

            var boxRotationMatrix = Matrix4x4.TRS(transform.position, Quaternion.AngleAxis(45, Vector3.up) * transform.rotation, transform.lossyScale);
            Gizmos.matrix = boxRotationMatrix;
            //1.4142f is the square root of 2
            var size = 1.4142f * collider.radius;
            
            //Gizmos.color = fillColor;
            //Gizmos.DrawCube(collider.center, new Vector3(size, collider.height - collider.radius * 2, size));
            
            Gizmos.color = outlineColor;
            Gizmos.DrawWireCube(collider.center, new Vector3(size, collider.height - collider.radius * 2, size));
            
            
            Gizmos.matrix = matrix;
        }
        
    }
    
}