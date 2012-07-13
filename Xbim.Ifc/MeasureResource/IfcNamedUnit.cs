﻿#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcNamedUnit.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System;
using System.ComponentModel;
using Xbim.Ifc2x3.SelectTypes;
using Xbim.XbimExtensions;
using Xbim.XbimExtensions.Interfaces;

#endregion

namespace Xbim.Ifc2x3.MeasureResource
{
    [IfcPersistedEntityAttribute, Serializable]
    public abstract class IfcNamedUnit : INotifyPropertyChanged, ISupportChangeNotification, IPersistIfcEntity, IfcUnit,
                                         INotifyPropertyChanging
    {

        #region IPersistIfcEntity Members

        private long _entityLabel;
        private IModel _model;

        public IModel ModelOf
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

        #region Fields

        private IfcDimensionalExponents _dimensions;
        private IfcUnitEnum _unitType;

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return _unitType.GetHashCode();
        }

        #endregion

        #region Part 21 Step file Parse routines

        /// <summary>
        ///   The dimensional exponents of the SI base units by which the named unit is defined.
        /// </summary>
        [IfcAttribute(1, IfcAttributeState.Mandatory)]
        public virtual IfcDimensionalExponents Dimensions
        {
            get
            {
                ((IPersistIfcEntity) this).Activate(false);
                return _dimensions;
            }
            set { this.SetModelValue(this, ref _dimensions, value, v => Dimensions = v, "Dimensions"); }
        }

        /// <summary>
        ///   The type of the unit
        /// </summary>
        [IfcAttribute(2, IfcAttributeState.Mandatory)]
        public IfcUnitEnum UnitType
        {
            get
            {
                ((IPersistIfcEntity) this).Activate(false);
                return _unitType;
            }
            set { this.SetModelValue(this, ref _unitType, value, v => UnitType = v, "UnitType"); }
        }

       

        public virtual void IfcParse(int propIndex, IPropertyValue value)
        {
            switch (propIndex)
            {
                case 0:
                    _dimensions = (IfcDimensionalExponents) value.EntityVal;
                    break;
                case 1:
                    _unitType = (IfcUnitEnum) Enum.Parse(typeof (IfcUnitEnum), value.EnumVal, true);
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

        #region ISupportIfcParser Members

        public virtual string WhereRule()
        {
            if (!IfcDimensionalExponents.CorrectDimensions(_unitType, Dimensions))
                return string.Format("WR1 NamedUnit : The dimensions of the named unit {0} are not correct\n",
                                     _unitType.ToString());
            else
                return "";
        }

        #endregion
    }
}