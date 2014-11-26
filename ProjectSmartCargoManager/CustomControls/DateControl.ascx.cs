using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace ProjectSmartCargoManager.UserControls
{
    public partial class DateControl : System.Web.UI.UserControl
    {
        #region "Variable Declaration"

        private DateTime _dtDateValue;
        private bool _blnCurrentDate;
        private string _strDateFormat;
        private string _strDateFormatDDMMYYYY;
        private string _strDateFormatMMDDYYYY;
        private string _strWidth;
        private bool _blnEnable = true;
        private bool _blnFocus;
        public event EventHandler TextChange;

        #endregion "Variable Declaration"

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["DateFormat"] != null)
            {
                _strDateFormat = Convert.ToString(Session["DateFormat"]);                
                extDate.Format = _strDateFormat;

                if (Width != "")
                    txtDate.Width = new Unit(Width);
            }

            if (!IsPostBack)
            {
                if (SetCurrentDate)
                {
                    DateTime dt = Convert.ToDateTime(Session["IT"]);
                    txtDate.Text = dt.ToString(_strDateFormat);
                }

                txtDate.Enabled = this.Enabled;
                imgDate.Enabled = this.Enabled;
            }
        }

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            if (TextChange != null)
            {
                TextChange(sender, e);
            }

            if (Session["DateFormat"] != null)
                _strDateFormat = Convert.ToString(Session["DateFormat"]);

            try
            {
                DateValue = DateTime.ParseExact(txtDate.Text.Trim(), _strDateFormat, null);       
            }
            catch
            {                
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Date Format", "alert('Please enter valid date.');", true);
            }
        }

        public DateTime DateValue
        {
            set
            {
                _dtDateValue = value;
                SetDate();
            }
            get
            {
                if (_dtDateValue != DateTime.MinValue)
                    return _dtDateValue;
                else
                {
                    return GetDateValue();
                }
            }
        }

        public string DateFormatDDMMYYYY
        {
            set
            {
                _strDateFormatDDMMYYYY = value;
                DateValue = DateTime.ParseExact(_strDateFormatDDMMYYYY, "dd/MM/yyyy", null);                
                SetDate();                
            }
            get
            {
                if (_strDateFormatDDMMYYYY == "" || _strDateFormatDDMMYYYY == null)
                    return GetDateValue().ToString("dd/MM/yyyy");                    
                else                
                    return _strDateFormatDDMMYYYY;                
            }
        }

        public string DateFormatMMDDYYYY
        {
            set
            {
                _strDateFormatMMDDYYYY = value;
                DateValue = DateTime.ParseExact(_strDateFormatMMDDYYYY, "MM/dd/yyyy", null);
                SetDate();                
            }
            get
            {
                if (_strDateFormatMMDDYYYY == "" || _strDateFormatMMDDYYYY == null)
                    return GetDateValue().ToString("MM/dd/yyyy");                    
                else
                    return _strDateFormatMMDDYYYY;               
            }
        }

        public bool SetCurrentDate
        {
            set
            {
                _blnCurrentDate = value;
            }
            get
            {
                return _blnCurrentDate;
            }
        }

        public string Width
        {
            set
            {
                _strWidth = value;
            }
            get
            {
                return _strWidth;
            }
        }

        private void SetDate()
        {
            try
            {
                if (Session["DateFormat"] != null)
                    _strDateFormat = Convert.ToString(Session["DateFormat"]);

                txtDate.TextChanged -= new EventHandler(txtDate_TextChanged);
                txtDate.Text = DateValue.ToString(_strDateFormat);
                txtDate.TextChanged += new EventHandler(txtDate_TextChanged);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Date Format", "alert('Please enter valid date.');", true);
            }
        }

        public string Text
        {
            get
            {
                return txtDate.Text.Trim();
            }
        }

        public bool Enabled
        {
            set
            {
                _blnEnable = value;                
            }
            get
            {
                return _blnEnable;
            }
        }

        public bool Focus
        {
            set
            {
                _blnFocus = value;
                if (_blnFocus)
                    txtDate.Focus();
            }
            get
            {
                return _blnFocus;
            }
        }

        private DateTime GetDateValue()
        {
            if(txtDate.Text.Trim()!="")
            {
                DateTime dt = DateTime.ParseExact(txtDate.Text.Trim(),Convert.ToString(Session["DateFormat"]),null);
                return dt;
            }
            return DateTime.MinValue;
        }

        public string userCtlClientId
        {
            get { return this.ClientID; }
        }
    }
}