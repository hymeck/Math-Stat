﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab1
{
    public class SampleHandler
    {
        static int _tableRoundDigits = 6;
        static int _sampleRoundDigits = 1;

        public Func<double, double> Function { get; private set; }

        Dictionary<double, int> _sample;
        public Dictionary<double, int> Sample => new Dictionary<double, int>(_sample);

        double[] _samplePoints;
        public double[] SamplePoints
        {
            get
            {
                double[] samplePoints = new double[_samplePoints.Length];
                _samplePoints.CopyTo(samplePoints, 0);
                return samplePoints;
            }

            private set
            {
                double[] samplePoints = value.Select(x => Math.Round(Function(x), _sampleRoundDigits)).ToArray();
                Array.Sort(samplePoints);
                _samplePoints = samplePoints;
            }
        }

        void SetSample()
        {
            int volume = _samplePoints.Length;
            Dictionary<double, int> sample = new Dictionary<double, int>(volume);

            for (int i = 0; i < volume; i++)
            {
                double samplePoint = _samplePoints[i];
                if (!sample.TryGetValue(samplePoint, out int count))
                    sample[samplePoint] = 1;
                else
                    sample[samplePoint]++;
            }

            _sample = sample;
        }

        public SampleHandler(double[] arguments, Func<double, double> function)
        {
            Function = function;
            SamplePoints = arguments;
            SetSample();
        }

        public object[,] ToTableRows()
        {
            int volume = _samplePoints.Length,
                size = _sample.Count;

            object[,] rows = new object[size + 1, 4];

            int[] counts = _sample.Values.ToArray();
            double[] points = _sample.Keys.ToArray();
            for (int i = 0; i < size; i++)
            {
                double count = counts[i],
                       relativeFrequency = Math.Round(count / volume, _tableRoundDigits),
                       accumulatedFrequency = relativeFrequency;

                if (i != 0)
                    accumulatedFrequency += Convert.ToDouble(rows[i - 1, 3]);

                rows[i, 0] = points[i];
                rows[i, 1] = count;
                rows[i, 2] = relativeFrequency;
                rows[i, 3] = accumulatedFrequency;
            }

            rows[size - 1, 3] = Math.Round(Convert.ToDouble(rows[size - 1, 3]));

            rows[size, 0] = "SUM:";
            rows[size, 1] = volume;
            rows[size, 2] = "1";
            rows[size, 3] = " ";

            return rows;
        }
    }
}
