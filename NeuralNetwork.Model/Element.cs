using NeuralNetwork.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NeuralNetwork.Model
{
   public abstract class Element : IEquatable<Element>, INotifyPropertyChanged
   {
      private string _id;
      private object _object;

      public event PropertyChangedEventHandler PropertyChanged;

      internal Element(string id)
      {
         _id = id;
         ValidateId();
      }

      public string Id
      {
         get => _id;
         set { ChangeProperty(ref _id, value); }
      }

      public object Object
      {
         get => _object;
         set { ChangeProperty(ref _object, value); }
      }

      internal void ValidateId(IDictionary<string, Element> acumulatedIds)
      {
         ValidateId();

         if (acumulatedIds == null)
            throw new ArgumentNullException($"Internal Error: {nameof(acumulatedIds)} is null.");

         if (acumulatedIds.ContainsKey(this.Id))
            throw new DuplicatedIdException(this.Id);

         acumulatedIds.Add(this.Id, this);

         ValidateDuplicatedIChild(acumulatedIds);
      }

      private void ValidateId()
      {
         if (string.IsNullOrWhiteSpace(this.Id))
            throw new InvalidIdException(this.Id);
      }

      private protected void ChangeProperty<T>(ref T prop, T newValue, [CallerMemberName] string propertyName = null)
      {
         prop = newValue;
         FireChanges(this, propertyName);
      }

      private protected void FireChanges(object sender, string properyName)
      {
         PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(properyName));
      }

      private protected void FireChanges(string properyName)
      {
         FireChanges(this, properyName);
      }

      private protected virtual void ValidateDuplicatedIChild(IDictionary<string, Element> acumulatedIds)
      {

      }

      public override bool Equals(object obj)
      {
         return this.Equals(obj as Element);
      }

      public bool Equals(Element other)
      {
         return !(other is null) && (Object.ReferenceEquals(this, other) || this.Id == other.Id);
      }

      public override int GetHashCode()
      {
         return this.Id.GetHashCode();
      }

      public override string ToString()
      {
         return this.Id;
      }

      public static bool operator ==(Element b1, Element b2) => Object.Equals(b1, b2);
      public static bool operator !=(Element b1, Element b2) => !Object.Equals(b1, b2);
   }
}
