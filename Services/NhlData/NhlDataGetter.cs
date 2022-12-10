using Entities.DbModels;
using Services.RequestMaker;
using Services.NhlData.Mappers;
using Microsoft.Extensions.Logging;
using System;
using Entities.Models;

namespace Services.NhlData
{
    public class NhlDataGetter
    {
        public INhlGameGetter GameDataGetter;
        public INhlPlayerGetter PlayerDataGetter;
        public INhlScheduleGetter ScheduleDataGetter;
        public NhlDataGetter(INhlGameGetter gameGetter, INhlPlayerGetter playerGetter, INhlScheduleGetter scheduleGetter)
        {
            GameDataGetter = gameGetter;
            PlayerDataGetter = playerGetter;
            ScheduleDataGetter = scheduleGetter;
        }
        /// <summary>
        /// Builds a game id to the nhl api standard "year""gameType"""gameId"
        /// (ex. 2022020001    year=2022 gameType=02 (02 is regular season) gameId=0001)
        /// </summary>
        /// <param name="seasonstartYear">Year to use in id</param>
        /// <param name="gameNumber">Game number to use in id</param>
        /// <returns>The internal game id</returns>
        public static int GetGameIdFrom(int seasonstartYear, int gameNumber)
        {
            return (seasonstartYear * 1000000) + 20000 + gameNumber;
        }
        /// <summary>
        /// Gets the full season id used by the Nhl Api
        /// </summary>
        /// <param name="seasonStartYear"></param>
        /// <returns></returns>
        public static int GetFullSeasonId(int seasonStartYear)
        {
            int nextYear = seasonStartYear + 1;
            return (seasonStartYear * 10000) + nextYear;
        }
    }
}
