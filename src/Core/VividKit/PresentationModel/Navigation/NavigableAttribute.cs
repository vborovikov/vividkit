namespace Toolkit.PresentationModel.Navigation
{
    using System;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Specifies the page URI associated with an alias (target view name).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class NavigableAttribute : Attribute
    {
        /// <summary>
        /// The default target name.
        /// </summary>
        public const string DefaultTarget = "default";

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigableAttribute"/> class.
        /// </summary>
        /// <param name="contentUri">The content URI.</param>
        public NavigableAttribute(string contentUri)
            : this()
        {
            this.ContentUri = contentUri;
        }

        public NavigableAttribute(Type contentType)
            : this()
        {
            this.ContentType = contentType;
        }

        private NavigableAttribute()
        {
            this.Target = DefaultTarget;
        }

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        public Type ContentType { get; set; }

        /// <summary>
        /// Gets or sets the page URI.
        /// </summary>
        /// <value>The page URI.</value>
        public string ContentUri { get; set; }

        /// <summary>
        /// Gets or sets the target view name used for navigation.
        /// </summary>
        /// <value>
        /// The view name.
        /// </value>
        public string Target { get; set; }

        /// <summary>
        /// Gets or sets the type of the URI builder.
        /// </summary>
        /// <value>
        /// The type of the URI builder.
        /// </value>
        public Type UriBuilderType { get; set; }

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="navigableType">Type of the navigable instance.</param>
        /// <returns>Type of the view.</returns>
        public static Type GetContentType(string target, Type navigableType)
        {
            var navigableAttr = GetNavigableAttribute(navigableType, target);
            return navigableAttr.ContentType;
        }

        /// <summary>
        /// Gets the page URI by a target view name.
        /// </summary>
        /// <param name="target">The target view name.</param>
        /// <param name="navigableType">Type of the navigable instance.</param>
        /// <param name="extraData">The extra data.</param>
        /// <returns></returns>
        public static Uri GetContentUri(string target, Type navigableType, object extraData = null)
        {
            var navigableAttr = GetNavigableAttribute(navigableType, target);
            var contentUri = navigableAttr.ContentUri;

            if (navigableAttr.UriBuilderType != null)
            {
                var uriBuilder = Activator.CreateInstance(navigableAttr.UriBuilderType) as IUriBuilder;
                if (uriBuilder != null)
                {
                    contentUri = uriBuilder.BuildUri(contentUri, extraData);
                }
            }

            return new Uri(contentUri, UriKind.RelativeOrAbsolute);
        }

        private static NavigableAttribute GetNavigableAttribute(Type navigatorType, string target)
        {
            var navigableAttr = navigatorType.GetTypeInfo()
                .GetCustomAttributes(typeof(NavigableAttribute), inherit: true)
                .Cast<NavigableAttribute>()
                .SingleOrDefault(attr => attr.Target == target);

            if (navigableAttr == null)
                throw new InvalidOperationException();

            return navigableAttr;
        }
    }
}