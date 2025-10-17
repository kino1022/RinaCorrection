using System;

namespace RinaCorrection.Modules {
    /// <summary>
    /// 補正値に変化が生じた際に通知するクラスに対して約束するインターフェース
    /// </summary>
    public interface ICorrectionChangeHandler {
        
        /// <summary>
        /// 補正値が変化した際のコールバック
        /// </summary>
        Action CorrectionChangeEvent { get; set; }
        
    }
}