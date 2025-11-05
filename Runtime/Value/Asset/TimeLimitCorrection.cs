using System;
using R3;
using RinaCorrection.Definition;
using Sirenix.Serialization;

namespace RinaCorrection.Asset {
    [Serializable]
    public class TimeLimitCorrection : ICorrectionValue{

        [OdinSerialize]
        private ReactiveProperty<float> m_value;
        
        [OdinSerialize]
        private ICorrectionType m_type;

        private TimeSpan m_limit;
        
        private Subject<ICorrectionValue> m_release = new Subject<ICorrectionValue>();
        
        private CompositeDisposable m_disposable = new CompositeDisposable();
        
        public ReadOnlyReactiveProperty<float> Value => m_value;
        
        public ICorrectionType Type => m_type;

        public Observable<ICorrectionValue> OnRelease => m_release;

        public TimeLimitCorrection(ICorrectionType type, float value, TimeSpan limit) {

            if (type is null) throw new ArgumentNullException();
            
            m_type = type;
            
            m_value = new  ReactiveProperty<float>(value);

            m_limit = limit;
            
            RegisterTimeLimit();
            
        }

        public void Dispose() {
            m_value.Value = 0.0f;
            m_disposable?.Dispose();
            m_release?.Dispose();
        }
        
        private void RegisterTimeLimit() {
            Observable
                .Timer(m_limit)
                .Subscribe(_ => {
                    m_release.OnNext(this);
                    m_release.OnCompleted();
                    m_value.Value = 0.0f;
                    Dispose();
                })
                .AddTo(m_disposable);
        }
    }
}