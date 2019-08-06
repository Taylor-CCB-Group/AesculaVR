using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManipulatable
{
    void OnTransformationStarted();
    void OnTransformation();
    void OnTransformationEnded();

}
