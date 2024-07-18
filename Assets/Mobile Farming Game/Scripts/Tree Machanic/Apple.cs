using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Apple : MonoBehaviour
{
	enum State { Ready, Growing}
	private State state;

	[Header("Elements")]
	[SerializeField] private new Renderer renderer;
	private Rigidbody rig;

	[Header("Settings")]
	[SerializeField] private float shakeMultiplier;
	private Vector3 initialPos;
	private Quaternion initialRot;

	private void Awake()
	{
		rig = GetComponent<Rigidbody>();

		initialPos = transform.position;
		initialRot = transform.rotation;
	}

	// Start is called before the first frame update
	void Start()
	{
		state = State.Ready;
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Shake(float magnitude)
	{
		float realShakeMagtinude = magnitude * shakeMultiplier;

		renderer.material.SetFloat("_Magnitude", realShakeMagtinude);
	}
	public void Release()
	{
		rig.isKinematic = false;

		state = State.Growing;

		renderer.material.SetFloat("_Magnitude", 0);
	}

	public bool IsFree()
	{
		return !rig.isKinematic;
	}

	public void Reset()
	{
		LeanTween.scale(gameObject, Vector3.zero, 1).setDelay(2).setOnComplete(ForceReset);
	}

	public bool IsReady()
	{
		return state == State.Ready;
	}

	private void ForceReset()
	{
		transform.position = initialPos;
		transform.rotation = initialRot;

		rig.isKinematic = true;

		

		// Scale up
		float randomScaleTime = Random.Range(5f, 10f);
		LeanTween.scale(gameObject, Vector3.one, randomScaleTime).setOnComplete(SetReady);
	}

	private void SetReady()
	{
		state = State.Ready;
	}
}
