using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace Renshaw.Service
{
    public interface IMissionService
    {
        void TriggerMission(int userId, int eventId, int progress = 1, int preCondition = 0);
        void PutMissionReward(int userId, int missionId);
    }
}
