using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Magicolo;

namespace Magicolo.EditorTools {
	public class StateMachineGeneratorWindow : CustomWindowBase {

		public string path = "Assets";
		public string layer = "";
		List<string> states = new List<string> { "" };
		
		[MenuItem("Magicolo's Tools/State Machine Generator")]
		public static void Create() {
			CreateWindow<StateMachineGeneratorWindow>("State Machine Generator", new Vector2(275, 250));
		}
		
		void OnGUI() {
			ShowPath();
			ShowLayer();
			ShowStates();
			minSize = new Vector2(minSize.x, states.Count * 16 + 164);
			maxSize = new Vector2(maxSize.x, minSize.y);
		}
	
		void OnDestroy() {
			Save();
		}
		
		void ShowPath() {
			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			
			GUIStyle style = new GUIStyle("boldLabel");
			EditorGUILayout.LabelField("Path: ".ToGUIContent(), style, GUILayout.Width("Path: ".GetWidth(style.font) + 13));
			path = CustomEditorBase.FolderPathButton(path, Application.dataPath.Substring(0, Application.dataPath.Length - 6));
			
			GUILayout.Space(5);
			
			EditorGUILayout.EndHorizontal();
			
			CustomEditorBase.Separator();
		}
		
		void ShowLayer() {
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.LabelField("Layer", GUILayout.Width(50));
			layer = EditorGUILayout.TextField(layer);

			EditorGUILayout.EndHorizontal();
			
			ShowGenerateLayerButton();
			
			CustomEditorBase.Separator();
		}
		
		void ShowStates() {
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.LabelField("States");
			if (CustomEditorBase.AddButton()) {
				states.Add("");
			}
			GUILayout.Space(6);
			
			EditorGUILayout.EndHorizontal();
			
			EditorGUI.indentLevel += 1;
			
			for (int i = 0; i < states.Count; i++) {
				EditorGUILayout.BeginHorizontal();
				
				states[i] = EditorGUILayout.TextField(states[i]);
				if (CustomEditorBase.DeleteButton()) {
					states.RemoveAt(i);
					break;
				}
				GUILayout.Space(6);
				
				EditorGUILayout.EndHorizontal();
			}
			
			EditorGUI.indentLevel -= 1;
			
			ShowGenerateStatesButton();
			
			CustomEditorBase.Separator();
		}

		void ShowGenerateLayerButton() {
			EditorGUILayout.Space();
			
			if (GUILayout.Button("Generate Layer".ToGUIContent())) {
				GenerateLayer();
			}
		}
		
		void ShowGenerateStatesButton() {
			EditorGUILayout.Space();
			
			if (GUILayout.Button("Generate States".ToGUIContent())) {
				GenerateStates();
			}
		}
		
		void GenerateLayer() {
			#if !UNITY_WEBPLAYER
			if (string.IsNullOrEmpty(path)) {
				Logger.LogError("Path can not be empty.");
				return;
			}
			
			if (string.IsNullOrEmpty(layer)) {
				Logger.LogError("Layer name can not be empty.");
				return;
			}
			
			string layerFileName = layer.Capitalized() + ".cs";
			List<string> layerScript = new List<string>();
			
			if (!string.IsNullOrEmpty(HelperFunctions.GetAssetPath(layerFileName))) {
				Logger.LogError(string.Format("A script named {0} already exists.", layerFileName));
				return;
			}

			layerScript.Add("using UnityEngine;");
			layerScript.Add("using System.Collections;");
			layerScript.Add("using System.Collections.Generic;");
			layerScript.Add("using Magicolo;");
			layerScript.Add("");
			layerScript.Add("public class " + layer + " : StateLayer {");
			layerScript.Add("	");
			layerScript.Add("	");
			layerScript.Add("}");
			
			File.WriteAllLines(Application.dataPath.Substring(0, Application.dataPath.Length - 6) + Path.AltDirectorySeparatorChar + path + Path.AltDirectorySeparatorChar + layerFileName, layerScript.ToArray());
			AssetDatabase.Refresh();
			Save();
			#endif
		}
		
		void GenerateStates() {
			#if !UNITY_WEBPLAYER
			if (string.IsNullOrEmpty(path)) {
				Logger.LogError("Path can not be empty.");
				return;
			}
			
			if (string.IsNullOrEmpty(layer)) {
				Logger.LogError("Layer name can not be empty.");
				return;
			}
			
			foreach (string state in states) {
				string stateFileName = layer.Capitalized() + state.Capitalized() + ".cs";
				List<string> stateScript = new List<string>();
				
				if (string.IsNullOrEmpty(state)) {
					continue;
				}
				
				if (!string.IsNullOrEmpty(HelperFunctions.GetAssetPath(stateFileName))) {
					Logger.LogError(string.Format("A script named {0} already exists.", stateFileName));
					continue;
				}
				
				stateScript.Add("using UnityEngine;");
				stateScript.Add("using System.Collections;");
				stateScript.Add("using System.Collections.Generic;");
				stateScript.Add("using Magicolo;");
				stateScript.Add("");
				stateScript.Add("public class " + layer + state + " : State {");
				stateScript.Add("	");
				stateScript.Add("    " + layer + " Layer {");
				stateScript.Add("    	get { return ((" + layer + ")layer); }");
				stateScript.Add("    }");
				stateScript.Add("	");
				stateScript.Add("	public override void OnEnter() {");
				stateScript.Add("		");
				stateScript.Add("	}");
				stateScript.Add("	");
				stateScript.Add("	public override void OnExit() {");
				stateScript.Add("		");
				stateScript.Add("	}");
				stateScript.Add("	");
				stateScript.Add("	public override void OnUpdate() {");
				stateScript.Add("		");
				stateScript.Add("	}");
				stateScript.Add("}");
				
				File.WriteAllLines(Application.dataPath.Substring(0, Application.dataPath.Length - 6) + path + Path.AltDirectorySeparatorChar + stateFileName, stateScript.ToArray());
			}
			
			AssetDatabase.Refresh();
			Save();
			#endif
		}
	}
}

