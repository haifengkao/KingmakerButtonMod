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

        static ContainersUIController ContainersUIController;
        static bool IsBuffBotLoaded() => IsModLoaded("KingmakerBuffBot");
        static bool IsModLoaded(string modId)
        {
            var modEntry = UnityModManager.FindMod(modId);
            return modEntry != null && modEntry.Active;
        }

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;

            if (Enabled)
            {
                if (ContainersUIController == null) { 
                     ContainersUIController = new ContainersUIController();
                 }
                ContainersUIController.HandleModEnable();
            }
            else
            {
                ContainersUIController.HandleModDisable();
            }
            return true;
        }
        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
     
            GUILayout.Label($"BuffBotLoaded {IsBuffBotLoaded()}");

            Type mainType = Assembly.Load("KingmakerBuffBot").GetType("KingmakerBuffBot.Main");
            if (mainType != null)
            {
                MethodInfo executeMethod = mainType.GetMethod("Execute", BindingFlags.Static | BindingFlags.NonPublic);
                if (executeMethod != null)
                {
                    //executeMethod.Invoke(null, null);

                    GUILayout.Label("Find Execute method");
                }
            }
            GUILayout.EndHorizontal();

        }

    }
}

