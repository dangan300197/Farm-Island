using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UIManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject treeModePanel;

    [SerializeField] private GameObject treeButton;
    [SerializeField] private GameObject toolButtonContainer;

	private void Awake()
	{
        PlayerDetection.onEnteredTreeZone += EnteredTreeZoneCallback;
        PlayerDetection.onExitedTreeZone += ExitedTreeZoneCallback;

        AppleTreeManager.onTreeModeStarted += SetTreeMode;
        AppleTreeManager.onTreeModeEnded += SetGameMode;

	}

	private void OnDestroy()
	{
		PlayerDetection.onEnteredTreeZone -= EnteredTreeZoneCallback;
		PlayerDetection.onExitedTreeZone -= ExitedTreeZoneCallback;

        AppleTreeManager.onTreeModeStarted -= SetTreeMode;
        AppleTreeManager.onTreeModeEnded -= SetGameMode;
	}
	// Start is called before the first frame update
	void Start()
    {
        SetGameMode();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EnteredTreeZoneCallback(AppleTree tree)
    {
        treeButton.SetActive(true);
        toolButtonContainer.SetActive(false);
    }

	private void ExitedTreeZoneCallback(AppleTree tree)
	{
		treeButton.SetActive(false);
		toolButtonContainer.SetActive(true);
	}

    private void SetGameMode()
    {
        gamePanel.SetActive(true);
        treeModePanel.SetActive(false);
    }

	private void SetTreeMode(AppleTree tree)
	{
		gamePanel.SetActive(false);
		treeModePanel.SetActive(true);
	}
}
