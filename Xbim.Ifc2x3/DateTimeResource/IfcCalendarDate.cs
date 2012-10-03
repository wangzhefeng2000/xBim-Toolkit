﻿#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcCalendarDate.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System;
using System.ComponentModel;
using Xbim.XbimExtensions.SelectTypes;
using Xbim.XbimExtensions;
using Xbim.XbimExtensions.Interfaces;

#endregion

namespace Xbim.Ifc2x3.DateTimeResource
{
    /// <summary>
    ///   A date which is defined by a day in a month of a year.
    /// </summary>
    /// <remarks>
    ///   Definition from ISO/CD 10303-41:1992: A date which is defined by a day in a month of a year. 
    ///   NOTE Corresponding STEP name: calendar_date, please refer to ISO/IS 10303-41:1994 for the final definition of the formal standard. 
    ///   HISTORY New entity in IFC Release 1.5.1.
    /// </remarks>
    [IfcPersistedEntityAttribute, Serializable]
    public class IfcCalendarDate : IfcDateTimeSelect, INotifyPropertyChanged, ISupportChangeNotification,
                                   IPersistIfcEntity, IfcObjectReferenceSelect, INotifyPropertyChanging
    {

        #region IPersistIfcEntity Members

        private int _entityLabel;
        private IModel _model;

        public IModel ModelOf
        {
            get { return _model; }
        }

        void IPersistIfcEntity.Bind(IModel model, int entityLabel)
        {
            _model = model;
            _entityLabel = entityLabel;
        }

        bool IPersistIfcEntity.Activated
        {
            get { return _entityLabel > 0; }
        }

        public int EntityLabel
        {
            get { return _entityLabel; }
        }

        void IPersistIfcEntity.Activate(bool write)
        {
            if (_model != null && _entityLabel <= 0) _entityLabel = _model.Activate(this, false);
            if (write) _model.Activate(this, write);
        }

        #endregion

        private IfcDayInMonthNumber _dayComponent;
        private IfcMonthInYearNumber _monthComponent;
        private IfcYearNumber _yearComponent;

        /// <summary>
        ///   The day element of the calendar date.
        /// </summary>
        [IfcAttribute(1, IfcAttributeState.Mandatory)]
        public IfcDayInMonthNumber DayComponent
        {
            get
            {
                ((IPersistIfcEntity) this).Activate(false);
                return _dayComponent;
            }
            set { this.SetModelValue(this, ref _dayComponent, value, v => DayComponent = v, "DayComponent"); }
        }

        /// <summary>
        ///   The month element of the calendar date.
        /// </summary>
        [IfcAttribute(2, IfcAttributeState.Mandatory)]
        public IfcMonthInYearNumber MonthComponent
        {
            get
            {
                ((IPersistIfcEntity) this).Activate(false);
                return _monthComponent;
            }
            set { this.SetModelValue(this, ref _monthComponent, value, v => MonthComponent = v, "MonthComponent"); }
        }

        /// <summary>
        ///   The year element of the calendar date.
        /// </summary>
        [IfcAttribute(3, IfcAttributeState.Mandatory)]
        public IfcYearNumber YearComponent
        {
            get
            {
                ((IPersistIfcEntity) this).Activate(false);
                return _yearComponent;
            }
            set { this.SetModelValue(this, ref _yearComponent, value, v => YearComponent = v, "YearComponent"); }
        }

        #region INotifyPropertyChanged Members

        [field: NonSerialized] //don't serialize events
            private event PropertyChangedEventHandler PropertyChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { PropertyChanged += value; }
            remove { PropertyChanged -= value; }
        }

        void ISupportChangeNotification.NotifyPropertyChanging(string propertyName)
        {
            PropertyChangingEventHandler handler = PropertyChanging;
            if (handler != null)
            {
                handler(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        [field: NonSerialized] //don't serialize events
            private event PropertyChangingEventHandler PropertyChanging;

        event PropertyChangingEventHandler INotifyPropertyChanging.PropertyChanging
        {
            add { PropertyChanging += value; }
            remove { PropertyChanging -= value; }
        }

        #endregion

        #region ISupportChangeNotification Members

        void ISupportChangeNotification.NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region ISupportIfcParser Members

        public void IfcParse(int propIndex, IPropertyValue value)
        {
            switch (propIndex)
            {
                case 0:
                    _dayComponent = (int) value.IntegerVal;
                    break;
                case 1:
                    _monthComponent = (int) value.IntegerVal;
                    break;
                case 2:
                    _yearComponent = (int) value.IntegerVal;
                    break;

                default:
                    this.HandleUnexpectedAttribute(propIndex, value); break;
            }
        }

        #endregion

        public string ValidCalendarDate()
        {
            try
            {
                DateTime dt = new DateTime(_yearComponent, _monthComponent, _dayComponent);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return string.Format("WR21 CalendarDate : The {0} parameter with value {1} is invalid.", ex.ParamName,
                                     ex.ActualValue);
            }
            return "";
        }

        #region ISupportIfcParser Members

        public string WhereRule()
        {
            return ValidCalendarDate();
        }

        #endregion
    }
}