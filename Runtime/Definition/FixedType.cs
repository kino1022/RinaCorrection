using System.Collections.Generic;

namespace RinaCorrection.Definition {
    /// <summary>
    /// 固定値での補正値
    /// </summary>
    public readonly struct FixedType : ICorrectionType {
        
        public int Priority => 0;
        
        public float Apply(float baseValue, List<ICorrectionValue> corrections) {
            //補正値の合計値
            var totalCorrection = 0.0f;
            
            corrections.ForEach(c => totalCorrection += c.Value.CurrentValue);
            
            return baseValue + totalCorrection;
        }
        
    }
}