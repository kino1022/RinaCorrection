using System;
using System.Collections.Generic;
using ObservableCollections;

namespace RinaCorrection {
    /// <summary>
    /// 補正の演算を行うクラスに対して約束するインターフェース
    /// </summary>
    public interface ICorrector {

        /// <summary>
        /// 与えられた値に対して補正を施す
        /// </summary>
        /// <param name="baseValue"></param>
        /// <returns></returns>
        float Apply(float baseValue);
        
    }

    public class Corrector : ICorrector {
        
        private readonly IReadOnlyObservableList<ICorrectionValue> m_corrections;

        public Corrector(IReadOnlyObservableList<ICorrectionValue> corrections) {
            
            if (corrections is null || corrections.Count is 0) {
                throw new ArgumentNullException();
            }
            
            m_corrections = corrections;
        }

        public float Apply(float baseValue) {

            var groupedCorrections = new Dictionary<Type, List<ICorrectionValue>>();

            //補正値を型ごとに分類
            foreach (var correction in m_corrections) {

                var type = correction.Type.GetType();

                if (!groupedCorrections.ContainsKey(type)) {
                    groupedCorrections[type] = new List<ICorrectionValue>();
                }

                groupedCorrections[type].Add(correction);
            }

            //優先度順にソート
            var sortedTypes = new List<Type>(groupedCorrections.Keys);
            sortedTypes.Sort((a, b) => {
                var priorityA = groupedCorrections[a][0].Type.Priority;
                var priorityB = groupedCorrections[b][0].Type.Priority;
                return priorityA.CompareTo(priorityB);
            });

            //補正値の適用
            var correctedValue = baseValue;

            foreach (var type in sortedTypes) {
                var correctionType = groupedCorrections[type][0].Type;
                correctedValue = correctionType.Apply(correctedValue, groupedCorrections[type]);
            }

            return correctedValue;
        }
    }
}