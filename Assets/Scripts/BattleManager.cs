using System.Collections;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject postmanPref;
    [SerializeField] private GameObject enemyPref;
    [Space(10)]
    [SerializeField] private Transform postmanBattleGroundTransform;
    [SerializeField] private Transform enemyBattleGroundTransform;

    private readonly WaitForSeconds _turnDelay = new(1f);

    private static FighterBase _postman;
    private static FighterBase _enemy;

    public void SetupBattle()
    {
        _postman = Instantiate(postmanPref, postmanBattleGroundTransform).GetComponent<FighterBase>();
        _enemy = Instantiate(enemyPref, enemyBattleGroundTransform).GetComponent<FighterBase>();
    }

    public void Insult(FighterBase affronter, int insultDamage, string insultText)
    {
        if (affronter == _postman)
        {
            _postman.GetOffend(insultDamage);
            StartCoroutine(DelayTurn(_postman));
        }
        else
        {
            _enemy.GetOffend(insultDamage);
            StartCoroutine(DelayTurn(_enemy));
        }
    }

    public IEnumerator DelayTurn(FighterBase fighter)
    {
        yield return _turnDelay;

        fighter.TakeTurn();
    }
}
