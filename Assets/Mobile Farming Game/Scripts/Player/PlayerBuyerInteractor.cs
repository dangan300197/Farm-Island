using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuyerInteractor : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private InventoryManager inventoryManager;


	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Buyer"))
        {
			SellCrop();
        }
	}


    private void SellCrop()
    {
        Inventory inventory = inventoryManager.GetInventory();
        InventoryItem[] items = inventory.GetInventoryItems();

        int coinsEarned = 0;

        for(int i=0; i < items.Length; i++)
        {
            //caculate the earning
            int itemPrice = DataManager.instance.GetCropPriceFromCropType(items[i].cropType);
            coinsEarned += itemPrice * items[i].amount;
        }

        TransactionEffectManager.instance.PlayCoinParticles(coinsEarned);

        //CashManager.instance.AddCoins(coinsEarned);

        //clearn the inventory
        inventoryManager.ClearInventory();
    }
}
