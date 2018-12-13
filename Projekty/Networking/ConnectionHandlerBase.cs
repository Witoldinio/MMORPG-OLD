using System.Reflection;

namespace Networking
{
    public abstract class ConnectionHandlerBase<T>
    {
        /// <summary>
        /// Handling Unknown Packages
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="parsedData"></param>
        /// <param name="type"></param>
        protected abstract void HandleUnknownPackage(T connection, object parsedData, PackageType type);

        public void InvokeAction(T connection, object parsedData, PackageType type)
        {
            foreach (var meth in GetType().GetMethods())
            {
                var attribute = meth.GetCustomAttribute<PackageHandlerAttribute>();
                if (null == attribute)
                {
                    continue;
                }

                if (type == attribute.Type)
                {
                    meth.Invoke(this, new object[] { connection, parsedData });
                    return;
                }

                HandleUnknownPackage(connection, parsedData, type);
            }
        }
    }
}
