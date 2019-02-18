using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HistoryCollection
{
    // いい名前が思いつかない
    // ・FixedSizeSet？   ISet<T>だとこの用途に不要な集合演算が多くてイヤ 
    //                    ISetじゃないのにSet名乗るのも。。
    // ・FixedSizeQueue？ 重複を認めてる実装が多く混同しそう
    //                    QueueよりListっぽく使いたい
    /// <summary>重複せずに指定個数以上は古いものから忘れていくコレクション</summary>
    /// <typeparam name="T">コレクション内の要素の型</typeparam>
    public class HistoryCollection<T> : ICollection<T>
    {
        private const int defaultSize = 10;

        private LinkedList<T> list;

        private int limit;
        /// <summary>最大サイズを設定または取得します</summary>
        public int Limit
        {
            get => limit;
            set
            {
                if(value < 0) throw new ArgumentOutOfRangeException(nameof(Limit));
                limit = value;
                while(Limit < Count) list.RemoveFirst();
            }
        }

        /// <summary>要素数を取得します</summary>
        public int Count => list?.Count ?? 0;

        bool ICollection<T>.IsReadOnly => false;

        #region constructor
        /// <summary><see cref="HistoryCollection{T}" />クラスの新しいインスタンスを初期化します</summary>
        public HistoryCollection() : this(defaultSize, Array.Empty<T>()) { }

        /// <summary><see cref="HistoryCollection{T}" />クラスの新しいインスタンスを初期化します</summary>
        /// <param name="limit">最大サイズ</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="limit" />が 0 未満です</exception>
        public HistoryCollection(int limit) : this(limit, Array.Empty<T>()) { }

        /// <summary><see cref="HistoryCollection{T}" />クラスの新しいインスタンスを初期化します</summary>
        /// <param name="collection">元となるコレクション</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection" />は<see langword="null" />です</exception>
        public HistoryCollection(IEnumerable<T> collection) : this(defaultSize, collection) { }

        /// <summary><see cref="HistoryCollection{T}" />クラスの新しいインスタンスを初期化します</summary>
        /// <param name="limit">最大サイズ</param>
        /// <param name="collection">元となるコレクション</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection" />は<see langword="null" />です</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="limit" />が 0 未満です</exception>
        public HistoryCollection(int limit, IEnumerable<T> collection)
        {
            if(limit < 0) throw new ArgumentOutOfRangeException(nameof(limit));
            if(collection == null) throw new ArgumentNullException(nameof(collection));

            Limit = limit;
            var set = new HashSet<T>(collection);
            list = new LinkedList<T>(set.Skip(Math.Max(0, set.Count() - Limit)));
        }
        #endregion

        /// <summary>末尾に指定された値を追加します 最大サイズを超えた場合先頭から削除します 重複があった場合は削除してから追加します</summary>
        /// <param name="item">追加する値</param>
        public virtual void Add(T item)
        {
            Remove(item);
            list.AddLast(item);

            while(Limit < Count) list.RemoveFirst();
        }

        /// <summary>すべての値を削除します</summary>
        public virtual void Clear() => list.Clear();

        /// <summary>値がコレクションに含まれるかどうかを返します</summary>
        /// <param name="item">検索する値</param>
        /// <returns>含まれるかどうか</returns>
        public virtual bool Contains(T item) => list.Contains(item);

        /// <summary>全体を互換性のある１次元配列の、指定したインデックスから始まる位置にコピーします</summary>
        /// <param name="array">互換性のある１次元配列</param>
        /// <param name="index">コピー先<paramref name="array" />のコピー開始インデックス</param>
        /// <exception cref="ArgumentNullException"><paramref name="array" />は<see langword="null" />です</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" />が 0 未満です</exception>
        /// <exception cref="ArgumentException">コピー先<paramref name="array" />の<paramref name="index" />から最後までの使用可能領域を超えています</exception>
        public virtual void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

        /// <summary>値を削除します</summary>
        /// <param name="item">削除する値</param>
        /// <returns>削除できたかどうか</returns>
        public virtual bool Remove(T item)
        {
            var b = false;
            while(list.Remove(item)) b = true;

            return b;
        }

        public override string ToString() => $"{{{string.Join(", ", this)}}}";

        /// <summary><see cref="HistoryCollection{T}" />を反復処理する列挙子を返します</summary>
        /// <returns>列挙子</returns>
        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
