using System;
using MongoDB.Bson;
using Realms;

namespace ToDoMVVM.Models
{
	public class ToDoItem : RealmObject
    {
		[PrimaryKey]
		public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
		public string Title { get; set; }
		public bool IsCompleted { get; set; }
	}
}

