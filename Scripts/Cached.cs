using System;
using UnityEngine;

namespace Grayscale {
    public class Cached<T> {
        private readonly WeakReference<CacheStorage<T>> _weakCacheStorage;
        private readonly Hash128 _hash;
        public readonly T Value;

        public static implicit operator T(Cached<T> cached) {
            return cached.Value;
        }

        public void Drop() {
            if (_weakCacheStorage.TryGetTarget(out CacheStorage<T> cache)) {
                cache.Remove(_hash);
            }
        }

        internal Cached(CacheStorage<T> storage, Hash128 hash, T value) {
            _weakCacheStorage = new WeakReference<CacheStorage<T>>(storage);
            _hash = hash;
            Value = value;
        }
    }
}
