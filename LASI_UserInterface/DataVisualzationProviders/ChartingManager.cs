﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using LASI.Algorithm;
using LASI.Algorithm.DocumentConstructs;
using LASI.Algorithm.Thesauri;

namespace LASI.UserInterface.DataVisualzationProviders
{
    public static class ChartingManager
    {
        public static void ChangeChartKind(ChartKind chartKind) {
            foreach (var pair in documentsByChart) {

                Document doc = pair.Value;
                Chart chart = pair.Key;

                IEnumerable<KeyValuePair<string, float>> data = null;

                switch (chartKind) {
                    case ChartKind.SubjectVerbObject:
                        data = GetSVOIData(doc);
                        break;
                    case ChartKind.NounPhrasesOnly:
                        data = GetNounPhraseData(doc);
                        break;
                }
                data = data.Take(ChartItemLimit);
                chart.Series.Clear();
                chart.Series.Add(new BarSeries
                {
                    DependentValuePath = "Value",
                    IndependentValuePath = "Key",
                    ItemsSource = data,
                    IsSelectionEnabled = true,

                });
                chart.Title = string.Format("Key Relationships in {0}", doc.FileName);
                break;
            }
        }


        #region Chart Transposing Methods


        /// <summary>
        /// Reconfigures all charts to Subjects Column perspective
        /// </summary>
        /// <returns>A Task which completes on the successful reconstruction of all charts</returns>
        public static async Task ToColumnCharts() {
            foreach (var chart in WindowManager.ResultsScreen.FrequencyCharts.Items) {
                var items = GetItemSourceFor(chart);
                items.Reverse();
                var series = new ColumnSeries

                {
                    DependentValuePath = "Value",
                    IndependentValuePath = "Key",
                    ItemsSource = items,
                    IsSelectionEnabled = true


                };
                ResetChartContent(chart, series);
            }
            await Task.Delay(1);
        }



        /// <summary>
        /// Reconfigures all charts to Subjects Pie perspective
        /// </summary>
        /// <returns>A Task which completes on the successful reconstruction of all charts</returns>
        public static async Task ToPieCharts() {
            foreach (var chart in WindowManager.ResultsScreen.FrequencyCharts.Items) {
                var items = GetItemSourceFor(chart);
                items.Reverse();
                var series = new PieSeries
                {
                    DependentValuePath = "Value",
                    IndependentValuePath = "Key",
                    ItemsSource = items,
                    IsSelectionEnabled = true,
                };
                series.IsMouseCaptureWithinChanged += (sender, e) => {
                    series.ToolTip = (series.SelectedItem as DataPoint).DependentValue;
                };

                ResetChartContent(chart, series);
            }
            await Task.Delay(1);
        }

        /// <summary>
        /// Reconfigures all charts to Subjects Bar perspective
        /// </summary>
        /// <returns>A Task which completes on the successful reconstruction of all charts</returns>
        public static async Task ToBarCharts() {
            foreach (var chart in WindowManager.ResultsScreen.FrequencyCharts.Items) {
                var items = GetItemSourceFor(chart);
                items.Reverse();
                var series = new BarSeries
                {
                    DependentValuePath = "Value",
                    IndependentValuePath = "Key",
                    ItemsSource = items,
                    IsSelectionEnabled = true,
                };
                ResetChartContent(chart, series);
            }
            await Task.Delay(1);
        }
        public static IEnumerable<KeyValuePair<string, float>> GetAppropriateData(object chart) {
            var items = ChartingManager.GetAppropriateDataSet(documentsByChart[((chart as TabItem).Content as Chart)]);
            return items;
        }
        public static async void BuildDefaultBarChartDisplay(Document document) {

            var valueList = ChartingManager.GetAppropriateDataSet(document);
            Series series = new BarSeries
            {
                DependentValuePath = "Value",
                IndependentValuePath = "Key",
                ItemsSource = valueList,
                IsSelectionEnabled = true,
                Tag = document,

            };

            var chart = new Chart
            {
                Title = string.Format("Key Subjects in {0}", document.FileName),
                Tag = valueList.ToArray()
            };

            series.MouseMove += (sender, e) => {
                series.ToolTip = (e.Source as DataPoint).IndependentValue;
            };
            documentsByChart.Add(chart, document);
            chart.Series.Add(series);

            var tabItem = new TabItem
            {
                Header = document.FileName,
                Content = chart,
                Tag = chart
            };
            WindowManager.ResultsScreen.FrequencyCharts.Items.Add(tabItem);
            await ChartingManager.ToBarCharts();
        }




