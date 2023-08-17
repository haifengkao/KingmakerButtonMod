using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;

using UnityEngine;

using UnityEngine.UI;
using Harmony12;

using KingmakerBuffBot;
using System.Reflection;
using Kingmaker.PubSubSystem;
using Kingmaker;
using ModMaker.Utility;
using static UnityModManagerNet.UnityModManager.Param;

namespace KingmakerButtonMod
{
    static class Main
    {

        public static bool Enabled;
        public static UnityModManager.ModEntry ModEntry;
        public static UnityModManager.ModEntry.ModLogger Logger
        {
            get
            {
                return ModEntry.Logger;
            }
        }
        static ContainersUIController ContainersUIController;
        static bool IsBuffBotLoaded => IsModLoaded("KingmakerBuffBot");
        static bool IsModLoaded(string modId)
        {
            var modEntry = UnityModManager.FindMod(modId);
            return modEntry != null && modEntry.Active;
        }

        static bool Load(UnityModManager.ModEntry modEntry)
        {

                modEntry.OnToggle = OnToggle;
                modEntry.OnGUI = OnGUI;
                ModEntry = modEntry;

            Main.Logger?.Log("Load2");
            return true;
          
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {

            try
            {

                //Main.Logger?.Log("OnToggle");
                Enabled = value;

                if (Enabled)
                {

                    ContainersUIController = new ContainersUIController();

                    ContainersUIController?.HandleModEnable();
                }
                else
                {
                    ContainersUIController?.HandleModDisable();
                }

                Main.Logger?.Log("<--OnToggle");
               
            }
            catch (Exception ex)
            {
                Main.Logger?.Error($"Exception: {ex.Message}\nStack Trace: {ex.StackTrace}");
            }
            return true;
        }
        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginVertical();
     
            GUILayout.Label($"BuffBotLoaded {IsBuffBotLoaded}");

            
            if (CanBuff)
            {
                GUILayout.Label("Buff method is found");
            }
                  

            GUILayout.EndVertical();

        }

        public static void BeginBuff()
        {
              if (!CanBuff) { 
                Main.Logger?.Log("Can't buff");
                return;
            }

            KingmakerBuffBot.Main.ExecutionsBoth();
            KingmakerBuffBot.Main.AttachProfilesManager();
           
        }

     

        static bool CanBuff
        {
           get {
                return IsBuffBotLoaded;
            }
        }

    }
}

