using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class xrGrabInteractableTwoAttach : XRGrabInteractable
{
    public Transform leftAttachTransfrom;
    public Transform rightAttachTransfrom;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if(args.interactableObject.transform.CompareTag("Left Hand"))
        {
            attachTransform = leftAttachTransfrom;
        }
        else if(args.interactableObject.transform.CompareTag("Right Hand"))
        {
            attachTransform = rightAttachTransfrom;
        }
        base.OnSelectEntered(args);
    }
}
