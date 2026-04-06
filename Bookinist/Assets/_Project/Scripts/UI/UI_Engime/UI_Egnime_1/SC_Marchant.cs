using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Composant de l'énigme marchand (UI / spawn poids / contrôle boutons).
/// </summary>
public class SC_Marchant : MonoBehaviour
{
    #region Variables

    [Header("Gestion UI")]
    public GameObject prefablePoids;
    private SC_Tache _scriptTache;
    private GameObject _eMarchant;
    private GameObject _objectBalance;
    private GameObject _iconeMarchant;
    private Button _bSpawnMarchant;
    private bool _iconeApparition;

    [Header("Autre")]
    public GameObject[] buttonHidden;
    private GameObject _poids;
    private GameObject _poids2;

    private string _nameTacheSelf;

    #endregion

    #region Unity Methods

    private void Start()
    {
        _iconeMarchant = GameObject.Find("Icone_PNJ_Marchant");
        _eMarchant = GameObject.Find("E_Marchant");
        _objectBalance = GameObject.Find("Balance2D");

        _scriptTache = GameObject.Find("Canvas").GetComponent<SC_Tache>();
        _bSpawnMarchant = GameObject.Find("B_Spawn_Marchant").GetComponent<Button>();

        _iconeApparition = true;
        ChangeUI();

        foreach (GameObject aa in buttonHidden)
        {
            if (aa.name == "B_balance")
                aa.SetActive(true);
            else
                aa.SetActive(false);
        }
        _nameTacheSelf = "Egnigme_01";
    }

    #endregion

    #region Methods

    public void CacheBalance(GameObject self)
    {
        switch (self.name)
        {
            case "B_balance":
                CacheCetteObject(self);
                SpawnerPoidsBalance(0);
                break;
            case "B_Reset":
                SpawnerPoidsBalance(0);
                foreach (GameObject aa in buttonHidden)
                {
                    if (aa.name == "B_balance") aa.SetActive(true);
                    else aa.SetActive(false);
                }
                break;
            case "B_Object_1":
                CacheCetteObject(self);
                SpawnerPoidsBalance(1);
                break;
            case "B_Object_2":
                CacheCetteObject(self);
                Invoke(nameof(ChangeUI), 2);
                SpawnerPoidsBalance(2);
                break;
            case "B_Object_3":
                CacheCetteObject(self);
                SpawnerPoidsBalance(3);
                break;
            case "B_Object_4":
                CacheCetteObject(self);
                SpawnerPoidsBalance(4);
                break;
            case "B_Object_5":
                CacheCetteObject(self);
                SpawnerPoidsBalance(5);
                break;
        }
    }

    public void SpawnerPoidsBalance(int mass)
    {
        if (_poids != null)
        {
            Destroy(_poids);
            _poids = null;
        }
        if (_poids2 != null)
        {
            Destroy(_poids2);
            _poids2 = null;
        }

        Transform ee = GameObject.Find("Target_spawn_Poids_R").transform;
        _poids = Instantiate(prefablePoids, ee);

        ee = GameObject.Find("Target_spawn_Poids_L").transform;
        _poids2 = Instantiate(prefablePoids, ee);
        _poids2.GetComponent<Rigidbody2D>().mass = 100;

        switch (mass)
        {
            case 0:
                if (_poids != null)
                {
                    Destroy(_poids);
                    _poids = null;
                }
                break;
            case 1:
                _poids.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                _poids.GetComponent<Rigidbody2D>().mass = 0;
                break;
            case 2:
                _poids.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                _poids.GetComponent<Rigidbody2D>().mass = 100;
                break;
            case 3:
                _poids.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                _poids.GetComponent<Rigidbody2D>().mass = 25;
                break;
            case 4:
                _poids.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                _poids.GetComponent<Rigidbody2D>().mass = 230;
                break;
            case 5:
                _poids.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                _poids.GetComponent<Rigidbody2D>().mass = 175;
                break;
        }
    }

    private void CacheCetteObject(GameObject ee)
    {
        foreach (GameObject aa in buttonHidden)
        {
            if (aa.name != "B_balance") aa.SetActive(true);
        }
        ee.SetActive(false);
    }

    public void ChangeUI()
    {
        if (_iconeApparition)
        {
            _iconeMarchant.SetActive(true);
            _eMarchant.SetActive(false);
            _objectBalance.SetActive(false);
            _iconeApparition = false;
        }
        else
        {
            _iconeMarchant.SetActive(false);
            _eMarchant.SetActive(true);
            _objectBalance.SetActive(true);
            _iconeApparition = true;

            if (_bSpawnMarchant != null)
            {
                _bSpawnMarchant.onClick.RemoveListener(ChangeUI);
                _bSpawnMarchant.gameObject.SetActive(false);
            }
        }
    }

    public void RemoveListenerFunction(UnityEngine.Events.UnityAction call)
    {
        if (_bSpawnMarchant != null)
        {
            _bSpawnMarchant.onClick.RemoveListener(call);
        }
    }

    #endregion
}
