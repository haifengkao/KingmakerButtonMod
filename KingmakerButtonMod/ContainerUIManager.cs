
using Kingmaker.GameModes;
using Kingmaker.UI.Constructor;

using Kingmaker.UI;
using Kingmaker;


using TMPro;
using UnityEngine.Events;
using UnityEngine;

using UnityEngine.UI;


namespace KingmakerButtonMod
{
   
    class ContainersUIManager : MonoBehaviour
    {
        

        private ButtonWrapper _buttonWrapper;
        GameObject _button;
        private static GameObject hud;
        private static GameObject _tooglePanel;

        public static ContainersUIManager CreateObject()
        {
            Main.Logger.Log("CreateObject");
            UICommon uiCommon = Game.Instance.UI.Common;
            hud = uiCommon?.transform.Find("HUDLayout")?.gameObject;

            GameObject tooglePanel = uiCommon?.transform.Find("HUDLayout/CombatLog/TooglePanel/ToogleAll")?.gameObject;

            if (!hud || !tooglePanel)
            {


                Main.Logger.Log($"hud not found 2 ");


                if (uiCommon?.transform == null)
                {
                    Main.Logger.Log("transform is null");
                }
                else
                {
                    foreach (Transform child in uiCommon.transform)
                    {
                        Main.Logger.Log(child.name);
                    }
                }
               

                return null;
            }

            Main.Logger.Log("success");
            if (uiCommon?.transform == null)
            {
                Main.Logger.Log("transform is null");
            }
            else
            {
                foreach (Transform child in uiCommon.transform)
                {
                    Main.Logger.Log(child.name);
                }
            }

            _tooglePanel = tooglePanel;
            return hud.AddComponent<ContainersUIManager>();
        }

        private static void SetContainerPosition(RectTransform containersRect, GameObject menuButtons)
        {
            RectTransform menuButtonsTransform = (RectTransform)menuButtons.transform;

            ResetRect(containersRect);

            // anchor and pivot at the bottom-left
            containersRect.anchorMin = new Vector2(0, 0);
            containersRect.anchorMax = new Vector2(0, 0);
            containersRect.pivot = new Vector2(0, 0);

            // Set the size
            containersRect.sizeDelta = new Vector2(48, 48);


            float desiredPadding = 10f;

            float topY = menuButtonsTransform.anchoredPosition.y + (menuButtonsTransform.sizeDelta.y * (1 - menuButtonsTransform.pivot.y));

            // Position it at the bottom-top corner of the menuButtons
            containersRect.anchoredPosition = new Vector2(menuButtonsTransform.anchoredPosition.x, topY + desiredPadding);

            Main.Logger.Log($"anchoredPosition.y {menuButtonsTransform.anchoredPosition.y} {topY} {menuButtonsTransform.sizeDelta.y} {menuButtonsTransform.pivot.y}");
        }

        void Awake()
        {
            Main.Logger.Log("awake");
           
            GameObject menuButtons = Game.Instance.UI.Common?.transform.Find("HUDLayout/Menu_Buttons48px")?.gameObject;

            if (!menuButtons)
            {
                Main.Logger.Log($"menuButtons not found");
                return;
            }

            GameObject containers = new GameObject("MyKingmakerBuffButton", typeof(RectTransform));
            containers.transform.SetParent(hud.transform);
            containers.transform.SetSiblingIndex(0);

            SetContainerPosition((RectTransform)containers.transform, menuButtons);
            

            //initialize buttons
            GameObject toggleScrolls = Instantiate(_tooglePanel, containers.transform, false);
            toggleScrolls.transform.SetSiblingIndex(0);
            setToggleButtons(toggleScrolls, "MyButton4");
            ResetPosition(toggleScrolls);
            _button = toggleScrolls;
           

            void setToggleButtons(GameObject button, string name)
            {
                button.name = name;
                DestroyImmediate(button.GetComponent<Toggle>());
                button.AddComponent<ButtonPF>();
            }

            _buttonWrapper = new ButtonWrapper((RectTransform)toggleScrolls.transform, "B", HandleToggleScrolls);

            Camera uiCamera = Camera.current;
            
            Rect screenRect = GetScreenCoordinates(toggleScrolls.GetComponent<RectTransform>(), uiCamera);

            Main.Logger.Log($"Button is displayed from {screenRect.x}, {screenRect.y} with width {screenRect.width} and height {screenRect.height} in screen coordinates.");



            Rect screenRect4 = GetScreenCoordinates(containers.GetComponent<RectTransform>(), uiCamera);
            Main.Logger.Log($"containers is displayed from {screenRect4.x}, {screenRect4.y} with width {screenRect4.width} and height {screenRect4.height} in screen coordinates.");

          
    
        }

