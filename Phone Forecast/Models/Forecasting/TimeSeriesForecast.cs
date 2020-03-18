using Phone_Forecast.Models.DbContexts;
using Phone_Forecast.Models.Enums;
using Phone_Forecast.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Phone_Forecast.Models.Forecasting
{
    public class TimeSeriesForecast
    {
        private List<Observation> m_observations;
        private List<Transaction> m_transactions;
        private PhoneModel m_selectedPhone;
        //   private List<ForecastResult> m_forecastResults = new List<ForecastResult>();
        private bool IsDatasetForecastable = false;

        public TimeSeriesForecast(List<Transaction> transactions, PhoneModel model)
        {
            m_selectedPhone = model;
            List<Transaction> brandSpecificTransactions = transactions.Where(x => x.Phone == model).ToList();
            m_transactions = brandSpecificTransactions;

            List<Observation> observations = new List<Observation>();
            foreach (Transaction t in brandSpecificTransactions)
            {
                observations.Add(new Observation(t.PurchaseDate, t.Price));
            }
            m_observations = observations;

            ForecastExistingObservations();
        }

        public bool IsForecastable()
        {
            return IsDatasetForecastable;
        }

        public List<ForecastResult> GenerateFutureForecast(int months = 12, bool fillGaps = true)
        {
            if (fillGaps == true)
            {
                DateTime today = DateTime.Today;
                DateTime lastDate = m_observations.Max(x => x.Date);
                DateTimeSpan dateTimeSpan = DateTimeSpan.CompareDates(today, lastDate);
                int difference = (dateTimeSpan.Years * 12) + dateTimeSpan.Months;
                months += difference;
            }

            if (IsDatasetForecastable)
            {
                for (int i = 0; i < months; i++)
                {
                    // Adds new observation with date which is 1 month greater than the last object in m_observations;
                    m_observations.Add(new Observation(m_observations[m_observations.Count - 1].Date.AddMonths(1)));
                }

                for (int i = 0; i < m_observations.Count; i++)
                {
                    if (m_observations[i].Seasonality == null)
                    {
                        m_observations[i].Seasonality = Algorithm.CalculateSeasonality(m_observations, i);
                        m_observations[i].Trend = Algorithm.CalculateTrend(m_observations, i);
                        m_observations[i].Forecast = Algorithm.CalculateForecast(m_observations, i);
                    }
                }

                return m_observations.Select(x => new ForecastResult(x.Date, x.Forecast.Value)).ToList();
            }
            else
            {
                return null;
            }
        }

        public void print()
        {
            foreach (Observation o in m_observations)
            {
                System.Diagnostics.Debug.WriteLine($"ITEM: {o.Date} - {o.Forecast}");
            }
        }

        private void ForecastExistingObservations()
        {
            /* 
             * Preprocessing order is important. DO NOT CHANGE!
             * 
             * 1. If there are multiples transactions in the same month, remove them and add their average
             * 2. If there are any gaps where a month is missing, then average out the previous and next transaction
             * 3. Check if enough transactions exist.
             * 
             */
            Algorithm.PreProcessing.RemoveDuplicates(ref m_observations, m_selectedPhone);
            Algorithm.PreProcessing.FillGaps(ref m_observations);
            bool hasEnoughTransactions = Algorithm.PreProcessing.HasEnoughTransactions(m_observations, m_selectedPhone);

            if (hasEnoughTransactions)
            {
                Algorithm.MovingAverage(ref m_observations);
                Algorithm.CenteredMovingAverage(ref m_observations);
                Algorithm.SeasonalIrregularity(ref m_observations);
                Algorithm.Seasonality(ref m_observations);
                Algorithm.Deseasonalized(ref m_observations);
                Algorithm.Trend(ref m_observations);
                Algorithm.Forecast(ref m_observations);
                this.IsDatasetForecastable = true;
            }
        }

        private class Algorithm
        {
            #region Preprocessing
            public class PreProcessing
            {
                public static bool HasEnoughTransactions(List<Observation> observations, PhoneModel? phone)
                {
                    if (phone == null)
                    {
                        return false;
                    }

                    if (observations.Count <= 24 || observations == null)
                    {
                        return false;
                    }

                    int monthDifference = 0;
                    DateTime? earliestDate = null;
                    DateTime? latestDate = null;


                    if (observations.Count > 12)
                    {
                        earliestDate = observations.Select(x => x.Date).Min();
                        latestDate = observations.Select(x => x.Date).Max();
                        DateTimeSpan datetimespan = DateTimeSpan.CompareDates(earliestDate.Value, latestDate.Value);
                        monthDifference = datetimespan.Years * 12 + datetimespan.Months;
                    }
                    else
                    {
                        return false;
                    }

                    // At least two years of data must be present to compute forecast.
                    if (monthDifference < 24)
                    {
                        return false;
                    }
                    // Checks for gaps. If the recorded transactions are less than 90% 
                    // of the month difference, then the forecast would not be reliable.
                    if (observations.Count < ((monthDifference / 100) * 90))
                    {
                        return false;
                    }

                    return true;
                }

                public static void RemoveDuplicates(ref List<Observation> observations, PhoneModel? phone)
                {
                    if (phone == null)
                    {
                        throw new Exception("No phone is specified.");
                    }

                    DateTime? earliestDate = null;
                    DateTime? latestDate = null;

                    if (observations.Count > 12)
                    {
                        earliestDate = observations.Min(x => x.Date);
                        latestDate = observations.Max(x => x.Date);
                        if (earliestDate == latestDate || earliestDate > latestDate)
                        {
                            throw new Exception("Transaction datetime mismatch.");
                        }

                        // Itterates through all transactions, month by month.
                        for (DateTime date = earliestDate.Value; date < latestDate; date = date.AddMonths(1))
                        {
                            // Number of transactions for current month
                            var monthlyTransactions = observations.Where(x => x.Date.Month == date.Month && x.Date.Year == date.Year).ToList();

                            // Merge all transactions into a single one.
                            if (monthlyTransactions.Count() > 1)
                            {
                                DateTime currentDate = new DateTime(date.Year, date.Month, 1);

                                var monthlyAverageValue = monthlyTransactions.Average(x => x.Value);

                                if (monthlyAverageValue == null)
                                {
                                    throw new Exception("Could not calculate monthly average price.");
                                }

                                // Removes all specific monthly transactions
                                observations.RemoveAll(x => x.Date.Month == date.Month && x.Date.Year == date.Year);
                                // Add a transaction that is the average of all deleted ones
                                observations.Add(new Observation(currentDate, monthlyAverageValue));
                            }
                        }
                    }

                    // Sort all transactions from earliest to latest.
                    observations.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
                }

                public static void FillGaps(ref List<Observation> observations)
                {
                    DateTime? earliestDate = null;
                    DateTime? latestDate = null;

                    if (observations.Count > 12)
                    {
                        earliestDate = observations.Min(x => x.Date);
                        latestDate = observations.Max(x => x.Date);

                        if (earliestDate == latestDate || earliestDate > latestDate)
                        {
                            throw new Exception("Transaction datetime mismatch.");
                        }

                        // Preprocessing already checks if all items in List<Observation> are of the same brand
                        // No need to double check here.

                        for (DateTime date = earliestDate.Value; date < latestDate; date = date.AddMonths(1))
                        {
                            if (ContainsDate(observations, date) == false)
                            {
                                DateTime currentDate = new DateTime(date.Year, date.Month, 1);
                                observations.Add(new Observation(currentDate, CalculateSuggestedPrice(observations, currentDate)));
                                System.Diagnostics.Debug.WriteLine($"Creating new observation: {currentDate} -- {CalculateSuggestedPrice(observations, currentDate)}");
                            }
                        }
                    }

                    // Sorts all transactions in ascending order by DateTime
                    observations.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
                }

                private static double CalculateSuggestedPrice(List<Observation> observations, DateTime date)
                {
                    Observation previousTransaction;
                    Observation nextTransaction;

                    try
                    {
                        previousTransaction = observations
                                              .Where(i => i.Date < date)
                                              .OrderByDescending(i => i.Date)
                                              .FirstOrDefault();
                    }
                    catch
                    {
                        throw new Exception("Could not find previous transcation.");
                    }

                    try
                    {
                        nextTransaction = observations
                                          .Where(i => i.Date > date)
                                          .OrderByDescending(i => i.Date)
                                          .LastOrDefault();
                    }
                    catch
                    {
                        throw new Exception("Could not find next transaction.");
                    }

                    DateTime dateBefore = previousTransaction.Date;
                    DateTime dateAfter = nextTransaction.Date;

                    if (dateBefore == dateAfter || dateBefore > dateAfter)
                    {
                        throw new Exception("Datetime mismatch");
                    }

                    DateTimeSpan dateTimeSpan = DateTimeSpan.CompareDates(dateBefore, dateAfter);
                    int monthDifference = (dateTimeSpan.Years * 12 + dateTimeSpan.Months);

                    if (!previousTransaction.Value.HasValue || !nextTransaction.Value.HasValue)
                    {
                        throw new Exception("Ambiguously priced transactions.");
                    }

                    // Stupidly named, but an observation keeps the numeric field that is being forecast
                    // as "Value". So, "Value.Value" force unwraps the optional double named Value.
                    double transactionValueBefore = previousTransaction.Value.Value;
                    double transactionValueAfter = nextTransaction.Value.Value;

                    return (transactionValueBefore - ((transactionValueBefore - transactionValueAfter) / monthDifference));
                }

                private static bool ContainsDate(List<Observation> observations, DateTime date, bool precise = false)
                {
                    List<Observation> occurances = new List<Observation>();

                    if (precise)
                    {
                        occurances = observations.Where(x => x.Date.Year == date.Year &&
                                                            x.Date.Month == date.Month &&
                                                            x.Date.Day == x.Date.Day).ToList();
                    }
                    else
                    {
                        occurances = observations.Where(x => x.Date.Year == date.Year &&
                                                            x.Date.Month == date.Month).ToList();
                    }

                    if (occurances.Count > 0)
                        return true;
                    else
                        return false;
                }
            }
            #endregion

            #region Business Logic
            public static void MovingAverage(ref List<Observation> observations)
            {
                for (int index = 0; index < observations.Count; index++)
                {
                    observations[index].MovingAverage = CalculateMovingAverage(observations, index);
                }
            }

            private static double? CalculateMovingAverage(List<Observation> observations, int index)
            {
                if (observations.ElementAtOrDefault(index - 2) != null && observations.ElementAtOrDefault(index + 9) != null)
                    // Start 2 indexes before passed integer
                    // Take next 12 Observations
                    // Average the Observation's Value
                    // [4.8, 4.5, 4.3, 4.1, 4.9, 5.5, 6, 6.2, 6.3, 6.5, 6.3, 6, 5.8] -> [5.45, 5.53]
                    return observations.Skip(index - 2).Take(12).Select(x => x.Value).Average();
                else
                    return null;
            }

            public static void CenteredMovingAverage(ref List<Observation> observations)
            {
                for (int index = 0; index < observations.Count; index++)
                {
                    observations[index].CenteredMovingAverage = CalculateCenteredMovingAverage(observations, index);
                }
            }

            private static double? CalculateCenteredMovingAverage(List<Observation> observations, int index)
            {
                if (observations.ElementAtOrDefault(index).MovingAverage != null &&
                    observations.ElementAtOrDefault(index + 1).MovingAverage != null)
                {
                    return observations.Skip(index).Take(2).Select(x => x.MovingAverage).Average();
                }
                else
                    return null;
            }

            public static void SeasonalIrregularity(ref List<Observation> observations)
            {
                for (int index = 0; index < observations.Count; index++)
                {
                    observations[index].SeasonalIrregularity = CalculateSeasonalIrregularity(observations, index);
                }
            }

            private static double? CalculateSeasonalIrregularity(List<Observation> observations, int index)
            {
                // If numer is even
                if (observations.Count % 2 == 0)
                {
                    // Calculate seasonal irregularity via Centered Moving Average
                    if (observations[index].CenteredMovingAverage != null &&
                        observations[index].CenteredMovingAverage.GetValueOrDefault() != 0)
                        return (observations[index].Value / observations[index].CenteredMovingAverage);
                    else
                        return null;
                }
                // If number is odd
                else if (observations.Count % 2 == 1)
                {
                    // Calculate seasonal irregularity via Moving Average
                    if (observations[index].MovingAverage != null &&
                        observations[index].MovingAverage.GetValueOrDefault() != 0)
                        return (observations[index].Value / observations[index].MovingAverage);
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }

            public static void Seasonality(ref List<Observation> observations)
            {
                for (int index = 0; index < observations.Count; index++)
                {
                    observations[index].Seasonality = CalculateSeasonality(observations, index);
                }
            }

            public static double? CalculateSeasonality(List<Observation> observations, int index)
            {
                return observations.Where(x => x.Date.Month == observations[index].Date.Month)
                                   .Where(x => x.SeasonalIrregularity != null)
                                   .Select(x => x.SeasonalIrregularity)
                                   .Average();
            }

            public static void Deseasonalized(ref List<Observation> observations)
            {
                for (int index = 0; index < observations.Count; index++)
                {
                    observations[index].Deseasonalized = CalculateDeseasonalized(observations, index);
                }
            }

            private static double? CalculateDeseasonalized(List<Observation> observations, int index)
            {
                if (observations[index].Seasonality != null)
                    return (observations[index].Value / observations[index].Seasonality);
                else
                    return null;
            }

            public static void Trend(ref List<Observation> observations)
            {
                for (int index = 0; index < observations.Count; index++)
                {
                    observations[index].Trend = CalculateTrend(observations, index);
                }
            }

            public static double? CalculateTrend(List<Observation> observations, int index)
            {
                int xValueCount = observations.Where(x => x.Deseasonalized.HasValue).Count();

                int[] xValues = Enumerable.Range(1, xValueCount).ToArray();
                double[] yValues = observations.Where(x => x.Deseasonalized.HasValue)
                                   .Select(x => x.Deseasonalized)
                                   .Cast<double>()
                                   .ToArray();

                CalculateLinearRegression(xValues, yValues, out double rSquared, out double yIntercept, out double slope);

                return (yIntercept + slope * (index + 1));
            }

            private static void CalculateLinearRegression(int[] xVals, double[] yVals, out double rSquared, out double yIntercept, out double slope)
            {
                if (xVals.Length != yVals.Length)
                {
                    throw new Exception("Input values should be with the same length.");
                }

                double sumOfX = 0;
                double sumOfY = 0;
                double sumOfXSq = 0;
                double sumOfYSq = 0;
                double sumCodeviates = 0;

                for (var i = 0; i < xVals.Length; i++)
                {
                    var x = xVals[i];
                    var y = yVals[i];
                    sumCodeviates += x * y;
                    sumOfX += x;
                    sumOfY += y;
                    sumOfXSq += x * x;
                    sumOfYSq += y * y;
                }

                var count = xVals.Length;
                var ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
                var ssY = sumOfYSq - ((sumOfY * sumOfY) / count);

                var rNumerator = (count * sumCodeviates) - (sumOfX * sumOfY);
                var rDenom = (count * sumOfXSq - (sumOfX * sumOfX)) * (count * sumOfYSq - (sumOfY * sumOfY));
                var sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

                var meanX = sumOfX / count;
                var meanY = sumOfY / count;
                var dblR = rNumerator / Math.Sqrt(rDenom);

                rSquared = dblR * dblR;
                yIntercept = meanY - ((sCo / ssX) * meanX);
                slope = sCo / ssX;
            }

            public static void Forecast(ref List<Observation> observations)
            {
                for (int index = 0; index < observations.Count; index++)
                {
                    observations[index].Forecast = CalculateForecast(observations, index);
                }
            }

            public static double? CalculateForecast(List<Observation> observations, int index)
            {
                if (observations[index].Seasonality.HasValue && observations[index].Trend.HasValue)
                    return observations[index].Seasonality * observations[index].Trend;
                else
                    return null;
            }
            #endregion
        }
    }
}
