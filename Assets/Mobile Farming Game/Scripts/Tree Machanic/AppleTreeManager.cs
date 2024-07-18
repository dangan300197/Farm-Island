using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;

public class AppleTreeManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Slider shakeSlider;

    [Header("Settings")]
    private AppleTree lastTriggeredTree;

    [Header("Actions")]
    public static Action<AppleTree> onTreeModeStarted;
    public static Action onTreeModeEnded;


	private void Awake()
	{
        PlayerDetection.onEnteredTreeZone += EnteredTreeZoneCallback;
	}

	private void OnDestroy()
	{
        PlayerDetection.onEnteredTreeZone -= EnteredTreeZoneCallback;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EnteredTreeZoneCallback(AppleTree tree)
    {
        lastTriggeredTree = tree;
    }


	public void TreeButtonCallback()
    {
        Debug.Log("tree button clicked");

        if (!lastTriggeredTree.IsReady())
        {
            Debug.Log("Not ready");
            return;
        }

        StartTreeMode();
	}

    private void StartTreeMode()
    {
		lastTriggeredTree.Initialize(this);
		

        onTreeModeStarted?.Invoke(lastTriggeredTree);

        //Initialize the slider
        UpdateShakeSlider(0);
	}

    public void UpdateShakeSlider(float value)
    {
        shakeSlider.value = value;
    }

    public void EndTreeMode()
    {
        onTreeModeEnded?.Invoke();
    }
}
