using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransactionEffectManager : MonoBehaviour
{
	public static TransactionEffectManager instance;

	[Header("Elements")]
	[SerializeField] private ParticleSystem coinPS;
	[SerializeField] private RectTransform coinRectTransform;

	[Header("Settings")]
	[SerializeField] private float moveSpeed;
	private int coinsAmount;
	private new Camera camera;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		camera = Camera.main;
	}

	[NaughtyAttributes.Button]
	private void PlayCoinParticlesTest()
	{
		PlayCoinParticles(100);
	}

	public void PlayCoinParticles(int amount)
	{
		// Đặt lại hệ thống hạt
		coinPS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

		// Thiết lập số lượng hạt bắn ra
		ParticleSystem.Burst burst = coinPS.emission.GetBurst(0);
		burst.count = amount;
		coinPS.emission.SetBurst(0, burst);

		// Đặt trọng lực để các đồng xu rơi xuống ban đầu
		ParticleSystem.MainModule main = coinPS.main;
		main.gravityModifier = 2;

		// Chạy hệ thống hạt
		coinPS.Play();

		coinsAmount = amount;

		StartCoroutine(PlayCoinParticlesCoroutine()); // Bắt đầu coroutine một cách đúng đắn
	}

	IEnumerator PlayCoinParticlesCoroutine() // Thay đổi từ IEnumerable thành IEnumerator
	{
		yield return new WaitForSeconds(1);

		Debug.Log("Bắt đầu PlayCoinParticlesCoroutine");

		ParticleSystem.MainModule main = coinPS.main;
		main.gravityModifier = 0; // Tắt trọng lực sau 1 giây

		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[coinsAmount];
		coinPS.GetParticles(particles);

		Vector3 direction = (coinRectTransform.position - camera.transform.position).normalized;
		Vector3 targetPosition = camera.transform.position + direction * Vector3.Distance(camera.transform.position, coinPS.transform.position);

		bool allParticlesReachedTarget = false;

		while (coinPS.isPlaying && !allParticlesReachedTarget)
		{
			allParticlesReachedTarget = true;

			for (int i = 0; i < particles.Length; i++)
			{
				if (particles[i].remainingLifetime <= 0)
					continue;

				particles[i].position = Vector3.MoveTowards(particles[i].position, targetPosition, moveSpeed * Time.deltaTime);

				if (Vector3.Distance(particles[i].position, targetPosition) < 0.1f) // Tăng ngưỡng kiểm tra để thử nghiệm
				{
					Debug.Log("Hạt đạt đến vị trí đích");
					//particles[i].position += Vector3.up * 100000;
					particles[i].remainingLifetime = 0; // Đặt tuổi thọ của cois về 0 để bắt đầu chu kỳ mới
					CashManager.instance.AddCoins(1); // Thêm một đồng xu
				}
				else
				{
					allParticlesReachedTarget = false;
				}
			}

			coinPS.SetParticles(particles);

			yield return null;
		}

		Debug.Log("Hệ thống hạt đã hoàn thành");
	}
}



