using System;
using System.Reflection;

namespace Technical
{
    /// <summary>
    /// Reflection helper.
    /// </summary>
    public static class ReflectionHelper
    {

        #region Public methods

        /// <summary>
        /// Execute a method of a static class.
        /// </summary>
        /// <param name="type">Static class type.</param>
        /// <param name="name">Method name.</param>
        /// <param name="arguments">Arguments of the method.</param>
        public static T ExecuteStaticMethod<T>(Type type, string name, params object[] arguments)
        {
            T result;
            try
            {
                var method = type.GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic);
                var objResult =  method?.Invoke(null, arguments);
                if (objResult != null)
                {
                    result = (T) objResult;
                }
                else
                {
                    result = default(T);
                }
            }
            catch
            {
                result = default(T);
            }

            return result;
        }

        /// <summary>
        /// Create an instance of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Result type.</typeparam>
        /// <param name="type">Type to create.</param>
        /// <returns>Instance of <typeparamref name="T"/></returns>
        public static T CreateInstance<T>(Type type)
        {
            T result;

            try
            {
                result = (T) Activator.CreateInstance(type);
            }
            catch
            {
                result = default(T);
            }

            return result;
        }

        /// <summary>
        /// Get the description of a enumeration value.
        /// </summary>
        /// <typeparam name="TR">Result type.</typeparam>
        /// <typeparam name="T">Enumeration type.</typeparam>
        /// <param name="enumerationValue">Value of the T enumeration.</param>
        /// <returns>Attribute.</returns>
        public static TR GetEnumValueAttribute<T, TR>(T enumerationValue) where T : struct
        {
            TR result;

            try
            {
                var type = typeof (T);

                var memberInfo = type.GetMember(enumerationValue.ToString());
                if (memberInfo.Length > 0)
                {
                    var descriptionAttribute = memberInfo[0].GetCustomAttributes(typeof (TR), false);
                    if (descriptionAttribute.Length > 0)
                    {
                        result = (TR) descriptionAttribute[0];
                    }
                    else
                    {
                        result = default(TR);
                    }
                }
                else
                {
                    result = default(TR);
                }
            }
            catch
            {
                result = default(TR);
            }

            return result;
        }

        #endregion

    }
}
