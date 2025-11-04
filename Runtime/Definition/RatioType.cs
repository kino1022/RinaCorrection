namespace RinaCorrection.Definition {
    /// <summary>
    /// 割合での補正値分類
    /// </summary>
    public readonly struct RatioType : ICorrectionType {
        
        public int Priority => 10;

        public float Apply(float baseValue, System.Collections.Generic.List<ICorrectionValue> corrections) {
            //補正値の合計値
            var totalRatio = 0.0f;

            corrections.ForEach(c => totalRatio += c.Value.CurrentValue);

            return baseValue * (1.0f + totalRatio);
        }
    }
}