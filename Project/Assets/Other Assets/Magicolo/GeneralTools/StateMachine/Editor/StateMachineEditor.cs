using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Magicolo.EditorTools;

namespace Magicolo.GeneralTools {
	[CustomEditor(typeof(StateMachine)), CanEditMultipleObjects]
	public class StateMachineEditor : CustomEditorBase {

		StateMachine stateMachine;
		GameObject stateMachineObject;
		StateLayer[] existingLayers;
		State[] existingStates;
		SerializedProperty layersProperty;
		StateLayer currentLayer;
		SerializedProperty currentLayerProperty;
		SerializedObject currentLayerSerialized;
		SerializedProperty statesProperty;
		SerializedProperty currentStatesProperty;
		State currentState;
		SerializedProperty currentStateProperty;
		SerializedObject currentStateSerialized;
		
		Type selectedLayerType;
		
		static List<Type> layerTypes;
		static List<Type> stateTypes;
		static Dictionary<Type, List<Type>> layerStateDict;
		static Dictionary<Type, List<Type>> LayerStateDict {
			get {
				if (layerStateDict == null) {
					BuildLayerStatesDict();
				}
				
				return layerStateDict;
			}
		}
				
		static Dictionary<string, List<string>> layerStateNameDict;
		static Dictionary<string, List<string>> LayerStateNameDict {
			get {
				if (layerStateDict == null) {
					BuildLayerStatesDict();
				}
				
				return layerStateNameDict;
			}
		}
		
		static Dictionary<Type, string> LayerTypeNameDict;

		public override void OnEnable() {
			base.OnEnable();
			
			stateMachine = (StateMachine)target;
			stateMachineObject = stateMachine.gameObject;
			HideLayersAndStates();
		}
		
		public override void OnDisable() {
			if (stateMachineObject != null) {
				CleanUp();
			}
		}
		
		public override void OnInspectorGUI() {
			Begin();
			
			HideLayersAndStates();
			ShowLayers();
			Separator();
			ShowGenerateButton();
			ReorderComponents();
			
			End();
		}

		void HideLayersAndStates() {
			existingLayers = stateMachineObject.GetComponents<StateLayer>();
			existingStates = stateMachineObject.GetComponents<State>();
			
			Array.ForEach(existingLayers, layer => layer.hideFlags = HideFlags.HideInInspector);
			Array.ForEach(existingStates, state => state.hideFlags = HideFlags.HideInInspector);
		}
		
		void ShowLayers() {
			layersProperty = serializedObject.FindProperty("layers");
			CleanUpLayers();
			
			ShowAddLayer();
			
			if (layersProperty.arraySize > 0) {
				Separator();
			}
			
			for (int i = 0; i < layersProperty.arraySize; i++) {
				currentLayerProperty = layersProperty.GetArrayElementAtIndex(i);
				currentLayer = currentLayerProperty.GetValue<StateLayer>();
				currentLayerSerialized = new SerializedObject(currentLayerProperty.objectReferenceValue);
				statesProperty = currentLayerSerialized.FindProperty("states");
				currentStatesProperty = currentLayerSerialized.FindProperty("currentStates");
				
				BeginBox();
				
				GUIStyle style = new GUIStyle("foldout");
				style.fontStyle = FontStyle.Bold;
				if (DeleteFoldOut(layersProperty, i, currentLayer.GetType().Name.ToGUIContent(), style, OnLayerDeleted)) {
					break;
				}
				
				ShowLayer();
				
				EndBox();
			}
		}

		void OnLayerDeleted(SerializedProperty arrayProperty, int indexToRemove) {
			StateLayer layer = arrayProperty.GetArrayElementAtIndex(indexToRemove).GetValue<StateLayer>();
			
			for (int i = 0; i < statesProperty.arraySize; i++) {
				State state = statesProperty.GetArrayElementAtIndex(i).GetValue<State>();
				
				if (state != null) {
					state.Remove();
				}
			}
			
			layer.Remove();
			DeleteFromArray(arrayProperty, indexToRemove);
		}
		
		void ShowAddLayer() {
			List<Type> layerTypes = new List<Type>();
			List<string> layerTypesName = new List<string>{ " " };
			
			foreach (Type layerType in LayerStateDict.Keys) {
				if (layersProperty.TrueForAll<StateLayer>(layer => layer.GetType() != layerType)) {
					layerTypes.Add(layerType);
					layerTypesName.Add(LayerTypeNameDict[layerType]);
				}
			}
			
			EditorGUI.BeginDisabledGroup(Application.isPlaying);
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.LabelField("Add Layer", new GUIStyle("boldLabel"), GUILayout.Width(75));
			int layerTypeIndex = EditorGUILayout.Popup(layerTypes.IndexOf(selectedLayerType) + 1, layerTypesName.ToArray()) - 1;
			selectedLayerType = layerTypeIndex == -1 ? null : layerTypes[Mathf.Clamp(layerTypeIndex, 0, layerTypes.Count - 1)];
			
			if (selectedLayerType != null) {
				OnLayerAdded(layersProperty);
			}
			
			EditorGUILayout.EndHorizontal();
			EditorGUI.EndDisabledGroup();
		}
		
