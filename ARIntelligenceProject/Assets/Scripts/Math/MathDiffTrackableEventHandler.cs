using UnityEngine;
using System.Collections;
using Vuforia;
using System;

public class MathDiffTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    public static string imageName = null;
    #region PRIVATE_MEMBER_VARIABLES

    private TrackableBehaviour mTrackableBehaviour;

    #endregion // PRIVATE_MEMBER_VARIABLES

    #region UNTIY_MONOBEHAVIOUR_METHODS

    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    #endregion // UNTIY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    /// Implementation of the ITrackableEventHandler function called when the
    /// tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackingFound();
        }
        else
        {
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PRIVATE_METHODS

    private void OnTrackingFound()
    {
        imageName = mTrackableBehaviour.name;
        this.GetComponent<ObtainSelfName>().Add(imageName);
        Debug.Log("Trackable " + mTrackableBehaviour.name + " found");
    }


    private void OnTrackingLost()
    {
        this.GetComponent<ObtainSelfName>().Remove();
        Debug.Log("Trackable " + mTrackableBehaviour.name + " lost");
    }

    #endregion // PRIVATE_METHODS
}

