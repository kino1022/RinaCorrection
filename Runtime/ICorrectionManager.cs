using System.Collections.Generic;
using ObservableCollections;

namespace RinaCorrection {
    /// <summary>
    /// 補正値のインスタンスを管理するクラスに対して約束するインターフェース
    /// </summary>
    public interface ICorrectionManager : ICorrector {
        
        IReadOnlyObservableList<ICorrectionValue> Corrections { get; }
        
        void Add (ICorrectionValue value);
        
        void Remove (ICorrectionValue value);

        void Clear();
        
    }
}