/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using System;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;

namespace TanksMP
{
    /// <summary>
    /// Encrypts strings passed in by using a set obfuscation key as well as
    /// the unique identifier of the running device. Used for storing in-app purchases.
    /// </summary>
    public class Encryptor : MonoBehaviour
    {
        //reference to this script instance
        private static Encryptor instance;

        /// <summary>
        /// Whether encryption should be enabled.
        /// </summary>
        public bool encrypt = false;

        /// <summary>
        /// 56+8 bit key for encrypting strings: 8 characters, do not use code
        /// characters (=.,? etc) and play-test that your key actually works!
        /// On Windows Phone this key must be exactly 16 characters (128 bit) long.
        /// SAVE THIS KEY SOMEWHERE ON YOUR END, SO IT DOES NOT GET LOST ON UPDATES.
        /// </summary>
        public string secret = "abcd1234";


        //set reference
		void Awake()
		{
			instance = this;
		}


        /// <summary>
        /// Returns a reference to this script instance.
        /// </summary>
        public static Encryptor GetInstance()
        {
			return instance;
        }


        /// <summary>
        /// Encrypt string based on secret key + device identifier.
        /// </summary>
        public static string Encrypt(string toEncrypt)
        {
            //if encryption is not turned on, just return it back
            if (!instance.encrypt) return toEncrypt;
            //attach device identifier to the encryption string
            toEncrypt += SystemInfo.deviceUniqueIdentifier;
			
            #pragma warning disable 0219
            //convert secret key and input string to byte array
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(instance.secret);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            byte[] resultArray = null;
            #pragma warning restore 0219

            #if UNITY_WP8
                //DES is not available on windows phones, we use AESManaged instead
                AesManaged aes = new AesManaged();
                aes.Key = keyArray;
                ICryptoTransform cTransform = aes.CreateEncryptor();
                //hack first 16 characters and put them at the end to avoid malformed IV input 
                Array.Resize(ref toEncryptArray, toEncryptArray.Length + 16);
                Array.Copy(toEncryptArray, 0, toEncryptArray, toEncryptArray.Length - 16, 16);
                resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            #elif (UNITY_ANDROID || UNITY_IPHONE)
                //create new DES service and set all necessary properties
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Key = keyArray;
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.PKCS7;
                //create DES encryptor
                ICryptoTransform cTransform = des.CreateEncryptor();
                //encrypt input array, then convert back to string
                //and return final encrypted (unreadable) string
                resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            #else
                keyArray = null;
                resultArray = toEncryptArray;
            #endif

            //return encrypted string
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

		
        /// </summary>
        /// Decrypt string based on secret key + device identifier.
        /// </summary>
        public static string Decrypt(string toDecrypt)
        {
            //if encryption is not turned on, just return it back
            if (!instance.encrypt) return toDecrypt;
           
            #pragma warning disable 0219
            //convert secret key and input string to byte array
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(instance.secret);
            byte[] toDecryptArray = Convert.FromBase64String(toDecrypt);
            byte[] resultArray = null;
            #pragma warning restore 0219

            #if UNITY_WP8
                AesManaged aes = new AesManaged();
                aes.Key = keyArray;
                ICryptoTransform cTransform = aes.CreateDecryptor();
                resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
                //hack array again and put last block back to the beginning
                Array.Copy(resultArray, resultArray.Length - 16, resultArray, 0, 16);
                Array.Resize(ref resultArray, resultArray.Length - 16);
            #elif (UNITY_ANDROID || UNITY_IPHONE)
                //create new DES service and set all necessary properties
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Key = keyArray;
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.PKCS7;
                //create DES decryptor
                ICryptoTransform cTransform = des.CreateDecryptor();
                //decrypt input array, then convert back to string
                //and return final decrypted (raw) string
                resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
            #else
                keyArray = null;
                resultArray = toDecryptArray;
            #endif

            //strip device identifier and return decrypted string
            return (UTF8Encoding.UTF8.GetString(resultArray, 0, resultArray.Length))
				   .Replace(SystemInfo.deviceUniqueIdentifier, String.Empty);
        }
    }
}