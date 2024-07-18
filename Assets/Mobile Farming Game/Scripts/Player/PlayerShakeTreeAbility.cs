using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]

public class PlayerShakeTreeAbility : MonoBehaviour
{
	[Header("Components")]
	private PlayerAnimator playerAnimator;

	[Header("Settings")]
	[SerializeField] private float distanceToTree;
	[Range(0f, 1f)]
	[SerializeField] private float shakeThreshold;
	private Vector2 previousMousePosition;
	private bool isActive;
	private bool isShaking;


	[Header("Elements")]
	private AppleTree currentTree;

	private void Awake()
	{
		playerAnimator = GetComponent<PlayerAnimator>();

		AppleTreeManager.onTreeModeStarted += TreeModeStartedCallback;
		AppleTreeManager.onTreeModeEnded += TreeModeEndedCallback;
	}
	private void OnDestroy()
	{
		AppleTreeManager.onTreeModeStarted -= TreeModeStartedCallback;
		AppleTreeManager.onTreeModeEnded -= TreeModeEndedCallback;
	}



	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (isActive && !isShaking)
			ManageTreeShaking();
	}
	private void TreeModeStartedCallback(AppleTree tree)
	{
		currentTree = tree;

		isActive = true;

		MoveTowardsTree();
	}

	private void TreeModeEndedCallback()
	{
		currentTree = null;

		isActive = false;
		isShaking = false;

		LeanTween.delayedCall(0.1f, () => playerAnimator.StopShakeTreeAnimation());
	}

	private void MoveTowardsTree()
	{
		Vector3 treePos = currentTree.transform.position;
		Vector3 dir = transform.position - treePos;

		Vector3 flatDir = dir;
		flatDir.y = 0;

		Vector3 targetPos = treePos + flatDir.normalized * distanceToTree;

		playerAnimator.ManageAnimations(-flatDir);

		LeanTween.move(gameObject, targetPos, 0.5f);
	}

	private void ManageTreeShaking()
	{
		if (!Input.GetMouseButton(0))
		{
			currentTree.StopShaking();
			return;
		}

		float shakeMagnitude = Vector2.Distance(Input.mousePosition, previousMousePosition);

		if (ShouldShake(shakeMagnitude))
		{
			Shake();
		}
		else
		{
			currentTree.StopShaking();
		}

		previousMousePosition = Input.mousePosition;
	}

	private void Shake()
	{
		isShaking = true;

		currentTree.Shake();

		playerAnimator.PlayShakeTreeAnimation();

		LeanTween.delayedCall(0.01f, () => isShaking = false);
	}

	private bool ShouldShake(float shakeMagtinude)
	{
		float screenPercent = shakeMagtinude / Screen.width;
		return screenPercent >= shakeThreshold;
	}

}
