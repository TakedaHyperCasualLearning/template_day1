using System.Collections;
using System.Collections.Generic;
using Donuts;
using UnityEngine;
using System;

namespace Donuts
{
    public partial class GameStat
    {
        public PlayerGroup playerGroup;
    }

    public class PlayerGroup : ComponentGroup<PlayerGroup,
    CharacterBaseComponent,
    CharacterMoveComponent,
    PlayerInputComponent>
    { }

    public class GameRuleSystem : AGameSystem
    {

        public override void SetupEvents()
        {
            gameEvent.onFirstInitialize += Initialize;
        }

        public void Initialize()
        {
            entityManager.Foreach<PlayerGroup>(SetupPlayer);
        }

        public void SetupPlayer(PlayerGroup group)
        {
            if (group.data1 == null) return;
            gameStat.playerGroup = group;
        }
    }
}

