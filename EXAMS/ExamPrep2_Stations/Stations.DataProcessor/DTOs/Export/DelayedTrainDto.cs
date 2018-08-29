namespace Stations.DataProcessor.DTOs.Export
{
    using System;

    public class DelayedTrainDto
    {
        public string TrainNumber { get; set; }

        public int DelayedTimes { get; set; }

        public string MaxDelayedTime { get; set; }
    }
}