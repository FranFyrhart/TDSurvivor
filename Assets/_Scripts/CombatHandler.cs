using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatHandler : MonoBehaviour
{
    [SerializeField] protected int baseHP;

    protected int currentHP;
    private VisualFeedbackHandler visualFeedbackHandler;

    public virtual void TakeDamage(int damage)
    {
        Debug.Log("Taking damage");
        currentHP -= damage;

        if (currentHP <= 0)
        {
            gameObject.SetActive(false);
            return;
        }

        #region visual feedback 


        
        #endregion
    }

    public int GetCurrentHP()
    {
        return currentHP;
    }
}
