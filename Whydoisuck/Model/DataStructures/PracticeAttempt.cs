using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ydis.Model.DataStructures
{
    public class PracticeAttempt
    {
        /// <summary>
        /// The attempt number
        /// </summary>
        [JsonProperty(PropertyName = "Number")] public int Number { get; set; }
        /// <summary>
        /// The percent the player started at
        /// </summary>
        [JsonProperty(PropertyName = "StartPercent")] public float StartPercent { get; set; }
        /// <summary>
        /// The percent the player died at
        /// </summary>
        [JsonProperty(PropertyName = "EndPercent")] public float EndPercent { get; set; }

        public PracticeAttempt(int number, float startPercent)
        {
            Number = number;
            StartPercent = startPercent;
        }
    }
}
