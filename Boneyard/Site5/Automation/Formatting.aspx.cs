﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Swarmops.Frontend.Automation
{
    public partial class Formatting : DataV5Base
    {
        protected void Page_Load (object sender, EventArgs e)
        {
            // This page exists only for its web methods, which are called by Ajax.
        }

        /// <summary>
        /// Returns a culturally-formatted integer with thousands separators.
        /// </summary>
        /// <param name="input">The integer to format.</param>
        /// <returns>The formatted integer for display.</returns>
        [WebMethod]
        public static string FormatInteger(int input)
        {
            GetAuthenticationDataAndCulture();
            return input.ToString("N0");
        }

        /// <summary>
        /// Returns a culturally-formatted double with thousands separators and two decimals.
        /// </summary>
        /// <param name="input">The double to format.</param>
        /// <returns>The formatted double for display.</returns>
        [WebMethod]
        public static string FormatCurrency(double input)
        {
            GetAuthenticationDataAndCulture();
            return input.ToString("N2");
        }

    }
}