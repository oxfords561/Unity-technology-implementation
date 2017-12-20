/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

namespace TanksMP
{
    /// <summary>
    /// Custom powerup implementation for adding player shield points.
    /// </summary>
	public class PowerupShield : Powerup 
	{
        /// <summary>
        /// Amount of shield points to add per consumption.
        /// </summary>
        public int amount = 3;


        /// <summary>
        /// Overrides the default behavior with a custom implementation.
        /// Check for the current shield and adds additional shield points.
        /// </summary>
		public override bool Apply(Player p)
		{
            //don't add shield if it is at the maximum already
            if (p.shield == amount)
                return false;

            //assign absolute shield points to player
            //we can't go over the maximum thus no need to check it here
            p.shield = amount;

            //return successful consumption
            return true;
		}
	}
}
