using System;

namespace Z4Net.Dto.Attributes
{
    /// <summary>
    /// Description the static class to use to process a response recevied from Z node.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal class DataReceivedAttribute : Attribute
    {

        /// <summary>
        /// Class type.
        /// </summary>
        public Type ClassType { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="classType"></param>
        public DataReceivedAttribute(Type classType)
        {
            ClassType = classType;
        }

    }
}
