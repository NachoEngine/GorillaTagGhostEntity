using BepInEx;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using Utilla;

namespace GorillaTagGhostEntity
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;

        void OnEnable()
        {
            /* Set up your mod here */
            /* Code here runs at the start and whenever your mod is enabled*/

            HarmonyPatches.ApplyHarmonyPatches();
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnDisable()
        {
            /* Undo mod setup here */
            /* This provides support for toggling mods with ComputerInterface, please implement it :) */
            /* Code here runs whenever your mod is disabled (including if it disabled on startup)*/

            HarmonyPatches.RemoveHarmonyPatches();
            Utilla.Events.GameInitialized -= OnGameInitialized;
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            Debug.Log("GhostEntity");
            Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("GorillaTagGhostEntity.Assets.ghostentity");
            AssetBundle bundle = AssetBundle.LoadFromStream(str);
            GameObject asset = bundle.LoadAsset<GameObject>("GhostEntityParent 1");
            var localasset = Instantiate(asset);

            
            Entity = localasset;
            GameObject soundobj = GameObject.Find("GhostEntityObject");
            AudioSource[] Sounds;
            Sounds = soundobj.GetComponents<AudioSource>();
            daisy = Sounds[0];
            daisy.volume = 0.3f;
            join = Sounds[1];
            localasset.SetActive(false);

        }
        public GameObject Entity;
        public AudioSource daisy;
        public AudioSource join;
        public bool flag = false;
        void Update()
        {
            if (inRoom)
            {
                if (!flag)
                {
                    flag = true;
                    Entity.SetActive(true);
                    join.Play();
                    daisy.Play();
                }
                
            }
            else
            {
                if (flag)
                {
                    flag = false;
                    Entity.SetActive(false);
                }
                
            }
        }

        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            /* Activate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = true;
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            /* Deactivate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = false;
        }
    }
}
