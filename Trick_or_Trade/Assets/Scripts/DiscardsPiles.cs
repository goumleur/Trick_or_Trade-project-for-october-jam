using UnityEngine;

public class DiscardsPiles : MonoBehaviour
{
    protected Vector3 oldScale;
    protected Vector3 oldPosition;
    protected Vector3 oldAngle;
    protected GameObject main;
    public GameObject carteASauver;
    virtual public void CarteASauver(GameObject carte) { }
    virtual public void discardCard(GameObject carte) { }
    virtual public void GetDicardCard() { }
    virtual public void PutDiscardCard() { }
    virtual public void PutDiscardCardSteal() {}
}
