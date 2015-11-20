using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DebugUI : MonoBehaviour {

	private Rect windowRect;
	// private GUIContent[] contents;

	// Use this for initialization
	void Start () {
		this.windowRect = new Rect(10, 10, 250, 100);
		// this.contents = new GUIContent[] { new GUIContent("↑"), new GUIContent("→"), new GUIContent("↓"), new GUIContent("←")};
	}
	
	void OnGUI()
	{
		this.windowRect = GUI.Window(0, windowRect, DebugWindow, "Debug Window");
	}

	void DebugWindow(int windowID)
	{
		GUI.DragWindow(new Rect(0, 0, 10000, 20));
		GUI.BeginGroup(Util.Add(windowRect, new Rect(0, 15, 0, 0)));
		{
			// int sg = GUI.SelectionGrid(new Rect(0, 0, 60, 60), 0, this.contents, 2);
		}
		GUI.EndGroup();
	}
}
