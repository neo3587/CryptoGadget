using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;



namespace CryptoGadget {

	public class PropManager<T> : INotifyPropertyChanged {
		public static PropertyInfo[] GetProps() {
			return typeof(T).GetProperties().Where(p => p.GetIndexParameters().Length == 0 &&
													(p.GetCustomAttribute<PropManager>() == null || p.GetCustomAttribute<PropManager>().Ignore == false)
												  ).ToArray();
		}
		public object this[string prop] {
			get { return GetType().GetProperty(prop).GetValue(this, null); }
			set { GetType().GetProperty(prop).SetValue(this, value, null); }
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "") {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public sealed class PropManager : Attribute {
		public bool Ignore { get; set; } = false;
	}

}
