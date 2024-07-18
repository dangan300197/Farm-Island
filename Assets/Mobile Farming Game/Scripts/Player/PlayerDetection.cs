using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;

public class PlayerDetection : MonoBehaviour
{
	[Header("Actions")]
	public static Action<AppleTree> onEnteredTreeZone;
	public static Action<AppleTree> onExitedTreeZone;

	private void OnTriggerStay(Collider other)
	{
		if(other.CompareTag("ChunkTrigger"))
		{
			Chunk chunk = other.GetComponentInParent<Chunk>();
			chunk.TryUnlock();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.TryGetComponent(out AppleTree tree))
		{
			TriggeredAppleTree(tree);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.TryGetComponent(out AppleTree tree))
		{
			ExitedAppleTreeZone(tree);
		}
	}

	private void TriggeredAppleTree(AppleTree tree)
	{
		Debug.Log("we've entered a tree zone");
		onEnteredTreeZone?.Invoke(tree);
	}

	private void ExitedAppleTreeZone(AppleTree tree)
	{
		Debug.Log("we've exit a tree zone");
		onExitedTreeZone?.Invoke(tree);
	}
}
