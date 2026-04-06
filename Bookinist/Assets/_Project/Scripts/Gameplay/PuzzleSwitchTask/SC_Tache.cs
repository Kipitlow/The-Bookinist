using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SC_Tache : MonoBehaviour
{
    #region Types

    [Serializable]
    public class ListElementTache
    {
        [SerializeField] public string nomMission;
        [SerializeField] public string tache;
        [SerializeField] public bool tacheTerminee;
    }

    [Serializable]
    public class UiCacheLayeur
    {
        [Header("Affiche UI & Objet dans un layeur")]
        public int layerAfficheMission;
        public List<GameObject> canvaUI;
    }

    #endregion

    #region Variables

    [Header("Variable Utiliser pour le Chronometre")]
    public TextMeshProUGUI textChronom;
    private bool _lanceCoroutine;
    public int totalSeconds;

    [Header("Autre")]
    public Camera cmPlayer;
    public CameraMovement cm;
    public List<UiCacheLayeur> uiCacheLayeur = new();
    private int _layeurActuelleDuJoueur;

    [Header("UI_Enigme_01")]
    public GameObject balance;
    public GameObject canvaMarchant;

    [Header("Prefable")]
    public GameObject prefableTache;
    public GameObject prefableCanvaGameOver;
    public Transform targetParentPrefable;
    public List<GameObject> listTemporairTache = new();

    [Header("Mission")]
    public List<ListElementTache> listeMission = new();

    #endregion

    #region Unity Methods

    private void Start()
    {
        if (cmPlayer == null)
            cmPlayer = GameObject.Find("CameraManager")?.GetComponent<Camera>();

        StartCoroutine(Chronometre());
        ChangeTacheList();
    }

    private void Update()
    {
        if (cmPlayer == null || cm == null) return;

        int camLayer = cm.currentIndexByLayer;
        int playerLayerFromZ = (int)Mathf.Round(cmPlayer.transform.position.z) + 1;

        if (_layeurActuelleDuJoueur != playerLayerFromZ && _layeurActuelleDuJoueur != camLayer)
        {
            _layeurActuelleDuJoueur = camLayer;
            Debug.Log("Layer change detected");

            for (int i = 0; i < uiCacheLayeur.Count; i++)
            {
                bool shouldShow = i == _layeurActuelleDuJoueur;
                foreach (GameObject cacheObjet in uiCacheLayeur[i].canvaUI)
                {
                    if (cacheObjet != null)
                        cacheObjet.SetActive(shouldShow);
                }
            }
        }
    }

    #endregion

    #region Methods

    public void ChangeTacheList()
    {
        if (prefableTache == null || targetParentPrefable == null) return;

        // Supprime les prefabs temporaires existants
        foreach (GameObject obj in listTemporairTache)
        {
            if (obj != null) Destroy(obj);
        }
        listTemporairTache.Clear();

        // Affiche toutes les missions terminées puis une seule mission non terminée
        for (int i = 0; i < listeMission.Count; i++)
        {
            SpawnPrefableTache(i);
            if (!listeMission[i].tacheTerminee)
                return;
        }
    }

    private void SpawnPrefableTache(int index)
    {
        GameObject newObject = Instantiate(prefableTache, targetParentPrefable);
        Vector3 pos = newObject.transform.position;
        pos.y = pos.y - 100 * index;
        newObject.transform.position = pos;

        SC_Prefable_Tache prefableScriptTache = newObject.GetComponentInChildren<SC_Prefable_Tache>();
        if (prefableScriptTache != null)
        {
            if (listeMission[index].tacheTerminee)
            {
                if (prefableScriptTache.textObjectif != null)
                {
                    prefableScriptTache.textObjectif.color = Color.red;
                    prefableScriptTache.textObjectif.text = $"<s>{listeMission[index].tache}</s>";
                }
            }
            else
            {
                if (prefableScriptTache.textObjectif != null)
                {
                    prefableScriptTache.textObjectif.color = Color.black;
                    prefableScriptTache.textObjectif.text = listeMission[index].tache;
                }
            }
        }

        listTemporairTache.Add(newObject);
    }

    public void TerminerTache(string nomMission)
    {
        for (int i = 0; i < listeMission.Count; i++)
        {
            if (!listeMission[i].tacheTerminee)
            {
                if (listeMission[i].nomMission == nomMission)
                {
                    listeMission[i].tacheTerminee = true;
                    ChangeTacheList();
                }
                else
                {
                    Debug.LogWarning($"La mission {listeMission[i].nomMission} est déjà terminée ou ne correspond pas");
                }
            }
        }
    }

    private void SpawnCanvaGameOver()
    {
        SC_UI_GameOver pui = prefableCanvaGameOver?.GetComponent<SC_UI_GameOver>();
        Transform goCanva = GameObject.Find("Canvas")?.transform;
        if (pui != null && goCanva != null)
        {
            GameObject rr = Instantiate(prefableCanvaGameOver, transform.position, transform.rotation);
            rr.transform.SetParent(goCanva, false);
            rr.transform.localScale = new Vector2(0.5f, 0.5f);

            RectTransform rt = rr.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(transform.position.x / 2f, transform.position.y / 2f);
        }
    }

    public void AfficheMarchant(GameObject self)
    {
        if (self != null) self.SetActive(false);

        if (balance != null)
        {
            balance.SetActive(!balance.activeSelf);
            Debug.Log($"Trouver Balance: {balance.activeSelf}");
        }
        else
        {
            Debug.LogWarning("Erreur du system Balance = null");
        }

        if (canvaMarchant != null)
        {
            canvaMarchant.SetActive(!canvaMarchant.activeSelf);
            Debug.Log($"Trouver CanvaMarchant: {canvaMarchant.activeSelf}");
        }
        else
        {
            Debug.LogWarning("Erreur du system CanvaMarchant = null");
        }
    }

    private IEnumerator Chronometre()
    {
        while (totalSeconds > 0)
        {
            totalSeconds -= 1;
            if (textChronom != null)
                textChronom.text = $"{totalSeconds / 60}:{totalSeconds % 60}";

            yield return new WaitForSeconds(1);
            _lanceCoroutine = true;
        }

        totalSeconds = 0;
        if (textChronom != null)
            textChronom.text = $"{totalSeconds / 60}:{totalSeconds % 60}";

        _lanceCoroutine = false;
        SpawnCanvaGameOver();
        yield break;
    }

    public void SetChronom(bool shouldContinue)
    {
        if (shouldContinue)
        {
            if (!_lanceCoroutine)
            {
                StartCoroutine(Chronometre());
                _lanceCoroutine = true;
            }
        }
        else
        {
            if (_lanceCoroutine)
            {
                StopCoroutine(Chronometre());
                _lanceCoroutine = false;
            }
        }
    }

    #endregion
}
