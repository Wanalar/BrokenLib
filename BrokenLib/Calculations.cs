namespace SF2022User01Lib
{
    public class Calculations
    {
        public string[] AvailablePeriods(TimeSpan[] startTimes,
                                         int[] durations,
                                         TimeSpan beginWorkingTime,
                                         TimeSpan endWorkingTime,
                                         int consultationTime)
        {
            if (startTimes == null)
                startTimes = new TimeSpan[0];
            if (durations == null)
                durations = new int[0];
            if (startTimes.Length != durations.Length )
                throw new ArgumentException("Количество в аргументе durations не совпадает с количеством в аргументе startTimes ");

            if (consultationTime < 0)
                throw new ArgumentException("consultationTime должен быть больше 0");

            if (beginWorkingTime > endWorkingTime)
                throw new ArgumentException("Начало работы должно быть меньше окончания работы");

            if (consultationTime > 120)
                throw new ArgumentException("consultationTime должен быть меньше 2 часов");

            if(durations.Any(s=> s <= 0))
                throw new ArgumentException("durations содержит отрицательное или нулевое значение");

            var queue = new Queue<(TimeSpan, int)>();
            for (int i = 0; i < startTimes.Length; i++)
                queue.Enqueue((startTimes[i], durations[i]));

            var consultTimeSpan = TimeSpan.FromMinutes(consultationTime);
            var expectedPeriods = new List<(TimeSpan, TimeSpan)>();
            var start = beginWorkingTime;
            while (start < endWorkingTime)
            {
                var next = start.Add(consultTimeSpan);
                if (queue.Count > 0)
                {
                    var busyTime = queue.Peek().Item1;
                    if (next >= busyTime)
                    {
                        if (start < busyTime && busyTime - start >= consultTimeSpan)
                            expectedPeriods.Add(new(start, busyTime));
                        var time = queue.Dequeue();
                        start = time.Item1.Add(TimeSpan.FromMinutes(time.Item2));
                        continue;
                    }
                }
                if (next > endWorkingTime)
                    break;
                expectedPeriods.Add(new(start, next));
                start = next;
            }


            return expectedPeriods.Select(((TimeSpan, TimeSpan) s) => s.Item1.ToString("hh\\:mm") + " - " + s.Item2.ToString("hh\\:mm")).ToArray();
        }
    }
}