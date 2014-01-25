﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc2x3.Kernel;
using Xbim.IO;

namespace Xbim.Analysis.Comparing
{
    public class ComparisonManager
    {
        private List<IModelComparerII> _comparers = new List<IModelComparerII>();
        private ComparisonResultsCollection _results = new ComparisonResultsCollection();
        private XbimModel _baseModel;
        private XbimModel _revisedModel;

        public ComparisonResultsCollection Results { get { return _results; } }

        public ComparisonManager(XbimModel baseModel, XbimModel revisedModel)
        {
            _baseModel = baseModel;
            _revisedModel = revisedModel;
        }

        public void AddComparer(IModelComparerII comparer)
        {
            if (comparer == null) throw new ArgumentNullException();
            if (!_comparers.Contains(comparer))
                _comparers.Add(comparer);

        }

        public void Compare<T>() where T : IfcRoot
        {
            _results = new ComparisonResultsCollection();
            var baselineRoots = _baseModel.Instances.OfType<T>();

            //perform all the comparisons in parallel
            //foreach (var comparer in _comparers)
            var opts = new ParallelOptions() {MaxDegreeOfParallelism = 8 };
            Parallel.ForEach<IModelComparerII>(_comparers, opts, comparer =>
            {
                foreach (var root in baselineRoots)
                {
                    var result = comparer.Compare<T>(root, _revisedModel);
                    //result can be null if there is no sense in comparison
                    //like comparison of geometry when there is no geometry at all
                    if (result != null)
                        lock (_results)
                        {
                            _results.Add(result);   
                        }
                }

                //get objects which are supposed to be new
                var residuum = comparer.GetResidualsFromRevision<T>(_revisedModel);
                lock (_results)
                {
                    _results.Add(residuum);
                }
            }
            );
        }

        public void SaveResultToCSV(string path)
        {
            var file = File.CreateText(path);

            //create header
            file.Write("{0},{1}","Baseline", "Revision");
            foreach (var cmp in _comparers)
            {
                file.Write(",{0} (Weight: {1})", cmp.ComparisonType, cmp.Weight);
            }
            file.Write(",Match\n");

            //write content
            foreach (var item in _results.Results)
            {
                var baseLabel = item.Root != null ? "#" + Math.Abs(item.Root.EntityLabel).ToString() : "-";
                var bestMatch = item.BestMatch;

                if (bestMatch.Count == 0)
                {
                    file.Write("{0},{1}", baseLabel, "-");
                    foreach (var cmp in _comparers)
                        file.Write("," + ResultType.ONLY_BASELINE);
                    file.Write(",100%\n");
                    continue;
                }

                foreach (var match in item.BestMatch)
                {
                    var matchLabel = "#" + Math.Abs(match.Root.EntityLabel).ToString();
                    file.Write("{0},{1}", baseLabel, matchLabel);
                    foreach (var cmp in _comparers)
                    {
                        var results = item.Results.Where(r => r.Comparer == cmp);
                        if (results.Count() == 0)
                            file.Write(",-");
                        else
                        {
                            var result = results.Where(r => r.Candidates.Contains(match.Root)).FirstOrDefault();
                            file.Write("," + (result != null ? "true" : "false"));
                        }
                    }
                    var relWeight = (float)(match.Weight) / (float)(item.MaximalWeight) * 100f;
                    file.WriteLine(",{0,-1:f2}%", relWeight);
                }
            }
            file.Close();
        }

    }

    /// <summary>
    /// Collection of comparison results. This class keeps detail results where
    /// there are baseline objects and lists of candidates for every comparison 
    /// criterium. You can either perform your own interpretation of the results
    /// or use predefined properties to get new, deleted, matched and ambiquous 
    /// objects from the baseline and revised models.
    /// </summary>
    public class ComparisonResultsCollection
    {
        private List<ComparisonResults> _results = new List<ComparisonResults>();
        public IEnumerable<ComparisonResults> Results { get { return _results; } }

        /// <summary>
        /// Add new comparison result to the collection
        /// </summary>
        /// <param name="result"></param>
        public void Add(ComparisonResult result)
        {
            var baseline = result.Baseline;
            var item = _results.Where(i => i.Root == baseline).FirstOrDefault();
            if (item != null)
                item.Add(result);
            else
            {
                var cr = new ComparisonResults();
                cr.Add(result);
                _results.Add(cr);
            }
        }

