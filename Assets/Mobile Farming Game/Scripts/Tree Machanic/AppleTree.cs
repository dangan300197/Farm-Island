using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour
{
	[Header("Elements")]
	[SerializeField] private GameObject treeCam;
	[SerializeField] private new Renderer renderer;
	[SerializeField] private Transform appleParent;
	private AppleTreeManager treeManager;

	[Header("Settings")]
	[SerializeField] private float maxShakeMagnitude;
	[SerializeField] private float shakeIncrement;
	private float shakeSliderValue;
	private float shakeMagnitude;
	private bool isShaking;

	[Header("Actions")]
	public static Action<CropType> onAppleHarvested;


	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Initialize(AppleTreeManager treeManager)
	{
		EnableCam();

		shakeSliderValue = 0;

		this.treeManager = treeManager;
	}
	public void EnableCam()
	{
		treeCam.SetActive(true);
	}
	public void DisableCam()
	{
		treeCam.SetActive(false);
	}

	public void Shake()
	{
		isShaking = true;

		TweenShake(maxShakeMagnitude);

		UpdateShakeSlider();
	}

	private void UpdateShakeSlider()
	{
		shakeSliderValue += shakeIncrement;
		treeManager.UpdateShakeSlider(shakeSliderValue);

		for (int i = 0; i < appleParent.childCount; i++)
		{
			float applePercent = (float)i / appleParent.childCount;

			Apple currentApple = appleParent.GetChild(i).GetComponent<Apple>();

			if(shakeSliderValue > applePercent && !currentApple.IsFree())
			{
				ReleaseApple(currentApple);
			}
		}

		if(shakeSliderValue >= 1)
		{
			ExitTreeMode();
		}
	}

	private void ReleaseApple(Apple apple)
	{
		apple.Release();

		onAppleHarvested?.Invoke(CropType.Apple);
	}

	

	public void StopShaking()
	{
		if (!isShaking)
			return;

		isShaking = false;
		Debug.Log("Stop Shaking");

		TweenShake(0);
	}

	public bool IsReady()
	{
		for (int i = 0; i < appleParent.childCount; i++)
		{
			if (!appleParent.GetChild(i).GetComponent<Apple>().IsReady())
			{
				return false;
			}
		}
		return true;
	}

	private void TweenShake(float targetMagnitude)
	{
		LeanTween.cancel(renderer.gameObject);
		LeanTween.value(renderer.gameObject, UpdateShakeMagnitude, shakeMagnitude, targetMagnitude, 1);
	}


	private void UpdateShakeMagnitude(float value)
	{
		shakeMagnitude = value;
		UpdateMaterials();
	}

	public void UpdateMaterials()
	{
		foreach (Material mat in renderer.materials)
		{
			mat.SetFloat("_Magnitude", shakeMagnitude);
		}

		foreach (Transform appleT in appleParent)
		{
			Apple apple = appleT.GetComponent<Apple>();

			if (apple.IsFree())
				continue;

			apple.Shake(shakeMagnitude);
		}
	}

	public void ExitTreeMode()
	{
		treeManager.EndTreeMode();

		DisableCam();

		TweenShake(0);

		ResetApples();
	}

	private void ResetApples()
	{
		for (int i = 0; i < appleParent.childCount; i++)
		{
			appleParent.GetChild(i).GetComponent<Apple>().Reset();
		}
	}
}
