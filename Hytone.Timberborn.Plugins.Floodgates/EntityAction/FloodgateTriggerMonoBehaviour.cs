﻿using Bindito.Core;
using Hytone.Timberborn.Plugins.Floodgates.Schedule;
using Timberborn.ConstructibleSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Timberborn.Persistence;
using Timberborn.WaterBuildings;
using Timberborn.WeatherSystem;
using UnityEngine;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction
{
    /// <summary>
    /// This class handles the data related to Floodgate Triggers. It also holds the actions
    /// which are executed when certain events happen.
    /// </summary>
    public class FloodgateTriggerMonoBehaviour : MonoBehaviour, IPersistentEntity, IFinishedStateListener
    {
        //Keys used in data saving/loading
        private static readonly ComponentKey FloodgateTriggerKey = new ComponentKey(nameof(FloodgateTriggerMonoBehaviour));
        private static readonly PropertyKey<bool> DroughtEndedEnabledKey = new PropertyKey<bool>(nameof(DroughtEndedEnabled));
        private static readonly PropertyKey<float> DroughtEndedHeightKey = new PropertyKey<float>(nameof(DroughtEndedHeight));
        private static readonly PropertyKey<bool> DroughtStartedEnabledKey = new PropertyKey<bool>(nameof(DroughtStartedEnabled));
        private static readonly PropertyKey<float> DroughtStartedHeightKey = new PropertyKey<float>(nameof(DroughtStartedHeight));
        private static readonly PropertyKey<float> FirstScheduleTimeKey = new PropertyKey<float>(nameof(FirstScheduleTime));
        private static readonly PropertyKey<float> FirstScheduleHeightKey = new PropertyKey<float>(nameof(FirstScheduleHeight));
        private static readonly PropertyKey<float> SecondScheduleTimeKey = new PropertyKey<float>(nameof(SecondScheduleTime));
        private static readonly PropertyKey<float> SecondScheduleHeightKey = new PropertyKey<float>(nameof(SecondScheduleHeight));
        private static readonly PropertyKey<bool> ScheduleEnabledKey = new PropertyKey<bool>(nameof(ScheduleEnabled));
        private static readonly PropertyKey<bool> DisableScheduleOnDroughtKey = new PropertyKey<bool>(nameof(DisableScheduleOnDrought));
        private static readonly PropertyKey<bool> DisableScheduleOnTemperateKey = new PropertyKey<bool>(nameof(DisableScheduleOnTemperateKey));

        private static readonly ListKey<StreamGaugeFloodgateLink> FloodgateLinksKey = new ListKey<StreamGaugeFloodgateLink>(nameof(FloodgateLinks));

        private IScheduleTriggerFactory _scheduleTriggerFactory;
        private IScheduleTrigger _scheduleTrigger;
        private DroughtService _droughtServíce;
        private StreamGaugeFloodgateLinkSerializer _linkSerializer;

        private readonly List<StreamGaugeFloodgateLink> _floodgateLinks = new List<StreamGaugeFloodgateLink>();
        public ReadOnlyCollection<StreamGaugeFloodgateLink> FloodgateLinks { get; private set; }

        public bool DroughtEndedEnabled { get; set; }
        public float DroughtEndedHeight { get; set; }
        public bool DroughtStartedEnabled { get; set; }
        public float DroughtStartedHeight { get; set; }

        public bool ScheduleEnabled { get; set; }
        public bool DisableScheduleOnDrought { get; set; }
        public bool DisableScheduleOnTemperate { get; set; }
        public float FirstScheduleTime { get; set; }
        public float FirstScheduleHeight { get; set; }
        public float SecondScheduleTime { get; set; }
        public float SecondScheduleHeight { get; set; }

        public int MaxStreamGaugeLinks = 1;

        [Inject]
        public void InjectDependencies(
            IScheduleTriggerFactory scheduleTriggerFactory,
            DroughtService droughtService,
            StreamGaugeFloodgateLinkSerializer linkSerializer)
        {
            _scheduleTriggerFactory = scheduleTriggerFactory;
            _droughtServíce = droughtService;
            _linkSerializer = linkSerializer;
        }

        public void Awake()
        {
            FloodgateLinks = _floodgateLinks.AsReadOnly();
            FirstScheduleTime = 0;
            SecondScheduleTime = 0;
        }

        public void OnEnterFinishedState()
        {
            _scheduleTrigger = _scheduleTriggerFactory.Create(SetFirstScheduleHeight, SetSecondScheduleHeight, FirstScheduleTime, SecondScheduleTime);
            if (ScheduleEnabled)
            {
                _scheduleTrigger.Enable();
            }
        }

        public void OnExitFinishedState()
        {
            _scheduleTrigger?.Disable();
            DetachAllLinks();
        }

        /// <summary>
        /// Save the data so we can load it later
        /// </summary>
        /// <param name="entitySaver"></param>
        public void Save(IEntitySaver entitySaver)
        {
            IObjectSaver component = entitySaver.GetComponent(FloodgateTriggerKey);
            component.Set(DroughtEndedEnabledKey, DroughtEndedEnabled);
            component.Set(DroughtEndedHeightKey, DroughtEndedHeight);
            component.Set(DroughtStartedEnabledKey, DroughtStartedEnabled);
            component.Set(DroughtStartedHeightKey, DroughtStartedHeight);
            component.Set(FirstScheduleTimeKey, FirstScheduleTime);
            component.Set(FirstScheduleHeightKey, FirstScheduleHeight);
            component.Set(SecondScheduleTimeKey, SecondScheduleTime);
            component.Set(SecondScheduleHeightKey, SecondScheduleHeight);
            component.Set(ScheduleEnabledKey, ScheduleEnabled);
            component.Set(DisableScheduleOnDroughtKey, DisableScheduleOnDrought);
            component.Set(DisableScheduleOnTemperateKey, DisableScheduleOnTemperate);
            component.Set(FloodgateLinksKey, FloodgateLinks, _linkSerializer);
        }

        /// <summary>
        /// Load saved data if it exists
        /// </summary>
        /// <param name="entityLoader"></param>
        public void Load(IEntityLoader entityLoader)
        {
            if (!entityLoader.HasComponent(FloodgateTriggerKey))
            {
                return;
            }
            IObjectLoader component = entityLoader.GetComponent(FloodgateTriggerKey);
            if (component.Has(DroughtEndedEnabledKey))
            {
                DroughtEndedEnabled = component.Get(DroughtEndedEnabledKey);
            }
            if (component.Has(DroughtEndedHeightKey))
            {
                DroughtEndedHeight = component.Get(DroughtEndedHeightKey);
            }
            if (component.Has(DroughtStartedEnabledKey))
            {
                DroughtStartedEnabled = component.Get(DroughtStartedEnabledKey);
            }
            if (component.Has(DroughtStartedHeightKey))
            {
                DroughtStartedHeight = component.Get(DroughtStartedHeightKey);
            }
            if (component.Has(FirstScheduleTimeKey))
            {
                FirstScheduleTime = component.Get(FirstScheduleTimeKey);
            }
            if (component.Has(FirstScheduleHeightKey))
            {
                FirstScheduleHeight = component.Get(FirstScheduleHeightKey);
            }
            if (component.Has(SecondScheduleTimeKey))
            {
                SecondScheduleTime = component.Get(SecondScheduleTimeKey);
            }
            if (component.Has(SecondScheduleHeightKey))
            {
                SecondScheduleHeight = component.Get(SecondScheduleHeightKey);
            }
            if (component.Has(ScheduleEnabledKey))
            {
                ScheduleEnabled = component.Get(ScheduleEnabledKey);
            }
            if (component.Has(DisableScheduleOnDroughtKey))
            {
                DisableScheduleOnDrought = component.Get(DisableScheduleOnDroughtKey);
            }
            if (component.Has(DisableScheduleOnTemperateKey))
            {
                DisableScheduleOnTemperate = component.Get(DisableScheduleOnTemperateKey);
            }
            if (component.Has(FloodgateLinksKey))
            {
                _floodgateLinks.AddRange(component.Get(FloodgateLinksKey, _linkSerializer));
                //_floodgateLinks.AddRange(component.Get(FloodgateLinksKey));

                foreach (var link in FloodgateLinks)
                {
                    PostAttachLink(link);
                }
            }
        }

        /// <summary>
        /// The stuff to do to FLoodgates when a Drought starts
        /// </summary>
        public void OnDroughtStarted()
        {
            var floodgate = GetComponent<Floodgate>();
            if (DroughtStartedEnabled == true &&
               floodgate.Height != DroughtStartedHeight)
            {
                floodgate.SetHeightAndSynchronize(DroughtStartedHeight);
            }
            if (ScheduleEnabled && !DisableScheduleOnDrought)
            {
                _scheduleTrigger.Enable();
            }
            else if (DisableScheduleOnDrought)
            {
                _scheduleTrigger.Disable();
            }
        }

        /// <summary>
        /// The stuff to do to Floodgates when a Drought ends
        /// </summary>
        public void OnDroughtEnded()
        {
            var floodgate = GetComponent<Floodgate>();
            if (DroughtEndedEnabled == true &&
               floodgate.Height != DroughtEndedHeight)
            {
                floodgate.SetHeightAndSynchronize(DroughtEndedHeight);
            }
            if (ScheduleEnabled && !DisableScheduleOnTemperate)
            {
                _scheduleTrigger.Enable();
            }
            else if (DisableScheduleOnTemperate)
            {
                _scheduleTrigger.Disable();
            }
        }

        /// <summary>
        /// When toggles related to Schedules are changed,
        /// then check if triggers need to be enabled/disabled
        /// </summary>
        public void OnChangedScheduleToggles()
        {
            if (!ScheduleEnabled)
            {
                _scheduleTrigger.Disable();
                return;
            }
            if (_droughtServíce.IsDrought)
            {
                if(DisableScheduleOnDrought)
                {
                    _scheduleTrigger.Disable();
                    return;
                }
                _scheduleTrigger.Enable();
                return;
            }
            if(!_droughtServíce.IsDrought)
            {
                if (DisableScheduleOnTemperate)
                {
                    _scheduleTrigger.Disable();
                    return;
                }
                _scheduleTrigger.Enable();
                return;
            }
        }

        /// <summary>
        /// When schedule times or heights are changed, remove old trigger 
        /// and create new. Enabled new trigger is old was enabled
        /// </summary>
        public void ChangeScheduleValues()
        {
            bool wasEnabled = _scheduleTrigger?.Enabled ?? false;
            if (_scheduleTrigger != null)
            {
                _scheduleTrigger.Disable();
            }
            _scheduleTrigger = _scheduleTriggerFactory.Create(SetFirstScheduleHeight,
                                                              SetSecondScheduleHeight,
                                                              FirstScheduleTime,
                                                              SecondScheduleTime);
            if (wasEnabled)
            {
                _scheduleTrigger.Enable();
            }
        }

        /// <summary>
        /// Set the height of floodgate to the configured height
        /// </summary>
        public void SetFirstScheduleHeight()
        {
            var floodgate = GetComponent<Floodgate>();
            if (ScheduleEnabled == true &&
                floodgate.Height != FirstScheduleHeight)
            {
                floodgate.SetHeightAndSynchronize(FirstScheduleHeight);
            }
        }

        /// <summary>
        /// Set the height of floodgate to the configured height
        /// </summary>
        public void SetSecondScheduleHeight()
        {
            var floodgate = GetComponent<Floodgate>();
            if (ScheduleEnabled == true &&
                floodgate.Height != SecondScheduleHeight)
            {
                floodgate.SetHeightAndSynchronize(SecondScheduleHeight);
            }
        }

        /// <summary>
        /// Creates a link between a Floodgate and Streamgauge
        /// </summary>
        /// <param name="floodgate"></param>
        /// <param name="streamGauge"></param>
        public void AttachLink(FloodgateTriggerMonoBehaviour floodgate,
                               StreamGaugeMonoBehaviour streamGauge)
        {

            var link = new StreamGaugeFloodgateLink(floodgate, streamGauge);
            _floodgateLinks.Add(link);
            PostAttachLink(link);
        }

        /// <summary>
        /// Do the linking at the streamgauge's end too
        /// </summary>
        /// <param name="link"></param>
        public void PostAttachLink(StreamGaugeFloodgateLink link)
        {
            link.StreamGauge.AttachFloodgate(link);
        }

        /// <summary>
        /// Deletes all existing links between this floodgate and stream gauges
        /// </summary>
        public void DetachAllLinks()
        {
            foreach (var link in _floodgateLinks)
            {
                //_floodgateLinks.Remove(link);
                PostDetachLink(link);
            }
            _floodgateLinks.Clear();
        }

        /// <summary>
        /// Deletes a link between a Floodgate and Streamgauge
        /// </summary>
        /// <param name="link"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void DetachLink(StreamGaugeFloodgateLink link)
        {
            if (!_floodgateLinks.Remove(link))
            {
                throw new InvalidOperationException($"Coudln't remove {link} from {this}, it wasn't added.");
            }
            PostDetachLink(link);
        }

        /// <summary>
        /// Remvoes the link from Streamgauge's end too
        /// </summary>
        /// <param name="link"></param>
        private void PostDetachLink(StreamGaugeFloodgateLink link)
        {
            link.StreamGauge.DetachFloodgate(link);
        }
    }
}
