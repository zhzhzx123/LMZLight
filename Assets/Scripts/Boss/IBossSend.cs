using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossSend
{
    void SwitchState(int s1, int s2);

    void Hurt(float hurt);

    void DestroyThis();

}
