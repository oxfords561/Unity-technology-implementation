/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

namespace TanksMP
{
    /// <summary>
    /// Custom powerup implementation for changing the player's bullet.
    /// </summary>
	public class PowerupBullet : Powerup 
	{
        /// <summary>
        /// Amount of shots before returning to the default bullet.
        /// </summary>
        public int amount = 5;
        
        /// <summary>
        /// Index of the new bullet, on the Player script, that should be assigned.
        /// </summary>
        public int bulletIndex = 1;


        /// <summary>
        /// Overrides the default behavior with a custom implementation.
        /// Check for the current bullet and refills its ammunition.
        /// </summary>
		public override bool Apply(Player p)
		{
            //do not consume powerup if the player owns the new bullet already
            //and the ammunition is at the maximum amount available
            if (p.currentBullet == bulletIndex && p.ammo == amount)
                return false;

            //otherwise assign new bullet and refill ammo
            p.currentBullet = bulletIndex;
            p.ammo = amount;

            //return successful consumption
            return true;
		}
	}
}
