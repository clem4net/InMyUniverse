using System.Collections.Generic;
using System.Linq;
using Z4Net.Dto.Services.Definitions;
using Z4Net.DtoFactory.Services.Definitions;

namespace Z4Net.Business.Services.Definitions
{
    /// <summary>
    /// Label business.
    /// </summary>
    internal static class LabelBusiness
    {

        #region Internal properties

        /// <summary>
        /// Language to use.
        /// </summary>
        internal static string Language { get; set; } = "FR-fr";

        #endregion

        #region Internal methods

        /// <summary>
        /// Get a label.
        /// </summary>
        /// <param name="identifier">Label global identifier.</param>
        /// <returns>Label.</returns>
        internal static string Get(int identifier)
        {
            LabelDto result;
            using (var ctx = new LabelDtoFactory())
            {
                result = ctx.Get(Language, identifier);
            }

            return result?.Value;
        }

        /// <summary>
        /// Get a list of label.
        /// </summary>
        /// <param name="identifiers">Label global identifiers.</param>
        /// <returns>Label list.</returns>
        internal static Dictionary<int, string> Get(params int[] identifiers)
        {
            List<LabelDto> labels;
            using (var ctx = new LabelDtoFactory())
            {
                labels = ctx.Get(Language, identifiers);
            }

            var result = new Dictionary<int, string>();
            foreach (var id in identifiers)
            {
                var label = labels.FirstOrDefault(x => x.LabelIdentifier == id);
                if (result.ContainsKey(id) == false)
                {
                    result.Add(id, label?.Value ?? string.Empty);
                }
            }

            return result;
        }

        #endregion

    }
}
