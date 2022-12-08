namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections.Generic;
    using TMPro;

    public class MiniMapCanvas : MonoBehaviour
    {
        public static MiniMapCanvas Instance = null;
        public GameObject miniMapObject;
        public MiniMapCamera miniMapCamera;
        public Transform playerTransform;
        public List<MiniMapIcon> miniMapIcons;
        public Transform iconParentTransform;
        public TextMeshProUGUI districtText;
        public TextMeshProUGUI streetText;
        public RectTransform canvasRect;
        public GameObject locationInfo;
        Vector3[] corners = new Vector3[4];

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            canvasRect = GetComponent<RectTransform>();
            SetMiniMapState();
            SetFixedMiniMapRotationState();
        }

        public void SetMiniMapState()
        {
            bool isOn = GameData.GetMiniMapState() == "ON" ? true : false;
            miniMapObject.SetActive(isOn);
        }

        public void SetFixedMiniMapRotationState()
        {
            bool isOn = GameData.GetFixedMiniMapRotationState() == "ON" ? true : false;
            miniMapCamera.fixedRotation = isOn;
        }

        public void RegisterMiniMapIcon(GameObject _iconObject, Image _icon, bool _updateRotation, bool _isPlayer)
        {
            Image image = Instantiate(_icon);
            MiniMapIcon miniMapIcon = new MiniMapIcon();
            miniMapIcon.isPlayer = _isPlayer;
            miniMapIcon.updateRotation = _updateRotation;
            miniMapIcon.icon = image;
            miniMapIcon.iconObject = _iconObject;
            miniMapIcon.iconTransform = _iconObject.transform;
            miniMapIcons.Add(miniMapIcon);
            miniMapIcon.icon.transform.SetParent(iconParentTransform);
        }

        public void RemoveMiniMapIcon(GameObject _iconObject)
        {
            List<MiniMapIcon> newList = new List<MiniMapIcon>();
            for (int i = 0; i < miniMapIcons.Count; i++)
            {
                if (miniMapIcons[i].iconObject == _iconObject)
                {
                    Destroy(miniMapIcons[i].icon);
                }
                else
                {
                    newList.Add(miniMapIcons[i]);
                }
            }
        }

        private void Update()
        {
            SetIconPositions();
        }

        void SetIconPositions()
        {
            foreach (MiniMapIcon mapIcon in miniMapIcons)
            {
                Vector3 position = miniMapCamera.miniMapCamera.WorldToViewportPoint(mapIcon.iconTransform.position);
                RectTransform rt = iconParentTransform.GetComponent<RectTransform>();
                corners = new Vector3[4];
                rt.GetWorldCorners(corners);

                position.x = Mathf.Clamp(position.x * (rt.rect.width * canvasRect.localScale.x) + corners[0].x, corners[0].x, corners[2].x);
                position.y = Mathf.Clamp(position.y * (rt.rect.height * canvasRect.localScale.y) + corners[0].y, corners[0].y, corners[1].y);
                position.z = 0;
                if (miniMapCamera.fixedRotation && mapIcon.isPlayer)
                {
                    Vector3 rotation = Vector3.zero;
                    rotation.z = 360 - mapIcon.iconTransform.rotation.eulerAngles.y;
                    mapIcon.icon.rectTransform.localEulerAngles = rotation;
                    mapIcon.icon.rectTransform.SetAsLastSibling();
                }
                else if (!mapIcon.isPlayer && mapIcon.updateRotation)
                {
                    if (miniMapCamera.fixedRotation)
                    {
                        Vector3 rotation = Vector3.zero;
                        rotation.z = 360 - mapIcon.iconTransform.rotation.eulerAngles.y;
                        mapIcon.icon.rectTransform.localEulerAngles = rotation;
                        mapIcon.icon.rectTransform.SetAsLastSibling();
                    }
                    else
                    {
                        Vector3 rotation = Vector3.zero;
                        rotation.z = 360 + playerTransform.rotation.eulerAngles.y - mapIcon.iconTransform.rotation.eulerAngles.y;
                        mapIcon.icon.rectTransform.localEulerAngles = rotation;
                        mapIcon.icon.rectTransform.SetAsLastSibling();
                    }
                    
                }
                else
                {
                    mapIcon.icon.rectTransform.localEulerAngles = Vector3.zero;
                }
                mapIcon.icon.transform.position = position;
                
            }
        }

        public void SetDistrict(string _districtName)
        {
            districtText.text = _districtName;
        }

        public void SetStreet(string _streetName)
        {
            streetText.text = _streetName;
        }
    }
}