		void OnLayerAdded(SerializedProperty arrayProperty) {
			AddToArray(arrayProperty);
			
			StateLayer newLayer = stateMachineObject.AddComponent(selectedLayerType) as StateLayer;
			newLayer.machine = stateMachine;
			newLayer.hideFlags = HideFlags.HideInInspector;
			arrayProperty.GetLastArrayElement().SetValue(newLayer);
		}
		
		void ShowLayer() {
			CleanUpStates();
			SetLayerStates();
			
			if (currentLayerProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				List<string> currentLayerStatesName = new List<string>{ "Empty" };
				
				for (int i = 0; i < statesProperty.arraySize; i++) {
					currentLayerStatesName.Add(FormatStateType(statesProperty.GetArrayElementAtIndex(i).GetValue().GetType(), currentLayer));
				}
				
				for (int i = 0; i < currentStatesProperty.arraySize; i++) {
					SerializedProperty stateProperty = currentStatesProperty.GetArrayElementAtIndex(i);
					
					Rect dragArea = EditorGUILayout.BeginHorizontal();
				
					EditorGUI.BeginChangeCheck();
				
					int stateIndex = EditorGUILayout.Popup(string.Format("Active State ({0})", i), stateProperty.objectReferenceValue == null ? 0 : statesProperty.IndexOf(stateProperty.objectReferenceValue) + 1, currentLayerStatesName.ToArray(), GUILayout.MinWidth(200)) - 1;
					stateProperty.SetValue(stateIndex == -1 ? null : statesProperty.GetArrayElementAtIndex(Mathf.Clamp(stateIndex, 0, statesProperty.arraySize - 1)).GetValue());
					
					if (EditorGUI.EndChangeCheck() && Application.isPlaying) {
						currentLayer.SwitchState(stateProperty.objectReferenceValue == null ? typeof(EmptyState) : stateProperty.objectReferenceValue.GetType(), i);
					}
				
					if (i == 0) {
						SmallAddButton(currentStatesProperty);
					}
					else if (DeleteButton(currentStatesProperty, i)) {
						break;
					}
					
					EditorGUILayout.EndHorizontal();
					
					Reorderable(currentStatesProperty, i, true, EditorGUI.IndentedRect(dragArea));
				}
				
				Separator();
				ShowLayerFields();
				ShowStates();
				if (statesProperty.arraySize > 0) Separator();
				
				EditorGUI.indentLevel -= 1;
			}
			
			currentLayerSerialized.ApplyModifiedProperties();
		}

		void ShowLayerFields() {
			SerializedProperty iterator = currentLayerSerialized.GetIterator();
			iterator.NextVisible(true);
			iterator.NextVisible(false);
			iterator.NextVisible(false);
			iterator.NextVisible(false);
			if (!iterator.NextVisible(false)) return;
			
			while (true) {
				EditorGUILayout.PropertyField(iterator, true);
				
				if (!iterator.NextVisible(false)) {
					break;
				}
			}
			
			Separator();
		}
		
		void ShowStates() {
			for (int i = 0; i < statesProperty.arraySize; i++) {
				currentStateProperty = statesProperty.GetArrayElementAtIndex(i);
				currentState = currentStateProperty.GetValue<State>();
				
				if (currentState == null) {
					DeleteFromArray(statesProperty, i);
					break;
				}
				
				BeginBox();
				
				Foldout(currentStateProperty, FormatStateType(currentState.GetType(), currentLayer).ToGUIContent(), GetStateStyle());
				Reorderable(statesProperty, i, true);
				
				ShowState();
				
				EndBox();
			}
		}
		
		void ShowState() {
			currentStateSerialized = new SerializedObject(currentStateProperty.objectReferenceValue);
			
			if (currentStateProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				
				ShowStateFields();
				
//				Separator();
				
				EditorGUI.indentLevel -= 1;
			}
			
			currentStateSerialized.ApplyModifiedProperties();
		}
		
		void ShowStateFields() {
			SerializedProperty iterator = currentStateSerialized.GetIterator();
			iterator.NextVisible(true);
			iterator.NextVisible(false);
			iterator.NextVisible(false);
				
			while (iterator.NextVisible(false)) {
				EditorGUILayout.PropertyField(iterator, true);
			}
		}

		void SetLayerStates() {
			foreach (Type stateType in LayerStateDict[currentLayer.GetType()]) {
				if (statesProperty.arraySize == 0 || statesProperty.TrueForAll<State>(state => state.GetType() != stateType)) {
					AddToArray(statesProperty);
					State newState = currentLayer.gameObject.AddComponent(stateType) as State;
					newState.layer = currentLayer;
					newState.machine = stateMachine;
					newState.hideFlags = HideFlags.HideInInspector;
					statesProperty.GetLastArrayElement().SetValue(newState);
				}
			}
		}

		void ShowGenerateButton() {
			if (layersProperty.arraySize == 0) {
				if (LargeButton("Generate".ToGUIContent(), true)) {
					StateMachineGeneratorWindow.Create();
				}
			}
		}
		
