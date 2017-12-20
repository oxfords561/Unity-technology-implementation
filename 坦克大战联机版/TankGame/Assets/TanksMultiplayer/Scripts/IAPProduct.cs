/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;
using UnityEngine.UI;
#if UNITY_ANALYTICS
using UnityEngine.Purchasing;
#endif

namespace TanksMP
{
    /// <summary>
    /// Describes an in-app purchase product that can be bought by using Unity IAP.
    /// Contains several UI elements and logic for selecting/deselecting.
    /// </summary>
    public class IAPProduct : MonoBehaviour
    {
		/// <summary>
		/// Whether this product should be registered with Unity IAP.
        /// This should only be set to true if the product exists on an App Store.
		/// </summary>
		public bool buyable = true;

        /// <summary>
        /// The unique identifier for this product.
        /// For live products, this should match the id on the App Store.
        /// </summary>
        public string id;

		/// <summary>
		/// Unique value saved for selectable products to identify the current selection.
		/// </summary>
		public int value;

        #if UNITY_ANALYTICS
        /// <summary>
        /// In-app purchase type that should match the product type on the App Store.
        /// </summary>
        public ProductType type = ProductType.NonConsumable;
        #endif

        /// <summary>
        /// UI button that triggers the purchase workflow via Unity IAP.
        /// </summary>
        public GameObject buyButton;

        /// <summary>
        /// Optional elements which get enabled if this product has been sold.
        /// </summary>
        public GameObject sold;

        /// <summary>
        /// UI button that triggers the selection of this product in the shop.
        /// If a group has been assigned to its Toggle, other products are deselected.
        /// </summary>
        public GameObject selectButton;

        /// <summary>
        /// Optional elements which get enabled if this product has been selected.
        /// </summary>
        public GameObject selected;


        //sets the initial purchase/selection state
        void Awake()
        {
            //this product has been purchased already
            if (PlayerPrefs.HasKey(Encryptor.Encrypt(id)))
                Purchased();
            else if (!buyable)
            {
                //the product has not been bought yet, but it is not marked as buyable
                //on the App Store either. Meaning we hide the buy button and show the
                //select button for it directly instead.
                buyButton.SetActive(false);
                selectButton.SetActive(true);
            }
        }


        //validates the value saved on the device against the value of this product: if they match,
        //this means that we previously selected this product and reinitialize it as selected again
        void Start()
        {
            if (Encryptor.Decrypt(PlayerPrefs.GetString(PrefsKeys.activeTank)) == value.ToString())
                IsSelected(true);
        }


        /// <summary>
        /// Tries to open the purchase dialog this product via Unity IAP.
        /// </summary>
        public void Purchase()
        {
            #if UNITY_ANALYTICS
            if (!buyable) return;
            UnityIAPManager.PurchaseProduct(id);
            #endif
        }


        /// <summary>
        /// Sets this product UI state to 'purchased', hiding the buy button
        /// and showing the 'sold' gameobject, if specified.
        /// </summary>
        public void Purchased()
        {
            buyButton.SetActive(false);
            if (sold) sold.SetActive(true);
        }


        /// <summary>
        /// For already bought products: sets this product UI state to 'selected' and saves the
        /// current selection value on the device. If a product gets selected, this method is
        /// called for all other products in the same group too, with the boolean being false.
        /// Thus both the logic for selection and deselection is handled within this method.
        /// Invoked by the onValueChanged event on the select button inspector.
        /// </summary>
        public void IsSelected(bool thisSelect)
        {
            //we need to buy this product first
            if (buyButton.activeInHierarchy)
                return;

            //if this object has been selected
            if (thisSelect)
            {  
                //get a reference to the Toggle component on the select button
                Toggle toggle = selectButton.GetComponent<Toggle>();

                //in case this product is part of a group of items
                if (toggle.group)
                {
                    //because Toggle components on deactivated gameobjects do not receive onValueChanged events,
                    //here we implement a hacky way to deselect all other Toggles, even deactivated ones
                    IAPProduct[] others = toggle.group.GetComponentsInChildren<IAPProduct>(true);
                    for (int i = 0; i < others.Length; i++)
                    {
                        //unselect the iterated product if it is not the selected product.
                        if (others[i].selectButton != null && others[i] != this)
                        {
                            others[i].IsSelected(false);
                        }
                    }
                }

                //display that this product is selected
                toggle.isOn = true;
                selectButton.SetActive(false);
                if (selected) selected.SetActive(true);

                //save the selection value to the device
				PlayerPrefs.SetString(PrefsKeys.activeTank, Encryptor.Encrypt(value.ToString()));
            }
            else
            {
                //if another object has been selected, show the select button
                //for this product and unset the 'selected' state
                if (selectButton) selectButton.SetActive(true);
                if (selected) selected.SetActive(false);
            }
        }
    }
}
