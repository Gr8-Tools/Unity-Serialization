namespace Tests.Runtime.Utils.Singletons {
    internal abstract class SingletonService<T> where T : new() {
        internal static T Instance {
            get {
                if(_instance == null) {
                    _instance = new T();
                }
                return _instance;
            }
        }

        private static T _instance;
    }
}