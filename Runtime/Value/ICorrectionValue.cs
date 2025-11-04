using System;
using R3;
using RinaCorrection.Definition;

namespace RinaCorrection {
    public interface ICorrectionValue : IDisposable {
        
        /// <summary>
        /// 補正値の値
        /// </summary>
        ReadOnlyReactiveProperty<float> Value { get; }
        
        /// <summary>
        /// 補正値の分類
        /// </summary>
        ICorrectionType Type { get; }
        
        /// <summary>
        /// 補正値が消滅した際に流れるストリーム
        /// </summary>
        Observable<ICorrectionValue> OnRelease { get; }
    }
}