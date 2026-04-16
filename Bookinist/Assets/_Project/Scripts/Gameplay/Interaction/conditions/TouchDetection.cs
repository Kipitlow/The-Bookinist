using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TouchDetection : MonoBehaviour
{
    Camera _cam;
    [SerializeField] private float _maxDistance = 100f;
    [SerializeField] private LayerMask _hitMask = ~0;

    [SerializeField] private GraphicRaycaster _uiRaycaster;
    [SerializeField] private EventSystem _eventSystem;


    public event Action<GameObject> OnClick;

    private void Awake()
    {
        _cam = Camera.main;
    }

    public void OnTouch(Vector2 screenPosition)
    {
        if (_uiRaycaster == null || _eventSystem == null || _cam == null)
            return;

        // --- 1. RAYCAST UI ---
        PointerEventData pointerData = new PointerEventData(_eventSystem)
        {
            position = screenPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        _uiRaycaster.Raycast(pointerData, results);

        if (results.Count > 0)
        {
            foreach (var result in results)
            {
                // CanvasGroup qui bloque visuellement
                CanvasGroup cg = result.gameObject.GetComponentInParent<CanvasGroup>();
                if (cg != null && cg.blocksRaycasts && cg.alpha > 0.01f)
                {
                    return; // UI bloque la touche
                }

                var shaderRaycast = result.gameObject.GetComponent<ShaderBasedRaycast>();

                if (shaderRaycast != null)
                {
                    bool isValid = shaderRaycast.IsRaycastLocationValid(screenPosition, _cam);

                    // CORRECTION : si valide (opaque) => bloquer la propagation
                    if (isValid)
                        return;
                    else
                        continue; // transparent, continuer la vérification
                }
                else
                {
                    // UI sans shader => bloque
                    return;
                }
            }
        }

        // --- 2. RAYCAST 3D ---
        Ray ray = _cam.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _maxDistance, _hitMask))
        {
            InteractionRunner interactionRunner = hit.collider.GetComponent<InteractionRunner>();
            MoveOnZoom moveOnZoom = hit.collider.GetComponent<MoveOnZoom>();

            if (moveOnZoom == null && interactionRunner == null) return;

            int camLayer = _cam.GetComponent<CameraMovement>().currentIndexLayer;
            int hitlayer = moveOnZoom.GetLayer();

            if (camLayer != hitlayer) return;

            OnClick?.Invoke(hit.collider.gameObject);

            InteractionContext context = new InteractionContext
            {
                instigator = null,
                target = hit.collider.gameObject,
                isTouchEvent = true,
            };

            if (interactionRunner != null)
                interactionRunner.TryExecuteAll(context);
        }
    }
}
