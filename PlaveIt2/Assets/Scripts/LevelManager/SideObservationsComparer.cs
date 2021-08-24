using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideObservationsComparer : MonoBehaviour
{
    private SideObservation m_Solution, m_CurrentState;
    private int differences;

    public SideObservationsComparer(SideObservation i_Solution)
    {
        m_Solution = i_Solution;
        countInitialDifferences();
    }

    private void countInitialDifferences()
    {
        differences = 0;

    }
}
