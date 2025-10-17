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
}