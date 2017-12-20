/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;

namespace TanksMP
{
    /// <summary>
    /// When attached to an gameobject containing ParticleSystems,
    /// handles automatic despawn after the maximum particle duration.
    /// </summary>
    public class PoolParticle : MonoBehaviour 
    {
        /// <summary>
        /// Delay before despawning this gameobject. Calculated based on the ParticleSystem duration,
        /// but can be overwritten by setting it to something greater than zero.
        /// </summary>
        public float delay = 0f;
        
        //references to all ParticleSystem components
        private ParticleSystem[] pSystems;
        
        
        //initialize variables
        void Awake()
        {
            pSystems = GetComponentsInChildren<ParticleSystem>();
            
            //don't continue if delay has been overwritten
            //otherwise find the maximum particle duration
            if(delay > 0) return;
            for(int i = 0; i < pSystems.Length; i++)
            {
                var main = pSystems[i].main;
                if(main.duration > delay)
                    delay = main.duration;
            }
        }
        
        
        //play particles
        void OnSpawn()
        {
            //loop over ParticleSystem references and play them
            //Unity does not seem to calculate a new iteration of particles when
            //particles get activated, so here we add a randomized seed to it too
            for(int i = 0; i < pSystems.Length; i++)
            {
				pSystems[i].Stop();
                pSystems[i].randomSeed = (uint)Random.Range(0f, uint.MaxValue);
                pSystems[i].Play();
            }

            //set automatic despawn after play duration
            PoolManager.Despawn(gameObject, delay);
        }
    }
}
