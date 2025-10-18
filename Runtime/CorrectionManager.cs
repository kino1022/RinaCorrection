using System;
using System.Collections.Generic;
using ObservableCollections;
using RinaCorrection.Definition;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace RinaCorrection {
    [Serializable]
    public class CorrectionManager : ICorrectionManager, ICorrector {

        [OdinSerialize]
        [LabelText("補正値")]
        private ObservableList<ICorrectionValue> m_corrections = new();
        
        public IReadOnlyObservableList<ICorrectionValue> Corrections => m_corrections;

        public float Apply(float baseValue) {
            return baseValue;
        }

        public void Add(ICorrectionValue value) {
            
        }

        public void Remove(ICorrectionValue value) {
            
        }

        public void Clear() {
            
        }
    }
}