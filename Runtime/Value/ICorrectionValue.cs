using System;
using R3;

namespace RinaCorrection {
    public interface ICorrectionValue : IDisposable {
        
        ReadOnlyReactiveProperty<float> Value { get; }
        
        
    }
}