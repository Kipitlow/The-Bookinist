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

        UIRaycast(screenPosition);

        Raycast3D(screenPosition);
    }

    public void UIRaycast(Vector2 screenPosition)
    {
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
                        continue; // transparent, continuer la v�rification
                }
                else
                {
                    // UI sans shader => bloque
                    return;
                }
            }
        }
    }

    public void Raycast3D(Vector2 screenPosition)
    {
        Ray ray = _cam.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _maxDistance, _hitMask))
        {
            InteractionRunner interactionRunner = hit.collider.GetComponent<InteractionRunner>();

            if (SceneManager.GetActiveScene().name == "BookShopUpdated" || SceneManager.GetActiveScene().name == "LibraryTest")
            {
                BookshopRaycast(hit, interactionRunner);
            }
            else
            {
                BookRaycast(hit, interactionRunner);
            }
        }
    }

    public void BookshopRaycast(RaycastHit hit, InteractionRunner interactionRunner)
    {
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

    public void BookRaycast(RaycastHit hit, InteractionRunner interactionRunner)
    {
        GameObject hitObject = hit.collider.gameObject;
        MoveOnZoom moveOnZoom = hit.collider.GetComponent<MoveOnZoom>();
        InteractionFeedBack interactionFeedBack = hit.collider.GetComponent<InteractionFeedBack>();

        if (moveOnZoom == null) return;

        if (interactionRunner != null)
        {
            CameraMovement cameraMovement = _cam.GetComponent<CameraMovement>();
            int camLayer = cameraMovement.currentIndexLayer;
            int hitLayer = moveOnZoom.GetLayer();

            if (camLayer == hitLayer)
            {
                OnClick?.Invoke(hitObject);

                InteractionContext context = new InteractionContext
                {
                    instigator = null,
                    target = hitObject,
                    isTouchEvent = true,
                };

                interactionRunner.TryExecuteAll(context);
            }
        }
    }
}