        #endregion

        #region General Chart Building Methods

        public static void ResetChartContent(object c, DataPointSeries series) {
            ((c as TabItem).Content as Chart).Series.Clear();
            ((c as TabItem).Content as Chart).Series.Add(series);
        }

        public static List<KeyValuePair<string, float>> GetItemSourceFor(object chart) {
            var chartSource = ((chart as TabItem).Content as Chart).Tag as IEnumerable<KeyValuePair<string, float>>;
            var items = (from i in chartSource.ToArray()

                         orderby i.Value descending
                         select new KeyValuePair<string, float>(i.Key.ToString(), i.Value)).Take(10).ToList();
            return items;
        }

        #endregion
        private static IEnumerable<KeyValuePair<string, float>> GetSVOIData(Document doc) {
            var data = GetVerbWiseAssociationData(doc);
            return from svs in data

                   let SV = new KeyValuePair<string, float>(string.Format("{0} -> {1}\n", svs.Subject.Text, svs.Verbial.Text) + (svs.Direct != null ? " -> " + svs.Direct.Text : "") + (svs.Indirect != null ? " -> " + svs.Indirect.Text : ""),
                       (float) Math.Round(svs.RelationshipWeight, 2))
                   group SV by SV into svg
                   select svg.Key;

        }
        private static IEnumerable<NpVpNpNpQuatruple> GetVerbWiseAssociationData(Document doc) {
            var data =
                 from svPair in
                     (from v in doc.Phrases.GetVerbPhrases().AsParallel().WithSubject()
                      from s in v.BoundSubjects.AsParallel()
                      from dobj in v.DirectObjects.DefaultIfEmpty()
                      from iobj in v.IndirectObjects.DefaultIfEmpty()

                      select new NpVpNpNpQuatruple
                      {
                          Subject = s as NounPhrase ?? null,
                          Verbial = v as VerbPhrase ?? null,
                          Direct = dobj as NounPhrase ?? null,
                          Indirect = iobj as NounPhrase ?? null,
                          RelationshipWeight = s.Weight + v.Weight + (dobj != null ? dobj.Weight : 0) + (iobj != null ? iobj.Weight : 0)
                      } into tupple
                      where
                        tupple.Direct != null ||
                        tupple.Indirect != null &&
                        tupple.Subject.Text != (tupple.Direct ?? tupple.Indirect).Text
                      select tupple).Distinct()
                 select svPair into svps

                 orderby svps.RelationshipWeight

                 select svps;
            return data.ToArray();
        }

