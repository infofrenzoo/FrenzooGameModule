using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using TMPro;
public class ModuleActionInfo : MonoBehaviour
{
	#region Data
	public ShopModuleActionType ShopAction;
    public int ContentId; //can be 0
    public RewardGroup RewardGroup;
    [HideInInspector]
    public Product Product;
    #endregion

    public Button Btn;
    public TMP_Text Text;
    //Dont Edit Product


    void Start()
    {
        Btn.onClick.AddListener(()=> { GetComponentInParent<BaseModule>().OnButtonClicked(this); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