        /// <summary>
        /// Objects from baseline model which are not present in revision
        /// </summary>
        public IEnumerable<IfcRoot> Deleted
        {
            get 
            {
                foreach (var result in this._results)
                {
                    if (result.BestMatch.Count == 0 && result.Root != null)
                        yield return result.Root;
                }
            }
        }

        /// <summary>
        /// Objects from revision which are not present in baseline model
        /// </summary>
        public IEnumerable<IfcRoot> Added
        {
            get
            {
                foreach (var result in this._results)
                {
                    if (result.Root == null)
                        foreach (var item in result.BestMatch)
                        {
                            yield return item.Root;
                        }
                }
            }
        }

        /// <summary>
        /// Dictionary of matching objects where key is 
        /// from baseline model and value is from revision
        /// </summary>
        public IDictionary<IfcRoot, IfcRoot> MatchOneToOne
        {
            get 
            {
                var res = new Dictionary<IfcRoot, IfcRoot>();
                foreach (var result in this._results)
                {
                    var bestMatch = result.BestMatch;
                    if (result.Root != null && bestMatch.Count == 1)
                        res.Add(result.Root, bestMatch[0].Root);
                }
                return res;
            }
        }

        /// <summary>
        /// Dictionary of baseline objects and weighted objects 
        /// from revision where there are more than one best match
        /// in the revision model. Weights are based on the weights
        /// of comparers.
        /// </summary>
        public IDictionary<IfcRoot, List<WeightedRoot>> Ambiquity
        {
            get
            {
                var res = new Dictionary<IfcRoot, List<WeightedRoot>>();
                foreach (var result in this._results)
                {
                    var bestMatch = result.BestMatch;
                    if (result.Root != null && bestMatch.Count > 1)
                        res.Add(result.Root, bestMatch);
                }
                return res;
            }
        }

    }

    public class ComparisonResults
    {
        private List<ComparisonResult> _results = new List<ComparisonResult>();
        public IEnumerable<ComparisonResult> Results
        {
            get
            {
                foreach (var item in _results)
                {
                    yield return item;
                }
            }
        }

        public IfcRoot Root
        {
            get
            {
                var first = _results.FirstOrDefault();
                return first != null ? _results.FirstOrDefault().Baseline : null;
            }
        }

        public void Add(ComparisonResult result)
        {
            if (Root == null)
            {
                _results.Add(result);
                return;
            }
            if (Root == result.Baseline)
                _results.Add(result);
            else
                throw new ArgumentException("Result must have the same baseline object");
        }

        public ResultType ResultType
        {
            get
            {
                var best = BestMatch;
                var weightedResults = WeightedResults;
                if (best.Count == 1 && Root != null) return ResultType.MATCH;
                if (Root != null && best.Count == 0) return ResultType.ONLY_BASELINE;
                if (Root == null && weightedResults.Count > 0) return ResultType.ONLY_REVISION;
                if (Root != null && best.Count > 1) return ResultType.AMBIGUOUS;
                return ResultType.AMBIGUOUS;
            }
        }

        public List<WeightedRoot> WeightedResults 
        {
            get 
            {
                var weightedResults = new List<WeightedRoot>();
                foreach (var result in _results)
                {
                    foreach (var item in result.Candidates)
                    {
                        var existing = weightedResults.Where(wr => wr.Root == item).FirstOrDefault();
                        if (existing != null)
                            existing.Weight += result.Comparer.Weight;
                        else
                            weightedResults.Add(new WeightedRoot() { Root = item, Weight = result.Comparer.Weight });
                    }
                }
                //sort result
                weightedResults.Sort();

                return weightedResults;
            }
        }

        public List<WeightedRoot> BestMatch
        {
            get 
            {
                var weightedResults = WeightedResults;
                if (weightedResults.Count == 0)
                    return new List<WeightedRoot>();
                var weight = weightedResults.Last().Weight;
                return weightedResults.Where(wr => wr.Weight == weight).ToList();
            }
        }

        public int MaximalWeight
        {
            get 
            {
                var w = 0;
                foreach (var result in _results)
                    w += result.Comparer.Weight;
                return w;
            }
        }

        public int BestMatchWeight
        {
            get
            {
                var wr = WeightedResults;
                if (wr.Count == 0)
                    return 0;
                return wr.Last().Weight;
            }
        }

        
    }

    public class WeightedRoot : IComparable
    {
        public int Weight;
        public IfcRoot Root;

        public int CompareTo(object obj)
        {
            var second = obj as WeightedRoot;
            if (second == null)
                throw new NotImplementedException();
            return Weight.CompareTo(second.Weight);
        }
    }
}