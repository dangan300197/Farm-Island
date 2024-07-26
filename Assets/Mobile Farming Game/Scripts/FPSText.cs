using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSText : MonoBehaviour
{
	public TextMeshProUGUI fpsText;
	private float deltaTime = 0.0f;

	void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
		float fps = 1.0f / deltaTime;
		fpsText.text = string.Format("FPS: {0:0.}", fps);
	}

}


