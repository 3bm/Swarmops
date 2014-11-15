﻿using System;
using System.IO;
using System.Text;
using NBitcoin;
using Swarmops.Logic.Support;

namespace Swarmops.Frontend.Controls.v5.UI
{
    public partial class IncludedScripts : ControlV5Base
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder scriptRef = new StringBuilder();

            string[] controlNames = Controls.Split(',');
            foreach (string controlName in controlNames)
            {
                switch (controlName.Trim().ToLowerInvariant())
                {
                    case "fileupload":
                        scriptRef.Append(
                            "<script src=\"/Scripts/jquery.fileupload/jquery.iframe-transport.js\" type=\"text/javascript\" language=\"javascript\"></script>\r\n" +
                            "<script src=\"/Scripts/jquery.fileupload/jquery.fileupload.js\" type=\"text/javascript\" language=\"javascript\"></script>\r\n");
                        break;

                    case "switchbutton":
                        scriptRef.Append(
                            "<script src=\"/Scripts/jquery.switchButton.js\" language=\"javascript\" type=\"text/javascript\"></script>\r\n" +
                            "<link rel=\"stylesheet\" type=\"text/css\" href=\"/Style/jquery.switchButton.css\" />\r\n");
                        break;
                    default:
                        break;
                }
            }

            this.LiteralReference.Text = scriptRef.ToString();
        }

        public new string Controls { get; set; }
    }
}