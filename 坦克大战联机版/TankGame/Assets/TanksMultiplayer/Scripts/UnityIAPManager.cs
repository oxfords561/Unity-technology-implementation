/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using System;
using UnityEngine;
#if UNITY_ANALYTICS
using UnityEngine.Purchasing;
#endif

namespace TanksMP
{
    /// <summary>
    /// Manager handling the full in-app purchase workflow,
    /// granting purchases and catching errors using Unity IAP.
    /// </summary>
    #if UNITY_ANALYTICS
    public class UnityIAPManager : MonoBehaviour, IStoreListener
    #else
    public class UnityIAPManager : MonoBehaviour
    #endif
    {
        #pragma warning disable 0067
        /// <summary>
        /// Fired on failed purchases to deliver its product identifier.
        /// </summary>
        public static event Action<string> purchaseFailedEvent;
        
        /// <summary>
        /// Your Google Play app license key from the Google Play Developer Console.
        /// </summary>
        public string googlePlayLicenseKey;
        #pragma warning restore 0067

        #if UNITY_ANALYTICS
        //disable platform specific warnings, because Unity throws them
        //for unused variables however they are used in this context
        #pragma warning disable 0414
        private static IStoreController controller;
        private static IExtensionProvider extensions;
        #pragma warning restore 0414


        //initialize Unity IAP
        void Start()
        {
            //construct IAP purchasing instance and add the Google Play public key to it
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            builder.Configure<IGooglePlayConfiguration>().SetPublicKey(googlePlayLicenseKey);

            //iterate over all IAPProducts found in the scene and add their id to be looked up by Unity IAP
            IAPProduct[] products = Resources.FindObjectsOfTypeAll<IAPProduct>();
            foreach(IAPProduct product in products)
            {
				if(product.buyable)
                	builder.AddProduct(product.id, product.type);
            }

            //initialize Unity IAP with the configuration created above
            UnityPurchasing.Initialize(this, builder);
        }


        /// <summary>
        /// Called when Unity IAP is ready to make purchases, delivering the store controller
        /// (contains all online products) and platform specific extension
        /// </summary>
        public void OnInitialized (IStoreController ctrl, IExtensionProvider ext)
        {
            //cache references
            controller = ctrl;
            extensions = ext;

            //loop over products received and check if they are owned by this user
            //if so, hasReceipt returns true. Otherwise, delete any saved data for this product.
            #if UNITY_ANDROID
            foreach (Product p in ctrl.products.all)
            {	
                if(p.hasReceipt)
                    PlayerPrefs.SetString(Encryptor.Encrypt(p.definition.id), "");
            }
            PlayerPrefs.Save();
            #endif
        }


        /// <summary>
        /// Called when the user presses the 'Buy' button on an IAPProduct.
        /// </summary>
        public static void PurchaseProduct(string productId)
        {
            if(controller != null)
               controller.InitiatePurchase(productId);
        }


        /// <summary>
        /// Called when Unity IAP encounters an unrecoverable initialization error.
        /// </summary>
        public void OnInitializeFailed (InitializationFailureReason error)
        {
			Debug.Log(error);
        }


        /// <summary>
        /// Called when a purchase completes after being bought.
        /// </summary>
        public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs e)
        {
            //get all IAPProduct references in the scene, then loop over them
            IAPProduct[] products = FindObjectsOfType(typeof(IAPProduct)) as IAPProduct[];
            foreach(IAPProduct product in products)
            {
                //we have found the IAPProduct instance we have bought right now
                if(product.id == e.purchasedProduct.definition.id)
                {
                    //set the product to purchased
                    //if it is selectable, show its select button
                    product.Purchased();
                    if(product.selectButton)
                        product.IsSelected(true);
                    break;
                }
            }

            //save the encrypted identifier of this product on the device to keep it as purchased
            PlayerPrefs.SetString(Encryptor.Encrypt(e.purchasedProduct.definition.id), "");
            PlayerPrefs.Save();

            //return that we are done with processing the transaction
            return PurchaseProcessingResult.Complete;
        }


        /// <summary>
        /// Called when a purchase fails, providing the product and reason.
        /// </summary>
        public void OnPurchaseFailed (Product p, PurchaseFailureReason r)
        {
            if(purchaseFailedEvent != null)
                purchaseFailedEvent(r.ToString());
        }


        /// <summary>
        /// Method for restoring transactions which prompts users for their password (Apple only).
        /// </summary>
        public static void RestoreTransactions()
        {
            foreach (Product p in controller.products.all)
            {	
                if(!PlayerPrefs.HasKey(Encryptor.Encrypt(p.definition.id)))
                    PlayerPrefs.DeleteKey(Encryptor.Encrypt(p.definition.id));
            }

            if(extensions != null)
			    extensions.GetExtension<IAppleExtensions>().RestoreTransactions(null);
        }
        #endif
    }
}
