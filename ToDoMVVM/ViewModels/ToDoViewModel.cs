using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ToDoMVVM.Models;
using Realms;

namespace ToDoMVVM.ViewModels
{
	public class ToDoViewModel : INotifyPropertyChanged
	{
		private Realm realm;
		public ObservableCollection<ToDoItem> Items { get; set; }
		public ToDoViewModel()
		{
			realm = Realm.GetInstance();
			var items = realm.All<ToDoItem>();
			Items = new ObservableCollection<ToDoItem>(items);
			AddItemCommand = new Command(AddItem);
			DeleteItemCommand = new Command<ToDoItem>(DeleteItem);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
		{
			if (EqualityComparer<T>.Default.Equals(backingStore, value))
				return false;

			backingStore = value;
			onChanged?.Invoke();
			OnPropertyChanged(propertyName);
			Console.WriteLine($"Property {propertyName} has been updated.");
			return true;
		}

		private string newItemTitle;
		public string NewItemTitle
		{
			get => newItemTitle;
			set => SetProperty(ref newItemTitle, value);
		}

		public ICommand AddItemCommand { get; }
    	public ICommand DeleteItemCommand { get; }

		private void AddItem()
		{
			if (!string.IsNullOrWhiteSpace(NewItemTitle))
			{
				var newItem = new ToDoItem { Title = NewItemTitle, IsCompleted = false };
				realm.Write(() =>
				{
					realm.Add(newItem);
				});
				Items.Add(newItem);
				NewItemTitle = string.Empty; // Reset the input field
			}
		}

		private void DeleteItem(ToDoItem item)
		{
			if (item != null)
			{
				realm.Write(() =>
				{
					realm.Remove(item);
				});
				Items.Remove(item);
			}
		}
	}
}

