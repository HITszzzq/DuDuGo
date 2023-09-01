using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject slotPrefeb;
    public const int numSlots = 4;
    GameObject[] slots = new GameObject[numSlots];
    Item[] items = new Item[numSlots];
    Image[] itemImages = new Image[numSlots];
    Text[] slotTxts = new Text[numSlots];
    Button[] slotBtns = new Button[numSlots];
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        CreateSlots();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateSlots()
    {
        if(slotPrefeb != null)
        {
            for(int i = 0; i < numSlots; i++)
            {
                GameObject newSlot = Instantiate(slotPrefeb);
                newSlot.name = "ItemSlot_" + i;
                newSlot.transform.SetParent(gameObject.transform.GetChild(0).transform);
                slots[i] = newSlot;
                itemImages[i] = slots[i].GetComponent<Slot>().itemImage;
                slotTxts[i] = slots[i].GetComponent<Slot>().qtyTxt;
                slotBtns[i] = slots[i].GetComponent<Slot>().slotBtn;
                int temp = i;
                slotBtns[i].onClick.AddListener(delegate(){
                    DropItem(temp);
                });

            }
        }
    }

    public void AddItem(Item itemToAdd)
    {
        for(int i = 0; i < numSlots; i++)
        {
            if(items[i] != null && items[i].itemType == itemToAdd.itemType)
            {
                if(items[i].stackable == true)
                {
                    items[i].quantity += 1;
                    slotTxts[i].text = items[i].quantity.ToString();
                    return;
                }
                else
                {
                    return;
                }
            }
        }

        for(int i = 0; i < numSlots; i++)
        {
            if(items[i] == null)
            {
                items[i] = Instantiate(itemToAdd);
                items[i].quantity += 1;
                itemImages[i].sprite = itemToAdd.sprite;
                itemImages[i].enabled = true;
                if(items[i].stackable == true)
                {
                    slotTxts[i].text = items[i].quantity.ToString();
                    return;
                }
                else
                {
                    return;
                }
            }
        }

        return;
    }

    private void DropItem(int index)
    {
        if(items[index] == null)
        {
            return;
        }
        else
        {
            if(items[index].stackable == true)
            {
                items[index].quantity -= 1;
                slotTxts[index].text = items[index].quantity.ToString();
                switch(items[index].itemType)
                {
                    case Item.ItemType.HEALTH:
                        player.AdjustHealthPoints(items[index].amount);
                        break;
                    case Item.ItemType.STAMINA:
                        player.AdjustStaminaPoints(items[index].amount);
                        break;
                    default:
                        break;
                }
                if(items[index].quantity <= 0)
                {
                    items[index] = null;
                    itemImages[index].sprite = null;
                    itemImages[index].enabled = false;
                    slotTxts[index].text = "";
                }
            }
            else
            {
                items[index].quantity -= 1;
                //switch (items[index].itemType)
                //{
                //    case Item.ItemType.FIREBALL:
                //        Debug.Log("xxxxxxfireBall");
                //        break;
                //    case Item.ItemType.WATERBALL:
                //        Debug.Log("xxxxxxwaterBall");
                //        break;
                //    default:
                //        break;
                //}
                Weapon weaponScript = player.GetComponent<Weapon>();
                weaponScript.ChangeAmmoPool(items[index]);
                items[index] = null;
                itemImages[index].sprite = null;
                itemImages[index].enabled = false;
                slotTxts[index].text = "";
            }
        }
    }
}
