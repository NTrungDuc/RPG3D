using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void OnEnter(BotController bot);
    void OnExecute(BotController bot);
    void OnExit(BotController bot);
}
