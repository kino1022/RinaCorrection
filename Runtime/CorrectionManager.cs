using System;
using System.Collections.Generic;
using ObservableCollections;
using R3;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace RinaCorrection {

    /// <summary>
    /// 補正値のインスタンスを管理するクラスに対して約束するインターフェース
    /// </summary>
    public interface ICorrectionManager : ICorrector {
        
        /// <summary>
        /// 補正値のリスト
        /// </summary>
        IReadOnlyObservableList<ICorrectionValue> Corrections { get; }
        
        /// <summary>
        /// 補正値に変化が生じた際に流れるストリーム
        /// </summary>
        Observable<Unit> OnChanged { get; }
        
        /// <summary>
        /// 補正値の追加を行う
        /// </summary>
        /// <param name="correction"></param>
        void Add(ICorrectionValue correction);
        
        /// <summary>
        /// 補正値の除外を行う
        /// </summary>
        /// <param name="correction"></param>
        void Remove(ICorrectionValue correction);
        
        /// <summary>
        /// 補正値の初期化を行う
        /// </summary>
        void Clear();
    }
    
    [Serializable]
    public class CorrectionManager : ICorrectionManager , IDisposable{

        [OdinSerialize]
        [LabelText("管理している補正値")]
        [ReadOnly]
        private ObservableList<ICorrectionValue> m_corrections = new();

        [OdinSerialize]
        [LabelText("計算実行クラス")]
        [ReadOnly]
        private ICorrector m_corrector;

        private Subject<Unit> m_onChanged = new Subject<Unit>();
        
        private Dictionary<ICorrectionValue, IDisposable> m_valueSubscriptions = new();

        private CompositeDisposable m_disposable = new CompositeDisposable();
        
        public IReadOnlyObservableList<ICorrectionValue> Corrections => m_corrections;
        
        public Observable<Unit> OnChanged => m_onChanged;

        public CorrectionManager() {
            m_disposable = new  CompositeDisposable();
            m_corrections = new();
            
            RegisterCorrectionChanged();

            m_corrector = new Corrector(m_corrections);
        }

        public void Dispose() {
            foreach (var kv in m_valueSubscriptions.Values) {
                kv?.Dispose();
            }
            m_valueSubscriptions.Clear();
            m_disposable?.Dispose();
            m_onChanged?.Dispose();
        }

        public void Add(ICorrectionValue correction) {

            if (correction is null) throw new ArgumentNullException();
            
            m_corrections.Add(correction);
            
            RegisterValueChangeStream(correction);
        }

        public void Remove(ICorrectionValue correction) {

            if (correction is null) throw new ArgumentNullException();
            
            UnregisterValueChangeStream(correction);
            
            m_corrections.Remove(correction);
            
        }

        public void Clear() {
            var items = new List<ICorrectionValue>(m_corrections);
            foreach(var item in items) Remove(item);
        }
        
        public float Apply (float baseValue) => m_corrector.Apply(baseValue);

        private void RegisterCorrectionChanged() {
            
            m_corrections
                .ObserveAdd()
                .Select(_ => Unit.Default)
                .Subscribe(m_onChanged.OnNext)
                .AddTo(m_disposable);

            m_corrections
                .ObserveRemove()
                .Select(_ => Unit.Default)
                .Subscribe(m_onChanged.OnNext)
                .AddTo(m_disposable);

            m_corrections
                .ObserveReplace()
                .Select(_ => Unit.Default)
                .Subscribe(m_onChanged.OnNext)
                .AddTo(m_disposable);

            m_corrections
                .ObserveReset()
                .Select(_ => Unit.Default)
                .Subscribe(m_onChanged.OnNext)
                .AddTo(m_disposable);
        }
        
        private void RegisterValueChangeStream(ICorrectionValue value) {

            if (value is null) {
                Debug.Log("受け取った補正値インスタンスがnullでした");
                throw new ArgumentNullException();
            }
            
            if (m_valueSubscriptions.TryGetValue(value, out _)) {
                Debug.Log("すでに存在している補正値の変化ストリームを登録しようとしました。");
                return;
            }

            var cd = new CompositeDisposable();


            value
                .Value
                .Select(_ => Unit.Default)
                .Subscribe(m_onChanged.OnNext)
                .AddTo(cd);

            value
                .OnRelease
                .Subscribe(x => {
                    if (x is not null) Remove(x);
                })
                .AddTo(cd);
            
            cd.AddTo(m_disposable);

            m_valueSubscriptions.Add(value, cd);
        }
        
        private void UnregisterValueChangeStream(ICorrectionValue value) {

            if (value is null) {
                throw new ArgumentNullException();
            }
            
            if (m_valueSubscriptions.TryGetValue(value, out var sub)) {
                sub.Dispose();
                m_valueSubscriptions.Remove(value);
            }
        }
    }
}