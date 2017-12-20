/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

namespace TanksMP
{
    /// <summary>
    /// List of all keys saved on the user's device, be it for settings or selections.
    /// </summary>
    public class PrefsKeys
    {
        /// <summary>
		/// PlayerPrefs key for player name: UserXXXX
		/// </summary>
        public const string playerName = "TM_playerName";

        /// <summary>
        /// PlayerPrefs key for selected network mode: 0, 1 or 2
        /// </summary>
        public const string networkMode = "TM_networkMode";

        /// <summary>
        /// Server address for manual connection, e.g. in LAN games.
        /// This is only used when using Photon Networking, as UNET
        /// does support broadcast and automatic server discovery.
        /// </summary>
        public const string serverAddress = "TM_serverAddress";

        /// <summary>
        /// PlayerPrefs key for background music state: true/false
        /// </summary>
        public const string playMusic = "TM_playMusic";

        /// <summary>
        /// PlayerPrefs key for global audio volume: 0-1 range
        /// </summary>
        public const string appVolume = "TM_appVolume";
      
        /// <summary>
        /// PlayerPrefs key for recording gameplay: true/false
        /// </summary>
        public const string recordGame = "TM_recordGame";

        /// <summary>
        /// PlayerPrefs key for selected player model: 0/1/2 etc.
        /// </summary>
        public const string activeTank = "TM_activeTank";
    }
}
