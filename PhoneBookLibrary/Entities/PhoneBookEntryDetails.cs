using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PhoneBookLibrary
{
    /// <summary>
    ///  Phone Book Entry Details Class 
    /// <list>
    /// <item><em>Number Type</em></item>
    /// <item><em>Phone Number</em></item>
    /// </list>
    /// </summary>
    /// 
    [Serializable]
    public class PhoneBookEntryDetails
    {
        /// <summary>
        ///  Number typ Property
        ///  <list>
        /// <item><em>Work</em></item>
        /// <item><em> Cellphone </em></item>
        /// <item><em> Home</em></item>
        /// </list>
        /// </summary>
        [Required(ErrorMessage = "Number type parameter is required")]
        public string Type { get; set; }

        /// <summary>
        ///  Phone Number Property
        /// </summary>
        [Required(ErrorMessage = "Number parameter is required")]
        public string Number { get; set; }
    }
}
