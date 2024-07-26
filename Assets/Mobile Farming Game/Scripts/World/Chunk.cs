using System;
using TMPro;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(ChunkWalls))]
public class Chunk : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject unlockedElements;
    [SerializeField] private GameObject lockedElements;
    [SerializeField] private TextMeshPro priceText;
    [SerializeField] private MeshFilter chunkFilter;
    private ChunkWalls chunkWalls;


    [Header("Settings")]
    [SerializeField] private int initialPrice;
    private int currentPrice;
    private bool unlocked;
    private int configuration;

    [Header("Actions")]
    public static Action onUnlock;
    public static Action onPriceChanged;

	private void Awake()
	{
        chunkWalls = GetComponent<ChunkWalls>();
	}

	// Start is called before the first frame update
	void Start()
    {
        //currentPrice = initialPrice;
        //priceText.text = initialPrice.ToString();
    }

	public void Initialize(int loadedPrice)
	{
		currentPrice = loadedPrice;
		priceText.text = currentPrice.ToString();

		if (currentPrice <= 0)
		{
			Unlock(false);
		}

	}
	public void TryUnlock()
    {
        //Debug.Log("Trying to unlock the chunk : " + name);
        
        if(CashManager.instance.GetCoins() <= 0)
        {
            return;
        }

        currentPrice--;
        CashManager.instance.UseCoin(1);

        onPriceChanged?.Invoke();

        priceText.text = currentPrice.ToString();
        if(currentPrice <= 0)
        {
            Unlock();
        }

    }

	private void Unlock(bool triggerAction = true)
	{
        unlockedElements.SetActive(true);
        Scale();
		lockedElements.SetActive(false);

        unlocked = true;

        if (triggerAction)
        {
            onUnlock?.Invoke();
        }
	}

    private void Scale()
    {

        if(unlockedElements != null)
        {
            Vector3 orginScale = unlockedElements.transform.localScale;
            unlockedElements.transform.localScale = Vector3.zero;
            unlockedElements.transform.DOScale(orginScale, 0.5f).SetEase(Ease.OutBounce);

		}

	}

    public void UpdateWalls(int configuration)
    {
        this.configuration = configuration;
        chunkWalls.Configure(configuration);
    }

    public void DisplayLockedElements()
    {
        lockedElements.SetActive(true);
    }

    public void SetRenderer(Mesh chunkMesh)
    {
        chunkFilter.mesh = chunkMesh;
    }

	public bool IsUnlocked()
    {
        return unlocked;
    }

    public int GetInitialPrice()
    {
        return initialPrice;
    }

    public int GetCurrentPrice()
    {
        return currentPrice;
    }

	private void OnDrawGizmos()
	{
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 5);

        Gizmos.color = new Color(0, 0, 0, 0);
        Gizmos.DrawWireCube(transform.position, Vector3.one * 5);
	}
}
