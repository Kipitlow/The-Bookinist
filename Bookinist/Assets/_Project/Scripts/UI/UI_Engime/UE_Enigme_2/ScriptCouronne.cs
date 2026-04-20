using UnityEngine;

public class ScriptCouronne : MonoBehaviour
{
    /*
        #region Variable
        public GameObject prefableFlowers;
        private bool _typeFleureRed;

        public Sprite RougeFleur;
        public Sprite VioletFleur;

        public GameObject couronne_rouge;
        public GameObject couronne_fusion;
        public GameObject couronne_violet;
        int _redFlowers=0, _purpleFlowers = 0, _numberFlowers=0;
        #endregion
        #region Methods Unity
        public void spawnFlowers(GameObject selfObject)
        {
            //GameObject RR = Instantiate(prefableFlowers, selfObject.transform.position, selfObject.transform.rotation);
            GameObject RR = Instantiate(prefableFlowers, selfObject.transform.position, selfObject.transform.rotation);
            RR.transform.localScale = new Vector3(0.05f, 0.05f, 1f);
            if(_typeFleureRed)
            {
                RR.GetComponent<SpriteRenderer>().sprite = RougeFleur;
            }
            else { RR.GetComponent<SpriteRenderer>().sprite = VioletFleur; }
            gameObject.GetComponent<InteractionRunner>().enabled = false;
        }
        public void addFlower(bool redFleur)
        {
            if(redFleur)
            {
                _typeFleureRed = true;
                _redFlowers += 1;
            }
            else 
            {
                _typeFleureRed = false;
                _purpleFlowers += 1; 
            }
            _numberFlowers += 1;

            Debug.Log($"Fleur Rouge: {_redFlowers}, Fleur Purple: {_purpleFlowers},Fleur Number: {_numberFlowers}, Bool: {_typeFleureRed} ");

            if (_numberFlowers>=5)
            {
                _visiblingCouronne();
            }
        }
        private void _visiblingCouronne()
        {
           //if(_redFlowers>0 && _purpleFlowers == 0) { couronne_rouge.SetActive(true); }
           // if(_redFlowers>0 && _purpleFlowers>0) { couronne_fusion.SetActive(true); }
           // if(_redFlowers==0 && _purpleFlowers > 0) { couronne_violet.SetActive(true); }
            if(_redFlowers>0 && _purpleFlowers == 0) 
            {
                couronne_rouge.SetActive(true);
                //Instantiate(couronne_rouge,);
            }
            if(_redFlowers>0 && _purpleFlowers>0) { couronne_fusion.SetActive(true); }
            if(_redFlowers==0 && _purpleFlowers > 0) { couronne_violet.SetActive(true); }
        }
    #endregion
    */
    #region Variable
    private int FlowerRed = 0;
    private int FlowerViolet = 0;
    public GameObject prefableCrownFlowerRed;
    public GameObject prefableCrownFlowerViolet;
    public GameObject prefableCrownFlowerRV;
    //private InteractionRunner _interactionRunner;
    #endregion
    #region Methode Unity
    public void _findFlowers()
    {
        #region Reset Variable
        FlowerRed = 0;
        FlowerViolet = 0;
        GameObject[] _temporaireItem = GameObject.FindObjectsOfType<GameObject>();
        //_interactionRunner = gameObject.GetComponent<InteractionRunner>();
        #endregion
        #region _searchCloneFlowers
        foreach (GameObject _gameRef in _temporaireItem)
        {
            if (_gameRef.name == "emptyPrefab(Clone)")
            {
                if (_gameRef.GetComponent<SpriteRenderer>().sprite.name == "FLEURS_FICOIDE_1")
                {
                    FlowerRed += 1;
                }
                if (_gameRef.GetComponent<SpriteRenderer>().sprite.name == "FLEURS_FICOIDE_2")
                {
                    FlowerViolet += 1;
                }
            }
        }
        #endregion
        #region ConditionFlowers
        if(FlowerRed + FlowerViolet >= 5)
        {
            Debug.Log("<color=green> <Sucess Critique> </color>" + $"<Color=white> FlowerRed:{FlowerRed} + FlowerViolet:{FlowerViolet}</color>");
            string _sateFlowers;
            if (FlowerRed == 0 && FlowerViolet >= 5) _sateFlowers = "crownFlowerViolet";
            else if (FlowerRed >= 5 && FlowerViolet == 0) _sateFlowers = "crownFlowerRed";
            else _sateFlowers = "crownFlowerMultiColor";
            GameObject _itemInstantiate;
            switch (_sateFlowers)
            {
                case "crownFlowerRed":
                    _itemInstantiate = Instantiate(prefableCrownFlowerViolet, this.transform.position, this.transform.rotation);
                    break;
                case "crownFlowerViolet":
                    _itemInstantiate = Instantiate(prefableCrownFlowerRed, this.transform.position, this.transform.rotation);
                    break;
                case "crownFlowerMultiColor":
                    _itemInstantiate = Instantiate(prefableCrownFlowerRV, this.transform.position, this.transform.rotation);
                    break;
                    //_itemInstantiate.transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
                    _itemInstantiate.transform.SetParent(this.transform);
            }
            
            #endregion
        }
    }
    #endregion
}