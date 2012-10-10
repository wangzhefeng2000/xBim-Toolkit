﻿#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcCoordinatedUniversalTimeOffset.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System;
using System.ComponentModel;
using Xbim.XbimExtensions;
using Xbim.XbimExtensions.Parser;

#endregion

namespace Xbim.Ifc.DateTimeResource
{
    [IfcPersistedEntity, Serializable]
    public class IfcCoordinatedUniversalTimeOffset : IPersistIfcEntity, INotifyPropertyChanged,
                                                     ISupportChangeNotification, INotifyPropertyChanging
    {
#if SupportActivation

        #region IPersistIfcEntity Members

        private long _entityLabel;
        private IModel _model;

        IModel IPersistIfcEntity.ModelOf
        {
            get { return _model; }
        }

        void IPersistIfcEntity.Bind(IModel model, long entityLabel)
        {
            _model = model;
            _entityLabel = entityLabel;
        }

        bool IPersistIfcEntity.Activated
        {
            get { return _entityLabel > 0; }
        }

        public long EntityLabel
        {
            get { return _entityLabel; }
        }

        void IPersistIfcEntity.Activate(bool write)
        {
            if (_model != null && _entityLabel <= 0) _entityLabel = _model.Activate(this, false);
            if (write) _model.Activate(this, write);
        }

        #endregion

#endif

        #region Fields

        private IfcHourInDay _hourOffset;
        private IfcMinuteInHour? _minuteOffset;
        private IfcAheadOrBehind _sense;

        #endregion

        /// <summary>
        ///   The number of hours by which local time is offset from coordinated universal time.
        /// </summary>
        [IfcAttribute(1, IfcAttributeState.Mandatory)]
        public IfcHourInDay HourOffset
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _hourOffset;
            }
            set { ModelManager.SetModelValue(this, ref _hourOffset, value, v => HourOffset = v, "HourOffset"); }
        }

        /// <summary>
        ///   The number of minutes by which local time is offset from coordinated universal time.
        /// </summary>
        [IfcAttribute(2, IfcAttributeState.Optional)]
        public IfcMinuteInHour? MinuteOffset
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _minuteOffset;
            }
            set { ModelManager.SetModelValue(this, ref _minuteOffset, value, v => MinuteOffset = v, "MinuteOffset"); }
        }

        /// <summary>
        ///   The direction of the offset.
        /// </summary>
        [IfcAttribute(3, IfcAttributeState.Mandatory)]
        public IfcAheadOrBehind Sense
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _sense;
            }
            set { ModelManager.SetModelValue(this, ref _sense, value, v => Sense = v, "Sense"); }
        }

        #region ISupportIfcParser Members

        public void IfcParse(int propIndex, IPropertyValue value)
        {
            switch (propIndex)
            {
                case 0:
                    _hourOffset = (int) value.IntegerVal;
                    break;
                case 1:
                    _minuteOffset = (int) value.IntegerVal;
                    break;
                case 2:
                    _sense = (IfcAheadOrBehind) Enum.Parse(typeof (IfcAheadOrBehind), value.EnumVal, true);
                    break;
                default:
                    this.HandleUnexpectedAttribute(propIndex, value); break;
            }
        }

        #endregion

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

        public string WhereRule()
        {
            return "";
        }

        #endregion
    }
}