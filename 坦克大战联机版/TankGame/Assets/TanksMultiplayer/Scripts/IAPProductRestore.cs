/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;

namespace TanksMP
{
    /// <summary>
    /// Simple script to handle restoring purchases on iOS. Restoring purchases is a
    /// requirement by Apple and your app will be rejected if you do not provide it.
    /// </summary>
    public class IAPProductRestore : MonoBehaviour
    {
        //only show the restore button on iOS
        void Start()
        {
            #if !UNITY_IPHONE
                gameObject.SetActive(false);
            #endif
        }


        /// <summary>
        /// Calls Unity IAPs RestoreTransactions method.
        /// It makes sense to add this to an UI button event.
        /// </summary>
        public void Restore()
        {
            #if UNITY_ANALYTICS
            UnityIAPManager.RestoreTransactions();
            #endif
        }
    }
}
