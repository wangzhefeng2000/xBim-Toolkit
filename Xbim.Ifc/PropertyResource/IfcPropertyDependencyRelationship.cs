﻿#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcPropertyDependencyRelationship.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System;
using System.ComponentModel;
using Xbim.Ifc.MeasureResource;
using Xbim.XbimExtensions;
using Xbim.XbimExtensions.Parser;

#endregion

namespace Xbim.Ifc.PropertyResource
{
    /// <summary>
    ///   An IfcPropertyDependencyRelationship describes an identified dependency between the value of one property and that of another.
    /// </summary>
    /// <remarks>
    ///   Definition from IAI: An IfcPropertyDependencyRelationship describes an identified dependency between the value of one property and that of another.
    ///   HISTORY: New entity in Release IFC2x Edition 2
    ///   Use Definition
    ///   Whilst the IfcPropertyDependencyRelationship may be used to describe the dependency, and it may do so in terms of the expression of how the dependency operates, it is not possible through the current IFC model for the value of the related property to be actually derived from the value of the relating property. The determination of value according to the dependency is required to be performed by an application that can then use the Expression attribute to flag the form of the dependency.
    ///   Formal Propositions:
    ///   WR1   :   The DependingProperty shall not point to the same instance as the DependantProperty
    /// </remarks>
    [IfcPersistedEntity, Serializable]
    public class IfcPropertyDependencyRelationship : ISupportChangeNotification, INotifyPropertyChanged,
                                                     IPersistIfcEntity, INotifyPropertyChanging
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

        private IfcProperty _dependingProperty;
        private IfcProperty _dependantProperty;
        private IfcLabel? _name;
        private IfcText? _description;
        private IfcText? _expression;

        #endregion

        /// <summary>
        ///   The property on which the relationship depends.
        /// </summary>
        [IfcAttribute(1, IfcAttributeState.Mandatory)]
        public IfcProperty DependingProperty
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _dependingProperty;
            }
            set
            {
                ModelManager.SetModelValue(this, ref _dependingProperty, value, v => DependingProperty = v,
                                           "DependingProperty");
            }
        }

        /// <summary>
        ///   The dependant property
        /// </summary>
        [IfcAttribute(2, IfcAttributeState.Mandatory)]
        public IfcProperty DependantProperty
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _dependantProperty;
            }
            set
            {
                ModelManager.SetModelValue(this, ref _dependantProperty, value, v => DependantProperty = v,
                                           "DependantProperty");
            }
        }


        /// <summary>
        ///   A name used to identify or qualify the applied value relationship.
        /// </summary>
        [IfcAttribute(3, IfcAttributeState.Optional)]
        public IfcLabel? Name
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _name;
            }
            set { ModelManager.SetModelValue(this, ref _name, value, v => Name = v, "Name"); }
        }

        /// <summary>
        ///   A description that may apply additional information about an applied value relationship.
        /// </summary>
        [IfcAttribute(4, IfcAttributeState.Optional)]
        public IfcText? Description
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _description;
            }
            set { ModelManager.SetModelValue(this, ref _description, value, v => Description = v, "Description"); }
        }

        /// <summary>
        ///   The arithmetic operator applied in an applied value relationship.
        /// </summary>
        [IfcAttribute(5, IfcAttributeState.Mandatory)]
        public IfcText? Expression
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _expression;
            }
            set { ModelManager.SetModelValue(this, ref _expression, value, v => Expression = v, "Expression"); }
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

        #region IPersistIfc Members

        public virtual void IfcParse(int propIndex, IPropertyValue value)
        {
            switch (propIndex)
            {
                case 0:
                    _dependingProperty = (IfcProperty) value.EntityVal;
                    break;
                case 1:
                    _dependantProperty = (IfcProperty) value.EntityVal;
                    break;
                case 2:
                    _name = value.StringVal;
                    break;
                case 3:
                    _description = value.StringVal;
                    break;
                case 4:
                    _expression = value.StringVal;
                    break;
                default:
                    this.HandleUnexpectedAttribute(propIndex, value); break;
            }
        }

        public string WhereRule()
        {
            if (_dependantProperty == _dependingProperty)
                return
                    "WR1 PropertyDependencyRelationship : The DependingProperty shall not point to the same instance as the DependantProperty\n";
            else
                return "";
        }

        #endregion
    }
}