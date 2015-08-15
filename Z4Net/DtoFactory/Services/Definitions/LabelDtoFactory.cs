using System.Collections.Generic;
using System.Linq;
using Technical;
using Z4Net.Dto.Services;
using Z4Net.Dto.Services.Definitions;

namespace Z4Net.DtoFactory.Services.Definitions
{
    /// <summary>
    /// Label factory.
    /// </summary>
    public class LabelDtoFactory : BaseFactory<ServicesContext>
    {

        #region Public methods

        /// <summary>
        /// Get a list of label.
        /// </summary>
        /// <param name="identifiers">Label global identifiers.</param>
        /// <param name="language">Language to select.</param>
        /// <returns>Label list.</returns>
        public List<LabelDto> Get(string language, params int[] identifiers)
        {
            var sel = from p in Context.Labels
                where
                    identifiers.Contains(p.LabelIdentifier) &&
                    p.Language == language
                select p;

            return sel.ToListEx();
        }
        /// <summary>
        /// Get a label.
        /// </summary>
        /// <param name="identifier">Label global identifier.</param>
        /// <param name="language">Language to select.</param>
        /// <returns>Label.</returns>
        public LabelDto Get(string language, int identifier)
        {
            var sel = from p in Context.Labels
                      where
                          p.LabelIdentifier == identifier &&
                          p.Language == language
                      select p;

            return sel.FirstOrDefaultEx();
        }


        #endregion

    }
}
