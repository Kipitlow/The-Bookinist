using UnityEngine;

/// <summary>
/// Contrôle de la balance pour l'énigme (spawn de poids, fin de tâche).
/// </summary>
public class ScriptBalance : MonoBehaviour
{
    #region Variables

    public GameObject prefablePoids;
    public GameObject[] buttonHidden;
    private GameObject _poids;
    private GameObject _poids2;
    private bool _targetOne;

    #endregion

    #region Unity Methods

    private void Start()
    {
        _targetOne = true;
    }

    #endregion

    #region Methods

    public void FinishTask()
    {
        Debug.Log("finish task");
        Invoke(nameof(CacheSelf), 2.0f);
    }

    private void CacheSelf()
    {
        gameObject.SetActive(false);
    }

    public void SpawnerPoidsBalance(int mass)
    {
        Transform ee = GameObject.Find("Target_spawn_Poids_R").transform;
        _poids = Instantiate(prefablePoids, ee);

        ee = GameObject.Find("Target_spawn_Poids_L").transform;
        if (_targetOne == true)
        {
            _targetOne = false;
            _poids2 = Instantiate(prefablePoids, ee);
            _poids2.GetComponent<Rigidbody2D>().mass = 100;
        }

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
                _poids.transform.localScale = new Vector3(1f, 1f, 1f);
                _poids.GetComponent<Rigidbody2D>().mass = 175;
                break;
        }
    }

    #endregion
}
