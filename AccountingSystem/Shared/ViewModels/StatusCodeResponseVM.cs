using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Shared.ViewModels
{
    /// <summary>
    /// StatusCodeResponseVM
    /// </summary>
    [Serializable]
    public class StatusCodeResponseVM
    {
        /// <summary>
        /// HttpStatus
        /// </summary>
        public int HttpStatus { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Detail
        /// </summary>
        public string Detail { get; set; }
    }
}
