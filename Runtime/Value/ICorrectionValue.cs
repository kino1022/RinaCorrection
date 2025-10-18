using System;
using R3;
using RinaCorrection.Definition;

namespace RinaCorrection {
    public interface ICorrectionValue : IDisposable {
        
        ReadOnlyReactiveProperty<float> Value { get; }
        
        ICorrectionType Type { get; }
    }
}