using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;
using KennethDevelops.Extensions;

namespace KennethDevelops.Util{
    
    public static class GUIUtilPro{
        
        /// <summary>
        /// Creates a thin gray line to denote a separation
        /// </summary>
        public static void DrawSeparator(){
            GUILayout.Space(5);
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 1), Color.gray); 
            GUILayout.Space(5);
        }
        
        /// <summary>
        /// Creates a thin gray line to denote a separation
        /// </summary>
        public static void DrawSeparator(float pixelSpace){
            GUILayout.Space(pixelSpace);
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 1), Color.gray); 
            GUILayout.Space(pixelSpace);
        }
        
        /// <summary>
        /// Creates a thin gray line to denote a separation
        /// </summary>
        public static void DrawSeparator(Color color){
            GUILayout.Space(5);
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 1), color); 
            GUILayout.Space(5);
        }
        
        /// <summary>
        /// Creates a thin gray line to denote a separation
        /// </summary>
        public static void DrawSeparator(float pixelSpace, Color color){
            GUILayout.Space(pixelSpace);
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 1), color); 
            GUILayout.Space(pixelSpace);
        }
        
        /// <summary>
        /// Creates an are where every thing that's put there gets centered
        /// </summary>
        public static void BeginCenteredArea(){
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
        }

        public static void EndCenteredArea(){
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        #region Fields
        
        /// <summary>
        /// Draws a string List popup field, which allows you to easily select an element of that list through a popup field
        /// </summary>
        /// <param name="selected"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ListPopupField(string selected, List<string> list){
            return ListPopupField("", selected, list, n => n);
        }

        /// <summary>
        /// Draws a string List popup field, which allows you to easily select an element of that list through a popup field
        /// </summary>
        /// <param name="label"></param>
        /// <param name="selected"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ListPopupField(string label, string selected, List<string> list){
            return ListPopupField(label, selected, list, n => n);
        }
        
        /// <summary>
        /// Draws a List popup field, which allows you to easily select an element of that list through a popup field
        /// </summary>
        /// <param name="selected"></param>
        /// <param name="list"></param>
        /// <param name="nameSelector"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ListPopupField<T>(T selected, List<T> list, Func<T, string> nameSelector){
            return ListPopupField("", selected, list, nameSelector);
        }
        
        /// <summary>
        /// Draws a string List popup field, which allows you to easily select an element of that list through a popup field
        /// </summary>
        /// <param name="label"></param>
        /// <param name="selected"></param>
        /// <param name="list"></param>
        /// <param name="nameSelector"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ListPopupField<T>(string label, T selected, List<T> list, Func<T, string> nameSelector){
            if (list == null || !list.Any()){
                Debug.LogError("List contains no elements");
                return default(T);
            }

            if (list.IndexOf(selected) < 0)
                selected = list[0];
                
            var index = EditorGUILayout.Popup(label, list.IndexOf(selected), list.Select(nameSelector).ToArray());

            return list[index];
        }
        
        /// <summary>
        /// Draws a LayerMask field
        /// </summary>
        /// <param name="label"></param>
        /// <param name="layerMask"></param>
        /// <returns></returns>
        public static LayerMask LayerMaskField(string label, LayerMask layerMask, bool whiteText = false){
            LoadFoldoutStyle();
            
            EditorGUILayout.BeginHorizontal();

            if (!whiteText) EditorGUILayout.LabelField(label);
            else            EditorGUILayout.LabelField(label, _whiteLabelStyle);
		
            var mask = EditorGUILayout.MaskField(InternalEditorUtility.LayerMaskToConcatenatedLayersMask(layerMask), InternalEditorUtility.layers);
            mask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(mask);
		
            EditorGUILayout.EndHorizontal();

            return mask;
        }
        
        /// <summary>
        /// Draws a boolean field. Instead of the checkbox, you can select 'true' or 'false' through a popup
        /// </summary>
        /// <param name="label"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool BooleanField(string label, bool value){
            EditorGUILayout.BeginHorizontal();
		
            if (label != "") EditorGUILayout.LabelField(label);
            //first param is index, not VALUE, 1 -> false, 0 -> true, when it should be 0 -> false, 1 -> true
            var intValue = EditorGUILayout.Popup(value ? 1 : 0, new string[]{"false", "true"});
		
            EditorGUILayout.EndHorizontal();

            return intValue == 1;
        }

        /// <summary>
        /// Draws a boolean field. Instead of the checkbox, you can select 'true' or 'false' through a popup
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool BooleanField(bool value){
            return BooleanField("", value);
        }

        #endregion

        #region Handles
        
        /// <summary>
        /// Draws a 2D Grid
        /// </summary>
        /// <param name="gridSpacing"></param>
        /// <param name="gridRect"></param>
        /// <param name="offset"></param>
        /// <param name="lineColor"></param>
        public static void DrawGrid(float gridSpacing, Rect gridRect, Vector2 offset, Color lineColor){
            var widthDivisions  = Mathf.CeilToInt(gridRect.width / gridSpacing);
            var heightDivisions = Mathf.CeilToInt(gridRect.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = lineColor;

            var newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

            for(var i = 0; i <= widthDivisions; i++) {
                var origin      = new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset;
                var destination = new Vector3(gridSpacing * i, gridRect.height + gridSpacing, 0) + newOffset;
                Handles.DrawLine(origin, destination);
            }

            for(var i = 0; i <= heightDivisions; i++) {
                var origin      = new Vector3(-gridSpacing, gridSpacing * i, 0) + newOffset;
                var destination = new Vector3(gridRect.width + gridSpacing, gridSpacing * i, 0) + newOffset;
                Handles.DrawLine(origin, destination);
            }

            Handles.EndGUI();
        }
        
        
        private const float DEFAULT_ARROW_RADIUS = 5f;
	
        /// <summary>
        /// Draws an arrow
        /// </summary>
        /// <param name="middlePoint"></param>
        /// <param name="direction"></param>
        /// <param name="size"></param>
        public static void DrawArrow(Vector2 middlePoint, Vector2 direction, float size = DEFAULT_ARROW_RADIUS){
            var arrowRight = new Vector2(size, 0);
            var arrowUp    = new Vector2(-size * Mathf.Cos(60f.Deg2Rad()), size * Mathf.Sin(60f.Deg2Rad()));
            var arrowDown  = new Vector2(-size * Mathf.Cos(60f.Deg2Rad()), -size * Mathf.Sin(60f.Deg2Rad()));

            Handles.DrawAAConvexPolygon(middlePoint + arrowRight.FaceDirection(direction),
                                        middlePoint + arrowUp.FaceDirection(direction),
                                        middlePoint + arrowDown.FaceDirection(direction));
        }

        #endregion

        #region Foldout

        private static Texture2D _foldoutTrue;
        private static Texture2D _foldoutFalse;
	
        private static GUIStyle _whiteLabelStyle;
        private static GUIStyle _cyanLabelStyle;
        
        /// <summary>
        /// Draws a foldout with white label and white arrow icon
        /// </summary>
        /// <param name="foldout"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public static bool WhiteFoldout(bool foldout, string label){
            if (_whiteLabelStyle == null) LoadFoldoutStyle();
		
            var rect = EditorGUILayout.BeginHorizontal();

            var textRect = rect.AddX(15*(EditorGUI.indentLevel-1)).SetWidth(13).SetHeight(13);
		
            GUI.DrawTexture(textRect, foldout ? _foldoutTrue : _foldoutFalse);

            foldout = EditorGUILayout.Foldout(foldout, "   " + label, true, _whiteLabelStyle);
            
            EditorGUILayout.EndHorizontal();

            return foldout;
        }
	
        private static GUIStyle LoadFoldoutStyle(){
            const string path = "Assets/Plugins/KennethDevelops/Resources/GUI/";
            _foldoutTrue  = AssetDatabase.LoadAssetAtPath<Texture2D>(path + "FoldoutTrue.png");
            _foldoutFalse = AssetDatabase.LoadAssetAtPath<Texture2D>(path + "FoldoutFalse.png");
            
            _whiteLabelStyle = new GUIStyle{normal = new GUIStyleState{textColor = Color.white}};

            return _whiteLabelStyle;
        }

        private static void LoadStyles(){
            _whiteLabelStyle = new GUIStyle{normal = new GUIStyleState{textColor = Color.white}};
            _cyanLabelStyle  = new GUIStyle{normal = new GUIStyleState{textColor = Color.cyan}};
        }

        #endregion

        #region Buttons

        private static Texture _removeBtnTexture;
        private static Texture _removeBtnOnRollOverTexture;
        
        private static Texture _moveUpBtnTexture;
        private static Texture _moveUpBtnOnRollOverTexture;
        
        private static Texture _moveDownBtnTexture;
        private static Texture _moveDownBtnOnRollOverTexture;
        
        /// <summary>
        /// Draws a Remove Button
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool DrawRemoveButton(Rect rect, Event e){
            LoadTextures();

            rect = rect.AddX(10).SetWidth(9).SetHeight(13);
            
            var buttonPressed = false;
            
            var texture = _removeBtnTexture;

            if (rect.AddX(-2).AddY(-2).AddWith(4).AddHeight(4).Contains(e.mousePosition)){
                texture = _removeBtnOnRollOverTexture;
                
                if (e.type == EventType.MouseDown && e.button == 0)
                    buttonPressed = true;
            }
            
            GUI.DrawTexture(rect, texture);
            
            GUI.changed = true;
            
            return buttonPressed;
        }

        public static bool DrawCustomButton(Rect rect, Event e, Texture normal, Texture rollOver){
            rect = rect.AddX(10).SetWidth(normal.width).SetHeight(normal.height);
            
            var buttonPressed = false;
            
            var texture = normal;

            if (rect.AddX(-2).AddY(-2).AddWith(4).AddHeight(4).Contains(e.mousePosition)){
                texture = rollOver;
                
                if (e.type == EventType.MouseDown && e.button == 0)
                    buttonPressed = true;
            }
            
            GUI.DrawTexture(rect, texture);
            
            GUI.changed = true;
            
            return buttonPressed;
        }
        
        public static bool DrawMoveUpButton(Rect rect, Event e){
            LoadTextures();

            rect = rect.AddX(10).SetWidth(10).SetHeight(13);
            
            var buttonPressed = false;
            
            var texture = _moveUpBtnTexture;

            if (rect.AddX(-2).AddY(-2).AddWith(4).AddHeight(4).Contains(e.mousePosition)){
                texture = _moveUpBtnOnRollOverTexture;
                
                if (e.type == EventType.MouseDown && e.button == 0)
                    buttonPressed = true;
            }
            
            GUI.DrawTexture(rect, texture);
            
            GUI.changed = true;
            
            return buttonPressed;
        }
        
        public static bool DrawMoveDownButton(Rect rect, Event e){
            LoadTextures();

            rect = rect.AddX(10).SetWidth(10).SetHeight(13);
            
            var buttonPressed = false;
            
            var texture = _moveDownBtnTexture;

            if (rect.AddX(-2).AddY(-2).AddWith(4).AddHeight(4).Contains(e.mousePosition)){
                texture = _moveDownBtnOnRollOverTexture;
                
                if (e.type == EventType.MouseDown && e.button == 0)
                    buttonPressed = true;
            }
            
            GUI.DrawTexture(rect, texture);
            
            GUI.changed = true;
            
            return buttonPressed;
        }

        #endregion

        #region Textures

        private static void LoadTextures(){
            if (_removeBtnTexture && _removeBtnOnRollOverTexture) return;
            _removeBtnTexture             = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Plugins/KennethDevelops/Resources/GUI/RemoveBtn.png");
            _removeBtnOnRollOverTexture   = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Plugins/KennethDevelops/Resources/GUI/RemoveBtnRollOver.png");
            
            _moveUpBtnTexture             = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Plugins/KennethDevelops/Resources/GUI/ReorderUpBtn.png");
            _moveUpBtnOnRollOverTexture   = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Plugins/KennethDevelops/Resources/GUI/ReorderUpBtnRollOver.png");
            
            _moveDownBtnTexture           = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Plugins/KennethDevelops/Resources/GUI/ReorderDownBtn.png");
            _moveDownBtnOnRollOverTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Plugins/KennethDevelops/Resources/GUI/ReorderDownBtnRollOver.png");
        }

        #endregion
        
        /// <summary>
        /// Save the changes of the passed targets
        /// </summary>
        /// <param name="target">Objects that will be saved</param>
        public static void SaveChanges(params Object[] target){
            foreach (var t in target) EditorUtility.SetDirty(t);
		
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static GUIStyle WhiteLblStyle{
            get{
                if (_whiteLabelStyle == null) LoadStyles();
                return _whiteLabelStyle;
            }
        }
        
        public static GUIStyle CyanLblStyle{
            get{
                if (_cyanLabelStyle == null) LoadStyles();
                return _cyanLabelStyle;
            }
        }

    }
}