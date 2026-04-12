using UnityEngine;

public class VRRR
{
    public bool fleurRouge;
    public Transform tr;
}

public class ScriptCouronne : MonoBehaviour
{
    public GameObject prefableFlowers;
    private bool _typeFleureRed;

    public Sprite RougeFleur;
    public Sprite VioletFleur;
 
    public GameObject couronne_rouge;
    public GameObject couronne_fusion;
    public GameObject couronne_violet;
    int _redFlowers=0, _purpleFlowers = 0, _numberFlowers=0;

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
        /*if(_redFlowers>0 && _purpleFlowers == 0) { couronne_rouge.SetActive(true); }
        if(_redFlowers>0 && _purpleFlowers>0) { couronne_fusion.SetActive(true); }
        if(_redFlowers==0 && _purpleFlowers > 0) { couronne_violet.SetActive(true); }*/
        if(_redFlowers>0 && _purpleFlowers == 0) 
        {
            couronne_rouge.SetActive(true);
            //Instantiate(couronne_rouge,);
        }
        if(_redFlowers>0 && _purpleFlowers>0) { couronne_fusion.SetActive(true); }
        if(_redFlowers==0 && _purpleFlowers > 0) { couronne_violet.SetActive(true); }
    }
}
