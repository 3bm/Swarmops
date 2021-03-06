﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Swarmops.Logic.Communications;
using Swarmops.Logic.Communications.Payload;
using Swarmops.Logic.Financial;
using Swarmops.Logic.Swarm;

namespace Swarmops.Frontend.Automation
{
    public partial class FinancialFunctions : DataV5Base
    {
        protected void Page_Load (object sender, EventArgs e)
        {
            // do nothing
        }

        [WebMethod]
        public static AjaxCallResult SetBitcoinPayoutAddress(string bitcoinAddress)
        {
            AuthenticationData authData = GetAuthenticationDataAndCulture();

            if (authData == null)
            {
                throw new UnauthorizedAccessException();
            }

            // Remove whitespace from submitted address (whitespace will probably be entered in some cases)

            bitcoinAddress = bitcoinAddress.Replace (" ", string.Empty);

            if (string.IsNullOrEmpty (authData.CurrentUser.BitcoinPayoutAddress))
            {
                if (!BitcoinUtility.IsValidBitcoinAddress (bitcoinAddress))
                {
                    return new AjaxCallResult {Success = false, DisplayMessage = "Invalid address"};
                }

                authData.CurrentUser.BitcoinPayoutAddress = bitcoinAddress;
                authData.CurrentUser.BitcoinPayoutAddressTimeSet = DateTime.UtcNow.ToString (CultureInfo.InvariantCulture);

                // TODO: Create notifications for CEO and for user

                NotificationCustomStrings strings = new NotificationCustomStrings();
                strings["BitcoinAddress"] = bitcoinAddress;

                OutboundComm userNotify = OutboundComm.CreateNotification (authData.CurrentOrganization,
                    "BitcoinPayoutAddress_Set", strings, People.FromSingle (authData.CurrentUser));

                strings["ConcernedPersonName"] = authData.CurrentUser.Canonical;

                OutboundComm adminNotify = OutboundComm.CreateNotification (authData.CurrentOrganization,
                    "BitcoinPayoutAddress_Set_OfficerNotify", strings); // will send to admins of org as no people specified

                return new AjaxCallResult {Success = true};
            }
            else
            {
                // If the address is already set

                return new AjaxCallResult {Success = false, DisplayMessage = "Address already set"};
            }
        }

        [WebMethod]
        public static InterpretedCurrencyResult InterpretCurrency(string input)
        {
            AuthenticationData authData = GetAuthenticationDataAndCulture();

            string[] currencyStrings = input.Split(' ');
            if (currencyStrings.Length != 2)
            {
                return new InterpretedCurrencyResult {Success = false, DisplayMessage="Cannot interpret field"};
            }

            Currency currencyUsed = null;

            try
            {
                currencyUsed = Currency.FromCode(currencyStrings[0].ToUpperInvariant());
            }
            catch (Exception)
            {
                return new InterpretedCurrencyResult { Success = false, DisplayMessage = "Cannot interpret currency" };
            }

            Int64 amountCents = 0;
            double amountDouble = 0.0;
            bool amountInterpreted = false;

            if (Double.TryParse(currencyStrings[1], NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out amountDouble))
            {
                amountInterpreted = true;
            }
            else if (Double.TryParse(currencyStrings[1], NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out amountDouble))
            {
                amountInterpreted = true;
            }

            if (!amountInterpreted)
            {
                return new InterpretedCurrencyResult { Success = false, DisplayMessage = "Cannot interpret amount" };
            }

            Money money = new Money((long) (amountDouble * 100.0 + 0.5), currencyUsed);

            return new InterpretedCurrencyResult
            {
                Success = true,
                EnteredAmount = amountDouble.ToString("N2"),
                CurrencyCode = money.Currency.Code,
                DisplayAmount = (money.ToCurrency(authData.CurrentOrganization.Currency).Cents/100.0).ToString("N2")
            };
        }


        public class InterpretedCurrencyResult : AjaxCallResult
        {
            public string CurrencyCode { get; set; }
            public string EnteredAmount { get; set; }
            public string DisplayAmount { get; set; }
        }
    }
}