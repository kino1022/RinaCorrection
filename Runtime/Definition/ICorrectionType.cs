using System.Collections.Generic;

namespace RinaCorrection.Definition {
    /// <summary>
    /// 計算方法で分類された補正値の型
    /// </summary>
    public interface ICorrectionType {
        
        /// <summary>
        /// 補正値の実計算
        /// </summary>
        /// <param name="baseValue"></param>
        /// <param name="corrections"></param>
        /// <returns></returns>
        float Apply(float baseValue, List<ICorrectionValue> corrections);
        
    }
}