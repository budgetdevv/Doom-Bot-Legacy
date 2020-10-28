using IdGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snowflake
{
    public class SnowflakeEngine
    {
        private IdGenerator Generator { get; }

        private DateTime Epoch { get; }

        public SnowflakeEngine()
        {
            Epoch = new DateTime(2020, 7, 30, 0, 0, 0, DateTimeKind.Utc);

            //This is written @ 11:58 PM, 30/7/2020

            // Create an ID with 31 Bit for TimeStamp, 1 Bits for GeneratorID and 31 Bits for Sequence

            var IDStructure = new IdStructure(31, 1, 31);

            var Options = new IdGeneratorOptions(IDStructure, new DefaultTimeSource(Epoch));

            Generator = new IdGenerator(1, Options);
        }

        public long Gen()
        {
            return Generator.CreateId();
        }

        public DateTime SnowflakeToTime(long Snowflake)
        {
            long MS = Snowflake >> 31;

            return Epoch.AddMilliseconds(MS);
        }
    }
}
