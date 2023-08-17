
using ModMaker;


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
            ContainersUI?.SafeDestroy();
            ContainersUI = null;
        }

        public void Update()
        {
            Detach();
            Attach();
        }


        public void HandleModEnable()
        {
            Main.Logger.Log("HandleModEnable");

            Attach();

            EventBus.Subscribe(this);
        }

        public void HandleModDisable()
        {

            Main.Logger.Log("HandleModDisable");
            EventBus.Unsubscribe(this);

            Detach();
           
        }

        public void OnAreaBeginUnloading()
        {
            Main.Logger.Log("OnAreaBeginUnloading");
        }

        public void OnAreaDidLoad()
        {
            Main.Logger.Log("OnAreaDidLoad");
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
