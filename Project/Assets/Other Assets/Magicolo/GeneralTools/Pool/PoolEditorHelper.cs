using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using Magicolo.EditorTools;

namespace Magicolo.GeneralTools {
	[System.Serializable]
	public class PoolEditorHelper : EditorHelper {

		public Pool pool;
		
		public PoolEditorHelper(Pool pool) {
			this.pool = pool;
		}
		
		public void Initialize(Pool pool) {
			this.pool = pool;
		}
		
		public void RepaintInspector() {
			#if UNITY_EDITOR
			UnityEditor.SerializedObject poolSerialized = new UnityEditor.SerializedObject(pool);
			UnityEditor.SerializedProperty repaintDummyProperty = poolSerialized.FindProperty("editorHelper").FindPropertyRelative("repaintDummy");
			repaintDummyProperty.SetValue(!repaintDummy);
			#endif
		}
	}
}
