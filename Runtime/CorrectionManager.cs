using System;
using System.Collections.Generic;
using ObservableCollections;

namespace RinaCorrection {
    [Serializable]
    public class CorrectionManager : ICorrectionManager, ICorrector {

        private ObservableList<ICorrectionValue> m_corrections = new();
        
        public IReadOnlyObservableList<ICorrectionValue> Corrections => m_corrections;

        public float Apply(float baseValue) {
            return 0.0f;
        }

        public void Add(ICorrectionValue value) {
            
        }

        public void Remove(ICorrectionValue value) {
            
        }

        public void Clear() {
            
        }
    }
}