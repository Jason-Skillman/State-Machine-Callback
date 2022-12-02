namespace JasonSkillman.StateMachine.Editor {
	using System;
	using System.Collections;
	using System.Linq;
	using System.Reflection;
	using UnityEditor;
	using UnityEditorInternal;
	using UnityEngine;
	using UnityEngine.Events;
	
	[CustomEditor(typeof(StateMachineCallbackEvents))]
	public class StateMachineCallbackEventsEditor : Editor {

		private const float singleLineHeightWithMargin = 20.0f;
		
		/// <summary>
		/// The total amount of properties that exist.
		/// <para>Used to correctly calculate the height.</para>
		/// </summary>
		private const float propertyAmount = 2;
		/// <summary>
		/// The total amount of UnityEvents that exist.
		/// <para>Used to correctly calculate the height.</para>
		/// </summary>
		private const float unityEventAmount = 3;

		private ReorderableList reorderableList;
		
		/// <summary>
		/// The height of an empty UnityEvent.
		/// </summary>
		private float UnityEventHeight => EditorGUIUtility.singleLineHeight * 3 + 4 + 42;
		/// <summary>
		/// The height of a single card in a UnityEvent.
		/// </summary>
		private float CardHeight => EditorGUIUtility.singleLineHeight * 2 + 11;
		
		public void OnEnable() {
			SerializedProperty rulesListProperty = serializedObject.FindProperty("rulesList");

			reorderableList = new ReorderableList(serializedObject, rulesListProperty,
				true, true, true, true);
			reorderableList.drawHeaderCallback += OnDrawHeader;
			reorderableList.drawElementCallback += OnDrawElement;
			reorderableList.elementHeightCallback += GetElementHeight;
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			reorderableList.DoLayoutList();
			
			serializedObject.ApplyModifiedProperties();
		}

		private void OnDrawHeader(Rect rect) {
			EditorGUI.LabelField(rect, "State Machine Callback Events");
		}
		
		private void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused) {
			SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
			
			Rect currentRect = rect;
			currentRect.y += 2;
			currentRect.height = EditorGUIUtility.singleLineHeight;
			
			//Draw filter
			{
				EditorGUI.PropertyField(currentRect, element.FindPropertyRelative("animationStateName"));
			}
			
			//Draw layerIndex
			{
				currentRect.y += singleLineHeightWithMargin;
				EditorGUI.PropertyField(currentRect, element.FindPropertyRelative("layerIndex"));
			}
			
			//Draw onAnimationStart
			{
				currentRect.y += singleLineHeightWithMargin + 6;
				EditorGUI.PropertyField(currentRect, element.FindPropertyRelative("onAnimationStart"));
			}
			
			//Draw onAnimationUpdate
			{
				int previousEventCount = GetEventCount(element, "onAnimationStart");
				float extraEventHeight = CardHeight * (Math.Max(1, previousEventCount) - 1);
				
				currentRect.y += UnityEventHeight + extraEventHeight;
				EditorGUI.PropertyField(currentRect, element.FindPropertyRelative("onAnimationUpdate"));
			}
			
			//Draw onAnimationEnd
			{
				int previousEventCount = GetEventCount(element, "onAnimationUpdate");
				float extraEventHeight = CardHeight * (Math.Max(1, previousEventCount) - 1);
				
				currentRect.y += UnityEventHeight + extraEventHeight;
				EditorGUI.PropertyField(currentRect, element.FindPropertyRelative("onAnimationEnd"));
			}
		}

		private float GetElementHeight(int index) {
			SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
			
			//Add up all of the properties
			float propertyHeight = singleLineHeightWithMargin * propertyAmount + 6;

			//Add up all of the UnityEvents
			float unityEventBaseHeight = unityEventAmount * UnityEventHeight;
			
			//Add up all of the UnityEvent's extra event cards
			float extraEventHeight = CardHeight * (Math.Max(1, GetEventCount(element, "onAnimationStart")) - 1);
			extraEventHeight += CardHeight * (Math.Max(1, GetEventCount(element, "onAnimationUpdate")) - 1);
			extraEventHeight += CardHeight * (Math.Max(1, GetEventCount(element, "onAnimationEnd")) - 1);
			
			return propertyHeight + unityEventBaseHeight + extraEventHeight;
		}

		#region HelperMethods

		private int GetEventCount(SerializedProperty element, string propertyName) {
        	SerializedProperty parentProperty = element.FindPropertyRelative(propertyName);
        	object parentObj = GetParent(parentProperty);
        	
        	object objAnimStart = GetValue(parentObj, propertyName);
        	UnityEvent unityEventAnimStart = objAnimStart as UnityEvent;
        	return unityEventAnimStart.GetPersistentEventCount();
        }

        private object GetParent(SerializedProperty prop) {
        	var path = prop.propertyPath.Replace(".Array.data[", "[");
        	object obj = prop.serializedObject.targetObject;
        	var elements = path.Split('.');
        	foreach(var element in elements.Take(elements.Length - 1)) {
        		if(element.Contains("[")) {
        			var elementName = element.Substring(0, element.IndexOf("["));
        			var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
        			obj = GetValue(obj, elementName, index);
        		} else {
        			obj = GetValue(obj, element);
        		}
        	}

        	return obj;
        }

        private object GetValue(object source, string valueName) {
        	if(source == null)
        		return null;
        	var type = source.GetType();
        	var f = type.GetField(valueName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        	if(f == null) {
        		var p = type.GetProperty(valueName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        		if(p == null)
        			return null;
        		return p.GetValue(source, null);
        	}

        	return f.GetValue(source);
        }

        private object GetValue(object source, string valueName, int index) {
        	var enumerable = GetValue(source, valueName) as IEnumerable;
        	var enm = enumerable.GetEnumerator();
        	while(index-- >= 0)
        		enm.MoveNext();
        	return enm.Current;
        }

		#endregion
		
	}
}