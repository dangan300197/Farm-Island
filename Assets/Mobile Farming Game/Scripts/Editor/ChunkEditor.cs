using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Chunk))]

public class ChunkEditor : Editor
{
	private void OnSceneGUI()
	{
		GUIStyle style= new GUIStyle();
		style.alignment = TextAnchor.MiddleCenter;


		Chunk chunk = (Chunk)target;
		Handles.Label(chunk.transform.position, chunk.name);
		Debug.Log(chunk.name);
	}
}

#endif