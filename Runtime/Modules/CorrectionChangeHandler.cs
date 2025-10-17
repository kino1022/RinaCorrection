using System;
using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using R3;

namespace RinaCorrection.Modules {
    /// <summary>
    /// 受け取った補正値管理クラスに再計算の必要が生じた場合に通知するクラス
    /// </summary>
    public class CorrectionChangeHandler : ICorrectionChangeHandler, IDisposable {

        private ICorrectionManager m_manager;

        private Dictionary<ICorrectionValue, CompositeDisposable> m_correction;
        
        private CompositeDisposable m_rootDisposables;

        public Action CorrectionChangeEvent { get; set; }

        public CorrectionChangeHandler(ICorrectionManager manager) {
            
            m_manager = manager ?? throw new ArgumentNullException(nameof(manager));
            
            m_correction = new Dictionary<ICorrectionValue, CompositeDisposable>();
            
            RegisterAddCorrection();
            
            RegisterRemoveCorrection();
            
        }

        public void Dispose() {
            
            m_correction.Values
                .ToList()
                .ForEach(x => x.Dispose());
            
            m_correction.Clear();
            
            m_rootDisposables.Dispose();
            
        }

        private void RegisterAddCorrection() {
            m_manager
                .Corrections
                .ObserveAdd()
                .Subscribe(OnAddCorrection)
                .AddTo(m_rootDisposables);
        }

        private void RegisterRemoveCorrection() {
            m_manager
                .Corrections
                .ObserveRemove()
                .Subscribe(OnRemoveCorrection)
                .AddTo(m_rootDisposables);
        }

        private void OnAddCorrection(CollectionAddEvent<ICorrectionValue> x) {

            if (x.Value is null) {
                return;
            }
            
            //すでに登録されている補正値なら返す
            if (m_correction.TryGetValue(x.Value, out var disposable)) {
                return;
            }

            var pair = new KeyValuePair<ICorrectionValue, CompositeDisposable>(x.Value, new CompositeDisposable());

            m_correction.Add(pair.Key, pair.Value);
            
            RegisterCorrectionValueChanged(pair);
        }

        private void OnRemoveCorrection(CollectionRemoveEvent<ICorrectionValue> x) {

            if (x.Value is null) {
                return;
            }

            var disposable = m_correction[x.Value] ?? throw new NullReferenceException();
            
            disposable.Dispose();

            m_correction.Remove(x.Value);
            
            CorrectionChangeEvent?.Invoke();
        }

        private void RegisterCorrectionValueChanged(KeyValuePair<ICorrectionValue, CompositeDisposable> pair) {
            
            pair
                .Key
                .Value
                .Subscribe(OnCorrectionValueChanged)
                .AddTo(pair.Value);
            
        }

        private void OnCorrectionValueChanged(float next) {
            CorrectionChangeEvent?.Invoke();
        }
    }
}