using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.GameModes;
using Kingmaker.UI.Constructor;

using Kingmaker.UI;
using Kingmaker;

using System.Reflection;

using static System.Net.Mime.MediaTypeNames;

using TMPro;
using UnityEngine.Events;
using UnityEngine;



namespace KingmakerButtonMod
{
    class ContainersUIManager : MonoBehaviour
    {


        private ButtonWrapper _button;

        private static GameObject hud;

        public static ContainersUIManager CreateObject()
        {
            UICommon uiCommon = Game.Instance.UI.Common;
            hud = uiCommon?.transform.Find("HUDLayout")?.gameObject;


            if (!hud)
            {
                return null;
            }



            return hud.AddComponent<ContainersUIManager>();
        }

        void Awake()
        {

            // Now add your button to this panel
            GameObject newButton = new GameObject("MyNewButton");
            newButton.AddComponent<ButtonPF>();
            newButton.name = "MyNew";

            // Set button properties and listeners...

            newButton.transform.SetParent(hud.transform);
            newButton.transform.SetSiblingIndex(0);


            _button = new ButtonWrapper(newButton, "MyNew2", HandleToggleScrolls);

        }



        private void HandleToggleScrolls()
        {

        }


        void Update()
        {
            if (Game.Instance.CurrentMode == GameModeType.Default ||
                Game.Instance.CurrentMode == GameModeType.EscMode ||
                Game.Instance.CurrentMode == GameModeType.Pause)
            {

                _button._button.SetActive(true);

            }
            else
            {

                _button._button.SetActive(false);
            }
        }

    }

    class ButtonWrapper
    {

        public bool ButtonToggle { get; set; }
        private readonly Color _enableColor = Color.white;
        private readonly Color _disableColor = new Color(0.7f, 0.8f, 1f);
        public readonly GameObject _button;
        private readonly ButtonPF _toggle;
        private readonly TextMeshProUGUI _textMesh;


        public ButtonWrapper(GameObject button, string text, UnityAction OnToggle)
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
