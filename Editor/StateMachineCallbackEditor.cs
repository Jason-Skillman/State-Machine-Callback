using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using StateMachine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachineEditor {
	[CustomEditor(typeof(StateMachineCallback))]
	public class StateMachineCallbackEditor : Editor {

		private const float singleLineHeightWithMargin = 20.0f;

		private SerializedProperty dialogueGroupProperty;
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
			dialogueGroupProperty = serializedObject.FindProperty("group");

			reorderableList = new ReorderableList(serializedObject,
				dialogueGroupProperty.FindPropertyRelative("cards"),
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
			EditorGUI.LabelField(rect, "State Machine Callback");
		}
		
		private void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused) {
			SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
			
			Rect currentRect = rect;
			currentRect.y += 2;
			currentRect.height = EditorGUIUtility.singleLineHeight;
			
			//Draw filter
			{
				EditorGUI.PropertyField(currentRect, element.FindPropertyRelative("filter"));
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
				SerializedProperty parentProperty = element.FindPropertyRelative("onAnimationStart");
				object parentObj = GetParent(parentProperty);
			
				object objAnimStart = GetValue(parentObj, "onAnimationStart");
				UnityEvent unityEventAnimStart = objAnimStart as UnityEvent;
				int eventCountAnimStart = unityEventAnimStart.GetPersistentEventCount();
				
				float extraEventHeight = CardHeight * (Math.Max(1, eventCountAnimStart) - 1);
				
				currentRect.y += UnityEventHeight + extraEventHeight;
				EditorGUI.PropertyField(currentRect, element.FindPropertyRelative("onAnimationUpdate"));
			}
			
			//Todo: Add other unityevents
		}

		private float GetElementHeight(int index) {
			SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
			
			const float propertyAmount = 2;
			float propertyHeight = singleLineHeightWithMargin * propertyAmount + 6;

			const float unityEventAmount = 2;
			float unityEventBaseHeight = unityEventAmount * UnityEventHeight;
			
			
			//Get the element's parent property
			SerializedProperty parentProperty = element.FindPropertyRelative("onAnimationStart");
			object parentObj = GetParent(parentProperty);
			
			object objAnimStart = GetValue(parentObj, "onAnimationStart");
			UnityEvent unityEventAnimStart = objAnimStart as UnityEvent;
			int eventCountAnimStart = unityEventAnimStart.GetPersistentEventCount();
			
			object objAnimUpdate = GetValue(parentObj, "onAnimationUpdate");
			UnityEvent unityEventAnimUpdate = objAnimUpdate as UnityEvent;
			int eventCountAnimUpdate = unityEventAnimUpdate.GetPersistentEventCount();
			
			object objAnimEnd = GetValue(parentObj, "onAnimationUpdate");
			UnityEvent unityEventAnimEnd = objAnimEnd as UnityEvent;
			int eventCountAnimEnd = unityEventAnimEnd.GetPersistentEventCount();
			
			
			float extraEventHeight;
			extraEventHeight = CardHeight * (Math.Max(1, eventCountAnimStart) - 1);
			extraEventHeight += CardHeight * (Math.Max(1, eventCountAnimUpdate) - 1);
			
			
			//propertyHeight + unityEvent base height + unityEvent extra height
			return propertyHeight + unityEventBaseHeight + extraEventHeight;
		}
		
		
		
		
		
		//Todo: Move
		public object GetParent(SerializedProperty prop)
		{
			var path = prop.propertyPath.Replace(".Array.data[", "[");
			object obj = prop.serializedObject.targetObject;
			var elements = path.Split('.');
			foreach(var element in elements.Take(elements.Length-1))
			{
				if(element.Contains("["))
				{
					var elementName = element.Substring(0, element.IndexOf("["));
					var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[","").Replace("]",""));
					obj = GetValue(obj, elementName, index);
				}
				else
				{
					obj = GetValue(obj, element);
				}
			}
			return obj;
		}
 
		public object GetValue(object source, string name)
		{
			if(source == null)
				return null;
			var type = source.GetType();
			var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			if(f == null)
			{
				var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
				if(p == null)
					return null;
				return p.GetValue(source, null);
			}
			return f.GetValue(source);
		}
 
		public object GetValue(object source, string name, int index)
		{
			var enumerable = GetValue(source, name) as IEnumerable;
			var enm = enumerable.GetEnumerator();
			while(index-- >= 0)
				enm.MoveNext();
			return enm.Current;
		}

	}
}

//float width = EditorGUIUtility.currentViewWidth - EditorGUIUtility.fieldWidth - 12;
