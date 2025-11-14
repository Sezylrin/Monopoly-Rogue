using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
public class boolEventArgs : EventArgs
{
    public bool isSuccess { get; set; }
}
public class BaseItem : MonoBehaviour
{
    [SerializeField]
    protected ItemSO itemSO;

    public EventHandler<boolEventArgs> OnItemUse;

    protected ItemManager manager;

    public void AssignManager(ItemManager manager)
    {
        this.manager = manager;
    }
    /// <summary>
    /// if item was not succesfully used
    /// </summary>
    public virtual void ItemUseCancel()
    {
        Debug.Log(gameObject.name + " item failed to use");
        OnItemUse?.Invoke(this, new boolEventArgs { isSuccess = false });
    }
    /// <summary>
    /// if item was succesfully used
    /// </summary>
    public virtual void ItemUseSuccessful()
    {
        OnItemUse?.Invoke(this, new boolEventArgs { isSuccess = true });
    }
    /// <summary>
    /// Called when item is to be used
    /// </summary>
    public virtual void AttemptItemUse()
    {

    }
    /// <summary>
    /// When the item is generated
    /// </summary>
    /// <param name="itemSO"></param>
    public virtual void OnGenerate(ItemSO itemSO)
    {
        this.itemSO = itemSO;
    }
    public ItemSO GetSO()
    {
        return itemSO;
    }
}
