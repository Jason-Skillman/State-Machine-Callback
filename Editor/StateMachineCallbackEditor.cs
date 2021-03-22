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
			
			//Todo: Add 2 other unityevents

			
			
			/*SerializedProperty property = element.FindPropertyRelative("onAnimationStart");
			EditorGUI.PropertyField(currentRect, property );*/
			
			/*EditorGUI.PropertyField(new Rect(rect.x, rect.y + 40, width, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("onAnimationStart"));*/
			
			
			
			
			/*object o = GetParent(property);
			object o2 = GetValue(o, "onAnimationStart");
			UnityEvent unityEvent = o2 as UnityEvent;

			EditorGUILayout.LabelField("Log: " + unityEvent.GetPersistentEventCount());*/
		}

		private float GetElementHeight(int index) {
			//SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
			
			//The height of one card
			float cardHeight = EditorGUIUtility.singleLineHeight * 3 + 4;
			//Default value when no cards exist
			/*float extraInnerHeight = 20;
			//Find the amount of cards and get the final height value
			if(dictionaryInnerList.ContainsKey(element.propertyPath))
				if(dictionaryInnerList[element.propertyPath].count > 0)
					extraInnerHeight = dictionaryInnerList[element.propertyPath].count * cardHeight;*/
			
			return EditorGUIUtility.singleLineHeight * 10;
		}
		
		
		
		
		
		
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