        private static IEnumerable<KeyValuePair<string, float>> GetNounPhraseData(Document doc) {
            return from NP in doc.Phrases.GetNounPhrases().Distinct() //.Except(doc.Phrases.GetPronounPhrases()) 

                   group NP by new
                   {
                       NP.Text,
                       NP.Weight
                   } into NP
                   select NP.Key into master
                   orderby master.Weight descending
                   select new KeyValuePair<string, float>(master.Text, (float) Math.Round(master.Weight, 2));
        }
        public static IEnumerable<KeyValuePair<string, float>> GetAppropriateDataSet(Document document) {
            var valueList = chartKind == ChartKind.NounPhrasesOnly ? GetNounPhraseData(document) : chartKind == ChartKind.SubjectVerbObject ? GetSVOIData(document) : GetSVOIData(document);
            return valueList;
        }
        public static async Task BuildSVOIGridViewAsync(Document doc) {

            var transformedData = await Task.Factory.StartNew(() => {
                return CreateStringListsForData(ChartingManager.GetVerbWiseAssociationData(doc));
            });
            var wpfToolKitDataGrid = new Microsoft.Windows.Controls.DataGrid
            {
                ItemsSource = transformedData,
            };
            var tab = new TabItem
            {
                Header = doc.FileName,
                Content = wpfToolKitDataGrid
            };
            WindowManager.ResultsScreen.SVODResultsTabControl.Items.Add(tab);

        }
        private static IEnumerable<object> CreateStringListsForData(IEnumerable<NpVpNpNpQuatruple> elementsToSerialize) {
            var dataRows = from result in elementsToSerialize
                           orderby result.RelationshipWeight
                           select new
                           {
                               Subject = result.Subject != null ? result.Subject.Text : string.Empty,
                               Verbial = result.Verbial != null ? result.Verbial.Text : string.Empty,
                               Direct = result.Direct != null ? result.Direct.Text : string.Empty,
                               Indirect = result.Indirect != null ? result.Indirect.Text : string.Empty,
                           };
            return dataRows;
        }
        private static Dictionary<Chart, Document> documentsByChart = new Dictionary<Chart, Document>();


        private const int chartItemLimit = 14;

        public static int ChartItemLimit {
            get {
                return chartItemLimit;
            }
        }




        public static ChartKind chartKind {
            get;
            set;
        }
    }
    #region Result Bulding Helper Structs
    /// <summary>
    /// A little data type which provides custom uniqueness logic for NounPhrase to VerbPhrase  relationships. Provides the implementation of I equality comparer which is to be passed to the 
    /// "Distinct()" method of en IEnummerable collectio  of NpVpNpNpQuatruple instances.
    /// It is defined because distinct does not support lambda(read function) arguments like my query operatorrs do.
    /// Pay this type little heed
    /// </summary>
    struct CompositionComparer : IEqualityComparer<NpVpNpNpQuatruple>
    {
        public bool Equals(NpVpNpNpQuatruple x, NpVpNpNpQuatruple y) {
            return x == y;
        }

        public int GetHashCode(NpVpNpNpQuatruple obj) {
            return obj.GetHashCode();
        }
    }
    /// <summary>
    /// Sometimes an anonymous type simple will not do. So this little class is defined to 
    /// store temporary query data from transposed tables. god it is late. I can't document properly.
    /// </summary>
    class NpVpNpNpQuatruple
    {
        public NounPhrase Subject {
            get;
            set;
        }
        public VerbPhrase Verbial {
            get;
            set;
        }
        public NounPhrase Direct {
            get;
            set;
        }
        public NounPhrase Indirect {
            get;
            set;
        }
        public decimal RelationshipWeight {
            get;
            set;
        }
        /// <summary>
        /// Returns a textual representation of the NpVpNpNpQuatruple.
        /// </summary>
        /// <returns>a textual representation of the NpVpNpNpQuatruple.</returns>
        public override string ToString() {
            var result = Subject.Text + Verbial.Text;
            if (Direct != null) {
                result += Direct.Text;
            }
            if (Indirect != null) {
                result += Indirect.Text;
            }
            return result;
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }
        public override bool Equals(object obj) {
            return this == obj as NpVpNpNpQuatruple;
        }
        public static bool operator ==(NpVpNpNpQuatruple lhs, NpVpNpNpQuatruple rhs) {
            return lhs.ToString() == rhs.ToString() ||
                 lhs.Subject.IsSimilarTo(rhs.Subject) && lhs.Verbial.IsSimilarTo(rhs.Verbial);
        }
        public static bool operator !=(NpVpNpNpQuatruple lhs, NpVpNpNpQuatruple rhs) {
            return !(lhs == rhs);
        }

    }

    #endregion
}
