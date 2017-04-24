using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Autofac;

namespace Renshaw.Service
{
    public class MissionService:GameService,IMissionService
    {
        public MissionService(ILifetimeScope scope) 
            : base(scope)
        {

        }

        public void TriggerMission(int userId, int eventId, int progress = 1, int preCondition = 0)
        {
            
        }

        public void PutMissionReward(int userId, int missionId)
        {
            
        }
    }
}