		void CleanUp() {
			StateLayer[] layers = stateMachineObject.GetComponents<StateLayer>();
			State[] states = stateMachineObject.GetComponents<State>();
				
			foreach (StateLayer layer in layers) {
				if (layer.machine != stateMachine || layer.machine == null) {
					layer.Remove();
				}
			}
			
			foreach (State state in states) {
				if (state.machine != stateMachine || state.machine == null) {
					state.Remove();
				}
			}
		}

		void CleanUpLayers() {
			for (int i = layersProperty.arraySize - 1; i >= 0; i--) {
				if (layersProperty.GetArrayElementAtIndex(i).GetValue<StateLayer>() == null) {
					DeleteFromArray(layersProperty, i);
				}
			}
		}
		
		void CleanUpStates() {
			for (int i = statesProperty.arraySize - 1; i >= 0; i--) {
				if (statesProperty.GetArrayElementAtIndex(i).GetValue<State>() == null) {
					DeleteFromArray(statesProperty, i);
				}
			}
		}

		void ReorderComponents() {
			int firstStateOrLayerIndex = 0;
			
			Component[] components = stateMachine.GetComponents<Component>();
			for (int i = 0; i < components.Length; i++) {
				Component component = components[i];
				
				if (component as State != null || component as StateLayer != null) {
					firstStateOrLayerIndex = firstStateOrLayerIndex == 0 ? i : firstStateOrLayerIndex;
				}
				else if (firstStateOrLayerIndex > 0) {
					for (int j = 0; j < i - firstStateOrLayerIndex; j++) {
						UnityEditorInternal.ComponentUtility.MoveComponentUp(component);
					}
				}
			}
		}

		GUIStyle GetStateStyle() {
			GUIStyle style = new GUIStyle("foldout");
			style.fontStyle = FontStyle.Bold;
			Color textColor = style.normal.textColor * 1.5F;
		
			if (Application.isPlaying) {
				textColor = currentStatesProperty.Contains(currentState) ? new Color(0, 1, 0, 10) : new Color(1, 0, 0, 10);
			}

			style.normal.textColor = textColor * 0.7F;
			style.onNormal.textColor = textColor * 0.7F;
			style.focused.textColor = textColor * 0.85F;
			style.onFocused.textColor = textColor * 0.85F;
			style.active.textColor = textColor * 0.85F;
			style.onActive.textColor = textColor * 0.85F;
		
			return style;
		}
		
		static string FormatLayerType(Type layerType) {
			string layerName = layerType.Name;
			string formattedName = layerName.Split('.').Last();
			
			for (int i = formattedName.Replace("Layer", "").Length - 1; i >= 0; i--) {
				char letter = layerName[i];
				
				if (i > 0 && char.IsUpper(letter)) {
					formattedName = formattedName.Insert(i, "/");
				}
			}
			
			return formattedName;
		}
		
		static string FormatStateType(Type stateType, string layerTypePrefix) {
			string stateName = stateType.Name;
			string formattedName = stateName.Split('.').Last();
			
			formattedName = formattedName.Substring(layerTypePrefix.Length);
			
			return formattedName.Replace("State", "");
		}
		
		static string FormatStateType(Type stateType, StateLayer layer) {
			return FormatStateType(stateType, GetLayerPrefix(layer));
		}
		
		static string FormatStateType(Type stateType, Type layerType) {
			return FormatStateType(stateType, GetLayerPrefix(layerType));
		}
	
		static string GetLayerPrefix(StateLayer layer) {
			return GetLayerPrefix(layer.GetType());
		}
		
		static string GetLayerPrefix(Type layerType) {
			return layerType.Name.Split('.').Last().Replace("Layer", "");
		}
		
		static void BuildLayerStatesDict() {
			layerTypes = new List<Type>();
			stateTypes = new List<Type>();
			layerStateDict = new Dictionary<Type, List<Type>>();
			layerStateNameDict = new Dictionary<string, List<string>>();
			LayerTypeNameDict = new Dictionary<Type, string>();
			
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies) {
				Type[] types = assembly.GetTypes();
				
				foreach (Type type in types) {
					if (type.IsSubclassOf(typeof(StateLayer))) {
						layerTypes.Add(type);
					}
					else if (type.IsSubclassOf(typeof(State))) {
						stateTypes.Add(type);
					}
				}
			}
			
			foreach (Type layerType in layerTypes) {
				string layerTypePrefix = GetLayerPrefix(layerType);
				string layerTypeName = FormatLayerType(layerType);
				layerStateDict[layerType] = new List<Type>();
				layerStateNameDict[layerTypeName] = new List<string>();
				LayerTypeNameDict[layerType] = layerTypeName;
				
				foreach (Type stateType in stateTypes) {
					if (stateType.Name.Split('.').Last().StartsWith(layerTypePrefix)) {
						layerStateDict[layerType].Add(stateType);
						layerStateNameDict[layerTypeName].Add(FormatStateType(stateType, layerTypePrefix));
					}
				}
			}
		}
		
		[UnityEditor.Callbacks.DidReloadScripts]
		static void OnReloadScripts() {
			layerStateDict = null;
			layerStateNameDict = null;
		}
	}
}