        public static void ResetPosition(GameObject obj)
        {
            RectTransform transform = obj.GetComponent<RectTransform>();
            ResetRect(transform);
            FillParent(transform);

        }
        public static void ResetRect(RectTransform rectTransform)
        {
            // Reset local position
            rectTransform.localPosition = Vector3.zero;

            // Reset local scale
            rectTransform.localScale = Vector3.one;

            // Reset rotation
            rectTransform.localRotation = Quaternion.identity;

            // Optionally: reset other RectTransform specific properties
            // depending on your use case:
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        public static void FillParent(RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1); // This will stretch it to fill the entire parent
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = Vector2.zero; // Setting this to zero ensures it fills the parent
            rectTransform.anchoredPosition = Vector2.zero;
        }

        public static Rect GetScreenCoordinates(RectTransform transform, Camera uiCamera)
        {
            Vector2 sizeDelta = transform.rect.size;
            Vector3[] corners = new Vector3[4];

            // Get the four corners of the RectTransform in world space
            transform.GetWorldCorners(corners);

            // Convert the world space coordinates of the bottom left corner to screen space
            Vector2 screenBottomLeft = RectTransformUtility.WorldToScreenPoint(uiCamera, corners[0]);

            // Convert the world space coordinates of the top right corner to screen space
            Vector2 screenTopRight = RectTransformUtility.WorldToScreenPoint(uiCamera, corners[2]);

            // Create a Rect from the two points and return
            return new Rect(screenBottomLeft.x, screenBottomLeft.y, screenTopRight.x - screenBottomLeft.x, screenTopRight.y - screenBottomLeft.y);
        }


        private void HandleToggleScrolls()
        {
            Main.Logger.Log("toogle");
            Main.BeginBuff();
        }


        void Update()
        {
            if (Game.Instance.CurrentMode == GameModeType.Default ||
                Game.Instance.CurrentMode == GameModeType.EscMode ||
                Game.Instance.CurrentMode == GameModeType.Pause)
            {
                //gameObject.transform.position = SetMenuPosition(500, 500, hud.transform.position);
                _button.transform.gameObject.SetActive(true);

            }
            else
            {
              
                _button.transform.gameObject.SetActive(false);
            }
        }

        private static Vector3 SetMenuPosition(float x, float y, Vector3 z)
        {
            float ascpectRatio = (float)Screen.width / (float)Screen.height;
            return Camera.current.ScreenToWorldPoint
                (new Vector3(Screen.width * x, (Screen.height * y) * (ascpectRatio * 0.5625f), Camera.current.WorldToScreenPoint(z).z));
        }

    }

    class ButtonWrapper
    {

        public bool ButtonToggle { get; set; }
        private readonly Color _enableColor = Color.white;
        private readonly Color _disableColor = new Color(0.7f, 0.8f, 1f);
        public readonly RectTransform _button;
        private readonly ButtonPF _toggle;
        private readonly TextMeshProUGUI _textMesh;


        public ButtonWrapper(RectTransform button, string text, UnityAction OnToggle)
        {

            this.ButtonToggle = false;
            _button = button;
            _toggle = _button.GetComponent<ButtonPF>();

            _toggle.onClick.AddListener(new UnityAction(OnToggle));
            //_button = new Button.ButtonClickedEvent();
            //_button.onClick.AddListener(new UnityAction(onClick));
            _textMesh = _button.GetComponentInChildren<TextMeshProUGUI>();
            //_textMesh.fontSize = 20;
            //_textMesh.fontSizeMax = 72;
            //_textMesh.fontSizeMin = 18;
            _textMesh.text = text;
            //_textMesh.color = _button.interactable ? _enableColor : _disableColor;
            //_image = _button.gameObject.GetComponent<Image>();
            //_defaultSprite = _image.sprite;
            //_defaultSpriteState = _button.spriteState;
            //_pressedSpriteState = _defaultSpriteState;
            //_pressedSpriteState.disabledSprite = _pressedSpriteState.pressedSprite;
            //_pressedSpriteState.highlightedSprite = _pressedSpriteState.pressedSprite;
        }
    }
}
