namespace Toolkit.ComponentModel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Provides utility methods that help to raise <see cref="INotifyPropertyChanged.PropertyChanged"/> event
    /// for classes that implement <see cref="INotifyPropertyChanged"/> interface.
    /// </summary>
    public static class PropertyManager
    {
        private static readonly Dictionary<string, PropertyChangedEventArgs> propertyChangedEventArgsCache = new Dictionary<string, PropertyChangedEventArgs>();

        /// <summary>
        /// Raises <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="eventHandler">The event delegate.</param>
        /// <param name="propertyName">The property name.</param>
        public static void RaisePropertyChanged(object sender, PropertyChangedEventHandler eventHandler, string propertyName)
        {
            RaisePropertyChanged(sender, eventHandler, GetPropertyChangedEventArgs(sender, propertyName));
        }

        /// <summary>
        /// Raises <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="T">The property type.</typeparam>
        /// <param name="sender">The source of the event.</param>
        /// <param name="eventHandler">The event delegate.</param>
        /// <param name="propertyExpression">The strongly typed lambda expression of the property.</param>
        public static void RaisePropertyChanged<T>(object sender, PropertyChangedEventHandler eventHandler, Expression<Func<T>> propertyExpression)
        {
            RaisePropertyChanged(sender, eventHandler, GetPropertyChangedEventArgs(sender, propertyExpression));
        }

        /// <summary>
        /// Raises <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="eventHandler">The event delegate.</param>
        /// <param name="eventArgs">A <see cref="PropertyChangedEventArgs"/> that contains the event data.</param>
        public static void RaisePropertyChanged(object sender, PropertyChangedEventHandler eventHandler, PropertyChangedEventArgs eventArgs)
        {
            if (eventHandler != null)
                eventHandler(sender, eventArgs);
        }

        /// <summary>
        /// Extracts or creates a cached copy of <see cref="PropertyChangedEventArgs"/> for the property.
        /// </summary>
        /// <param name="object">The instance of a class that defines the property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>A cached copy of <see cref="PropertyChangedEventArgs"/>.</returns>
        public static PropertyChangedEventArgs GetPropertyChangedEventArgs(object @object, string propertyName)
        {
            if (propertyName == null)
                throw new ArgumentNullException("propertyName");

            VerifyProperty(@object, propertyName);

            PropertyChangedEventArgs eventArgs = null;

            if (!propertyChangedEventArgsCache.TryGetValue(propertyName, out eventArgs))
            {
                lock (((ICollection)propertyChangedEventArgsCache).SyncRoot)
                {
                    if (!propertyChangedEventArgsCache.TryGetValue(propertyName, out eventArgs))
                    {
                        eventArgs = new PropertyChangedEventArgs(propertyName);
                        propertyChangedEventArgsCache.Add(propertyName, eventArgs);
                    }
                }
            }

            return eventArgs;
        }

        /// <summary>
        /// Extracts or creates a cached copy of <see cref="PropertyChangedEventArgs"/> for the property.
        /// </summary>
        /// <typeparam name="T">The property type.</typeparam>
        /// <param name="object">The instance of a class that defines the property.</param>
        /// <param name="propertyExpression">The strongly typed lambda expression of the property.</param>
        /// <returns>A cached copy of <see cref="PropertyChangedEventArgs"/>.</returns>
        public static PropertyChangedEventArgs GetPropertyChangedEventArgs<T>(object @object, Expression<Func<T>> propertyExpression)
        {
            return GetPropertyChangedEventArgs(@object, GetPropertyName(propertyExpression));
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <param name="propertyExpression">The lambda expression of the property.</param>
        /// <returns>The name of the property</returns>
        public static string GetPropertyName(LambdaExpression propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException("propertyExpression");

            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                var unaryExpression = propertyExpression.Body as UnaryExpression;
                if (unaryExpression == null)
                    throw new ArgumentException("propertyExpression");

                memberExpression = unaryExpression.Operand as MemberExpression;
                if (memberExpression == null)
                    throw new ArgumentException("propertyExpression");
            }
            return memberExpression.Member.Name;
        }

        /// <summary>
        /// Verifies that the property of the specified object is public instance member.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <param name="propertyName">Name of the property.</param>
        [Conditional("DEBUG")]
        private static void VerifyProperty(object @object, string propertyName)
        {
            if (!String.IsNullOrWhiteSpace(propertyName))
            {
                var type = @object.GetType().GetTypeInfo();

                var propertyInfo = type.GetDeclaredProperty(propertyName);
                Debug.Assert(propertyInfo != null, String.Format("{0} is not a public property of {1}", propertyName, type.FullName));
            }
        }
    }
}