namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using System.Collections.Generic;
    using System;

    public class GameTemplate_PlayerCustomization : MonoBehaviour
    {
        public static GameTemplate_PlayerCustomization Instance = null;
        private ParticleSystem neonLight;
        private Material bodyMaterial;
        private Material rimMaterial;
        private Material windowMaterial;
        private int bodyMaterialIndex;
        private int rimMaterialIndex;
        private int windowMaterialIndex;
        private List<MeshRenderer> rimMeshList;
        private List<MeshRenderer> bodyMeshList;
        private List<MeshRenderer> windowMeshList;

        private void OnEnable()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
            rimMeshList = new List<MeshRenderer>();
            windowMeshList = new List<MeshRenderer>();
            bodyMeshList = new List<MeshRenderer>();
        }

        private void OnDisable()
        {
            Instance = null;
        }

        private void Start()
        {
            UpdateBodyColor();
            UpdateWindowColor();
            UpdateRimColor();
            UpdateNeonLightState();
        }

        #region Customization - Body Paint
        public void RegisterBodyMesh(MeshRenderer _meshRend, int _matIndex)
        {
            bodyMeshList.Add(_meshRend);
            bodyMaterialIndex = _matIndex;
            bodyMaterial = new Material(_meshRend.materials[_matIndex]);
            Material[] mats = _meshRend.sharedMaterials;
            mats[bodyMaterialIndex] = bodyMaterial;
            if (rimMeshList.Count > 0 && _meshRend == rimMeshList[0]) mats[rimMaterialIndex] = rimMaterial;
            if (windowMeshList.Count > 0 && _meshRend == windowMeshList[0]) mats[windowMaterialIndex] = windowMaterial;
            _meshRend.materials = mats;
            UpdateBodyColor();
        }

        public void UpdateBodyColor()
        {
            Color color;
            string bodyColorHex = GameData.GetCarBodyColorHex(PlayerVehicleManager.Instance.CurrentSelection());
            if (String.IsNullOrEmpty(bodyColorHex)) bodyColorHex = PlayerVehicleManager.Instance.DefaultBodyColorHex();
            if (ColorUtility.TryParseHtmlString("#" + bodyColorHex, out color))
            {
                bodyMaterial.color = color;
            }
        }

        public void SetNewBodyColor(string hex)
        {
            if (hex == "RESET")
            {
                UpdateBodyColor();
            }
            else
            {
                Color color;
                string bodyColorHex = hex;
                if (ColorUtility.TryParseHtmlString("#" + bodyColorHex, out color))
                {
                    bodyMaterial.color = color;
                }
            }
        }
        #endregion

        #region Customization - Rim Paint
        public void RegisterRimMesh(MeshRenderer _meshRend, int _matIndex)
        {
            rimMeshList.Add(_meshRend);
            rimMaterialIndex = _matIndex;
            rimMaterial = new Material(_meshRend.materials[_matIndex]);
            Material[] mats = _meshRend.sharedMaterials;
            mats[rimMaterialIndex] = rimMaterial;
            if (bodyMeshList.Count > 0 && _meshRend == bodyMeshList[0]) mats[bodyMaterialIndex] = bodyMaterial;
            if (windowMeshList.Count > 0 && _meshRend == windowMeshList[0]) mats[windowMaterialIndex] = windowMaterial;
            for (int i = 0; i < rimMeshList.Count; i++)
            {
                rimMeshList[i].materials = mats;
            }
            UpdateRimColor();
        }

        public void UpdateRimColor()
        {
            Color color;
            string rimColorHex = GameData.GetCarRimColorHex(PlayerVehicleManager.Instance.CurrentSelection());
            if (String.IsNullOrEmpty(rimColorHex)) rimColorHex = PlayerVehicleManager.Instance.DefaultRimColorHex();
            if (ColorUtility.TryParseHtmlString("#" + rimColorHex, out color))
            {
                rimMaterial.color = color;
            }
        }

        public void SetNewRimColor(string hex)
        {
            if (hex == "RESET")
            {
                UpdateRimColor();
            }
            else
            {
                Color color;
                string rimColorHex = hex;
                if (ColorUtility.TryParseHtmlString("#" + rimColorHex, out color))
                {
                    rimMaterial.color = color;
                }
            }
        }
        #endregion

        #region Customization - Window Color
        public void RegisterWindowMesh(MeshRenderer _meshRend, int _matIndex)
        {
            windowMeshList.Add(_meshRend);
            windowMaterialIndex = _matIndex;
            windowMaterial = new Material(_meshRend.materials[_matIndex]);
            Material[] mats = _meshRend.sharedMaterials;
            mats[windowMaterialIndex] = windowMaterial;
            if (bodyMeshList.Count > 0 && _meshRend == bodyMeshList[0]) mats[bodyMaterialIndex] = bodyMaterial;
            if (rimMeshList.Count > 0 && _meshRend == rimMeshList[0]) mats[rimMaterialIndex] = rimMaterial;
            _meshRend.materials = mats;
            UpdateWindowColor();
        }

        public void UpdateWindowColor()
        {
            Color color;
            string windowColorHex = GameData.GetCarWindowColorHex(PlayerVehicleManager.Instance.CurrentSelection());
            if (String.IsNullOrEmpty(windowColorHex)) windowColorHex = PlayerVehicleManager.Instance.DefaultWindowColorHex();
            if (ColorUtility.TryParseHtmlString("#" + windowColorHex, out color))
            {
                color.a = PlayerVehicleManager.Instance.DefaultWindowAlpha();
                windowMaterial.color = color;
            }
        }

        public void SetNewWindowColor(string hex)
        {
            if (hex == "RESET")
            {
                UpdateWindowColor();
            }
            else
            {
                Color color;
                string windowColorHex = hex;
                if (ColorUtility.TryParseHtmlString("#" + windowColorHex, out color))
                {
                    color.a = PlayerVehicleManager.Instance.DefaultWindowAlpha();
                    windowMaterial.color = color;
                }
            }
        }
        #endregion

        #region Customization - Neon Light
        public void RegisterNeonLight(ParticleSystem _neonLight)
        {
            neonLight = _neonLight;
        }

        public void UpdateNeonLightState()
        {
            if (GameData.GetCarNeonLightState(PlayerVehicleManager.Instance.CurrentSelection()) == "ON")
            {
                neonLight.gameObject.SetActive(true);
                UpdateNeonLightColor();
            }
            else if (GameData.GetCarNeonLightState(PlayerVehicleManager.Instance.CurrentSelection()) == "OFF")
            {
                neonLight.gameObject.SetActive(false);
            }
        }

        public void UpdateNeonLightColor()
        {
            Color color;
            ParticleSystem.MainModule mainModule = neonLight.main;
            string neonLightColorHex = GameData.GetCarNeonLightColorHex(PlayerVehicleManager.Instance.CurrentSelection());
            if (ColorUtility.TryParseHtmlString("#" + neonLightColorHex, out color))
            {
                color.a = 0.023f;
                mainModule.startColor = color;
                neonLight.gameObject.SetActive(false);
                neonLight.gameObject.SetActive(true);
            }
        }

        public void SetNewNeonLightColor(string hex)
        {
            if (hex == "RESET")
            {
                UpdateNeonLightState();
            }
            if (hex == "OFF")
            {
                neonLight.gameObject.SetActive(false);
            }
            else
            {
                Color color;
                ParticleSystem.MainModule mainModule = neonLight.main;
                string neonLightColorHex = hex;
                if (ColorUtility.TryParseHtmlString("#" + neonLightColorHex, out color))
                {
                    color.a = 0.023f;
                    mainModule.startColor = color;
                    neonLight.gameObject.SetActive(false);
                    neonLight.gameObject.SetActive(true);
                }
            }
        }

        public void DisableNeonLight()
        {
            neonLight.gameObject.SetActive(false);
        }
        #endregion
    }
}