﻿namespace  TurnTheGameOn.ArcadeRacer
{
    public static class CarAIWaypointRouteJunctionConfigurationHelper
    {
        public static void AssignNextWaypointJunction (CarAIWaypointRoute getFrom, CarAIWaypointRoute assignTo)
        {
            for (int i = 0; i < assignTo.waypointDataList.Count; i++)
            {
                int newArraySize = assignTo.waypointDataList [i]._waypoint.junctionPoint.Length + 1;
                System.Array.Resize (ref assignTo.waypointDataList [i]._waypoint.junctionPoint, newArraySize);
                if (i == assignTo.waypointDataList.Count - 1)
                {
                    assignTo.waypointDataList [i]._waypoint.junctionPoint [newArraySize - 1] = getFrom.waypointDataList [0]._waypoint;
                }
                else
                {
                    assignTo.waypointDataList [i]._waypoint.junctionPoint [newArraySize - 1] = getFrom.waypointDataList [i + 1]._waypoint;
                }
            }
        }
    }
}