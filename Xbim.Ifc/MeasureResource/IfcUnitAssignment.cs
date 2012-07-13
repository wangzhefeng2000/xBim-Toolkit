﻿#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcUnitAssignment.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Xbim.Ifc2x3.SelectTypes;
using Xbim.XbimExtensions;
using Xbim.XbimExtensions.Interfaces;

#endregion

namespace Xbim.Ifc2x3.MeasureResource
{
    [IfcPersistedEntityAttribute, Serializable]
    public class IfcUnitAssignment : INotifyPropertyChanged, ISupportChangeNotification, IPersistIfcEntity,
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

        public IfcUnitAssignment()
        {
            _units = new UnitSet(this);
        }

        #region Fields

        private UnitSet _units;

        #endregion

        #region Part 21 Step file Parse routines

        [IfcAttribute(1, IfcAttributeState.Mandatory, IfcAttributeType.Set, 1)]
        public UnitSet Units
        {
            get
            {
                ((IPersistIfcEntity) this).Activate(false);
                return _units;
            }
            set { this.SetModelValue(this, ref _units, value, v => Units = v, "Units"); }
        }


        public virtual void IfcParse(int propIndex, IPropertyValue value)
        {
            if (propIndex == 0)
            {
                _units.Add((IfcUnit) value.EntityVal);
            }
            else
                this.HandleUnexpectedAttribute(propIndex, value);
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
            string err = "";
            IEnumerable<IfcNamedUnit> namedUnits = _units.Where<IfcNamedUnit>(u => u.UnitType != IfcUnitEnum.USERDEFINED);
            IEnumerable<IfcDerivedUnit> derivedUnits =
                _units.Where<IfcDerivedUnit>(u => u.UnitType != IfcDerivedUnitEnum.USERDEFINED);
            IEnumerable<IfcMonetaryUnit> monetaryUnits = _units.OfType<IfcMonetaryUnit>();
            if (monetaryUnits.Count() > 1)
                err += "Only one Monetary Unit is allowed\n";
            HashSet<string> derivedUnitNames = new HashSet<string>();
            HashSet<string> namedUnitsNames = new HashSet<string>();
            foreach (IfcNamedUnit item in namedUnits)
            {
                if (namedUnitsNames.Contains(item.UnitType.ToString()))
                {
                    err += "Two named units of the same type are not allowed in UnitAssignment\n";
                }
                else
                    namedUnitsNames.Add(item.UnitType.ToString());
            }

            foreach (IfcDerivedUnit item in derivedUnits)
            {
                if (derivedUnitNames.Contains(item.UnitType.ToString()))
                {
                    err += "Two derived units of the same type are not allowed in UnitAssignment\n";
                }
                else
                    derivedUnitNames.Add(item.UnitType.ToString());
            }
            return err;
        }

        #endregion
    }

    #region Converter

    public class UnitAssignmentConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof (string))
                return true;
            else
                return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string str = value as string;
            if (str != null)
            {
                if (str == "SI")
                {
                    IfcUnitAssignment ua = new IfcUnitAssignment();
                    ua.Units.Add_Reversible(new IfcSIUnit
                                                {
                                                    UnitType = IfcUnitEnum.LENGTHUNIT,
                                                    Name = IfcSIUnitName.METRE,
                                                    Prefix = IfcSIPrefix.MILLI
                                                });
                    ua.Units.Add_Reversible(new IfcSIUnit
                                                {UnitType = IfcUnitEnum.AREAUNIT, Name = IfcSIUnitName.SQUARE_METRE});
                    ua.Units.Add_Reversible(new IfcSIUnit
                                                {UnitType = IfcUnitEnum.VOLUMEUNIT, Name = IfcSIUnitName.CUBIC_METRE});
                    ua.Units.Add_Reversible(new IfcSIUnit
                                                {UnitType = IfcUnitEnum.SOLIDANGLEUNIT, Name = IfcSIUnitName.STERADIAN});
                    ua.Units.Add_Reversible(new IfcSIUnit
                                                {UnitType = IfcUnitEnum.PLANEANGLEUNIT, Name = IfcSIUnitName.RADIAN});
                    ua.Units.Add_Reversible(new IfcSIUnit {UnitType = IfcUnitEnum.MASSUNIT, Name = IfcSIUnitName.GRAM});
                    ua.Units.Add_Reversible(new IfcSIUnit {UnitType = IfcUnitEnum.TIMEUNIT, Name = IfcSIUnitName.SECOND});
                    ua.Units.Add_Reversible(new IfcSIUnit
                                                {
                                                    UnitType = IfcUnitEnum.THERMODYNAMICTEMPERATUREUNIT,
                                                    Name = IfcSIUnitName.DEGREE_CELSIUS
                                                });
                    ua.Units.Add_Reversible(new IfcSIUnit
                                                {
                                                    UnitType = IfcUnitEnum.LUMINOUSINTENSITYUNIT,
                                                    Name = IfcSIUnitName.LUMEN
                                                });
                    return ua;
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    #endregion
}