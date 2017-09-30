using System;




namespace neo {

    class RefWrapper<T> {
        private Func<T> _get;
        private Action<T> _set;

        public RefWrapper(Func<T> @get, Action<T> @set) => Bind(@get, @set);
        public T Value {
            get => _get();
            set => _set(value);
        }
        public void Bind(Func<T> @get, Action<T> @set) {
            _get = @get;
            _set = @set;
        }
    }
    class StringAsT<T> {
        private RefWrapper<string> wrap;

        public StringAsT(Func<string> @get, Action<string> @set) => wrap = new RefWrapper<string>(@get, @set);
        public T Value {
            get => (T)Convert.ChangeType(wrap.Value, typeof(T));
            set => wrap.Value = value.ToString();
        }
        public void Bind(Func<string> @get, Action<string> @set) => wrap.Bind(@get, @set);
    }
    class WrapperT<T_Stored, T_Simulated> {
        private RefWrapper<T_Stored> wrap;
        private Func<T_Stored, T_Simulated> _get;
        private Func<T_Simulated, T_Stored> _set;

        public WrapperT(Func<T_Stored, T_Simulated> @get_converter, Func<T_Simulated, T_Stored> @set_converter, Func<T_Stored> @get, Action<T_Stored> @set) {
            _get = @get_converter;
            _set = @set_converter;
            wrap = new RefWrapper<T_Stored>(@get, @set);
        }
        public T_Simulated Value {
            get => _get(wrap.Value);
            set => wrap.Value = _set(value);
        }
        public void Bind(Func<T_Stored> @get, Action<T_Stored> @set) => wrap.Bind(@get, @set);
    }

}

