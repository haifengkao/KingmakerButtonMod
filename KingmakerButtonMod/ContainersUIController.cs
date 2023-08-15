using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using ModMaker;
using static UnityModManagerNet.UnityModManager.Param;

using UnityEngine;
using Kingmaker.PubSubSystem;
using ModMaker.Utility;

namespace KingmakerButtonMod
{
    class ContainersUIController : IModEventHandler, ISceneHandler
    {
        public int Priority => 400;

        public ContainersUIManager ContainersUI { get; private set; }

        public void Attach()
        {
            if (!ContainersUI)
                ContainersUI = ContainersUIManager.CreateObject();
        }

        public void Detach()
        {
            ContainersUI.SafeDestroy();
            ContainersUI = null;
        }

        public void Update()
        {
            Detach();
            Attach();
        }


        public void HandleModEnable()
        {

            Attach();

            EventBus.Subscribe(this);
        }

        public void HandleModDisable()
        {
          

            EventBus.Unsubscribe(this);

            Detach();
           
        }

        public void OnAreaBeginUnloading()
        {

        }

        public void OnAreaDidLoad()
        {
            Attach();
        }
    }
}

namespace ModMaker
{
    public interface IModEventHandler
    {
        int Priority { get; }

        void HandleModEnable();

        void HandleModDisable();
    }
}
namespace ModMaker.Utility
{
    public static class UnityExtensions
    {
        private static void SafeDestroyInternal(GameObject obj)
        {
            obj.transform.SetParent(null, false);
            obj.SetActive(false);
            UnityEngine.Object.Destroy(obj);
        }

        public static void SafeDestroy(this GameObject obj)
        {
            if (obj)
            {
                SafeDestroyInternal(obj);
            }
        }

        public static void SafeDestroy(this Component obj)
        {
            if (obj)
            {
                SafeDestroyInternal(obj.gameObject);
            }
        }
    }
}
