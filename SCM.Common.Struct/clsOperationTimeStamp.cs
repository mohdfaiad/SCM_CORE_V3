using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCM.Common.Struct
{
    public class clsOperationTimeStamp
    {
        private string _strAWBPrefix;
        private string _strAWBNo;
        private string _strOperationalType;
        private string _strOperationalStatus;
        private string _strDescription;
        private string _strStation;
        private string _strOperationDate;
        private string _strOperationTime;
        private string _strFlightNo;
        private DateTime _dtFlightDt;
        private string _strULDNo;
        private int _intPieces;
        private decimal _dcWeight;
        private string _strUpdatedBy;
        private DateTime _dtUpdatedOn;

        public int Pieces
        {
            set
            {
                _intPieces = value;
            }
            get
            {
                return _intPieces;
            }
        }

        public decimal Weight
        {
            set
            {
                _dcWeight = value;
            }
            get
            {
                return _dcWeight;
            }
        }

        public string UpdatedBy
        {
            set
            {
                _strUpdatedBy = value;
            }
            get
            {
                return _strUpdatedBy;
            }
        }

        public DateTime UpdatedOn
        {
            set
            {
                _dtUpdatedOn = value;
            }
            get
            {
                return _dtUpdatedOn;
            }
        }
        
        public string AWBPrefix
        {
            set
            {
                _strAWBPrefix = value;
            }
            get
            {
                return _strAWBPrefix;
            }
        }

        public string AWBNumber
        {
            set
            {
                _strAWBNo = value;
            }
            get
            {
                return _strAWBNo;
            }
        }

        public string OperationalType
        {
            set
            {
                _strOperationalType = value;
            }
            get
            {
                return _strOperationalType;
            }
        }

        public string OperationalStatus
        {
            set
            {
                _strOperationalStatus = value;
            }
            get
            {
                return _strOperationalStatus;
            }
        }

        public string Description
        {
            set
            {
                _strDescription = value;
            }
            get
            {
                return _strDescription;
            }
        }

        public string Station
        {
            set
            {
                _strStation = value;
            }
            get
            {
                return _strStation;
            }
        }

        public string OperationDate
        {
            set
            {
                _strOperationDate = value;
            }
            get
            {
                return _strOperationDate;
            }
        }

        public string OperationTime
        {
            set
            {
                _strOperationTime = value;
            }
            get
            {
                return _strOperationTime;
            }
        }

        public string FlightNo
        {
            set
            {
                _strFlightNo = value;
            }
            get
            {
                return _strFlightNo;
            }
        }

        public DateTime FlightDt
        {
            set
            {
                _dtFlightDt = value;
            }
            get
            {
                return _dtFlightDt;
            }
        }

        public string ULDNumber
        {
            set
            {
                _strULDNo = value;
            }
            get
            {
                return _strULDNo;
            }
        }
    }
}
