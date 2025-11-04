using R3;

namespace RinaCorrection {
    public static class CorrectionValueExtension {

        /// <summary>
        /// 指定された補正値の変化を検知するストリームを生成する
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Observable<Unit> CreateValueChangeStream(this ICorrectionValue value) {
            return value
                .Value
                .Select(_ => Unit.Default);
        }
        
    }
}