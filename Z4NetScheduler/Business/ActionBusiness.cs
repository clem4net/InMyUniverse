
using System.Linq;
using Z4Net;
using Z4NetScheduler.Dto;

namespace Z4NetScheduler.Business
{
    /// <summary>
    /// Action business.
    /// </summary>
    internal static class ActionBusiness
    {

        #region Internal methods

        /// <summary>
        /// Execute an action.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <returns>Process result.</returns>
        internal static bool Execute(ActionDto action)
        {
            ZService service = new ZService();
            var allLodes = service.GetNodes();
            var node = allLodes.FirstOrDefault(x => x.HomeIdentifier == action.HomeIdentifier && x.ZIdentifier == action.ZIdentifier);
            node.ValueProxy = action.Value;
            return service.Set(node);
        }

        #endregion

    }
}
