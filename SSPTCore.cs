using HarmonyLib;
using System;
using UnityEngine;

namespace ServerSidePracticeTools
{
    /// <summary>
    /// The core of the mod, containing the enable/disable logic and core functionality.
    /// </summary>
    public class SSPTCore : IPuckMod
    {

        public CommandParser commandParser = new CommandParser();

        /// <summary>
        /// Enables the mod when called during Puck mod loading.
        /// </summary>
        /// <returns>true if enable succeeded, false if enable failed</returns>
        public bool OnEnable()
        {
            try
            {
                Debug.Log($"[{SSPTConstants.MOD_NAME}] Enabling SSPT version {SSPTConstants.MOD_VERSION}...");

                commandParser.AddAsListener(EventManager.Instance);

                Debug.Log($"[{SSPTConstants.MOD_NAME}] SSPT successfully enabled.");

                return true;
            }
            catch (Exception e)
            {
                Debug.Log($"[{SSPTConstants.MOD_NAME}] SSPT failed to enable: {e}");
                return false;
            }
        }

        /// <summary>
        /// Disables the mod when called.
        /// </summary>
        /// <returns>true if disable succeeded, false if disable failed</returns>
        public bool OnDisable()
        {
            try
            {
                Debug.Log($"[{SSPTConstants.MOD_NAME}] Disabling SSPT...");
                commandParser = null;
                Debug.Log($"[{SSPTConstants.MOD_NAME}] SSPT disabled.");
                return true;
            }
            catch (Exception e)
            {
                Debug.Log($"[{SSPTConstants.MOD_NAME}] SSPT failed to disable: {e}");
                return false;
            }
        }
    }
}