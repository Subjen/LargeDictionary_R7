using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeDictionary_R7
{
    public class LargeDictionary<TKey, TValue> where TKey : notnull
    {
        private const int PageSize = 1000000; // 1M элементов на страницу
        private readonly Dictionary<int, Dictionary<TKey, TValue>> Pages;

        public LargeDictionary() 
        {
            Pages = [];
        }

        public TValue this[TKey key] 
        {
            get
            {
                if (TryGetValue(key, out TValue value))
                {
                    return value;
                }
                throw new KeyNotFoundException();
            }
            set => Add(key, value);
        }

        public ICollection<TKey> Keys => throw new NotImplementedException(); //_pages.Values.Select(page => page.Keys);

        public ICollection<TValue> Values => throw new NotImplementedException();

        public long Count => Pages.Values.Sum(page => (long)page.Count);

        public void Add(TKey key, TValue value)
        {
            int pageIndex = GetPageIndex(key);

            if (!Pages.TryGetValue(pageIndex, out var page))
            {
                page = new Dictionary<TKey, TValue>(PageSize / 1000);
                Pages[pageIndex] = page;
            }

            page.Add(key, value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            Pages.Clear();
        }


        public bool ContainsKey(TKey key)
        {
            int pageIndex = GetPageIndex(key);
            if (Pages.TryGetValue(pageIndex, out var page))
            {
                return page.ContainsKey(key);
            }
            return false;
        }


        public bool Remove(TKey key)
        {
            int pageIndex = GetPageIndex(key);
            if (Pages.TryGetValue(pageIndex, out var page))
            {
                return page.Remove(key);
            }
            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            int pageIndex = GetPageIndex(key);
            value = default;

            if (Pages.TryGetValue(pageIndex, out var page))
            {
                return page.TryGetValue(key, out value);
            }
            return false;
        }

        private static int GetPageIndex(TKey key)
        {
            int hashCode = (key.GetHashCode() & 0x7fffffff) / PageSize;
            return hashCode;
        }
    }
}
