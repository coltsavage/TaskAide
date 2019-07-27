﻿using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace zCompany.UiAutomation
{
    public class UiChart : UiElement
    {
        // Class Fields
        public static string ClassName = "IntervalChart";

        // Fields
        private VolatileList<UiInterval> intervals;
        private VolatileState<UiElement> originLabel;

        // Constructors
        public UiChart(IUiElement element)
            :base(element)
        {
            this.intervals = new VolatileList<UiInterval>(
                () => base.FindAll(By.ClassName(UiInterval.ClassName)),
                (item) => new UiInterval(item, this));

            this.originLabel = new VolatileState<UiElement>(
                () => new UiElement(base.Find(By.ClassName("TimeAxis")).Find("MajorTick#0")));
        }

        // Destructors
        public override void Dispose()
        {
            base.Dispose();
        }

        // Properties
        public IReadOnlyCollection<UiInterval> Intervals { get => this.intervals.Value; }

        public UiElement OriginLabel { get => this.originLabel.Value; }
               
        // Methods
        public new UiChart Refresh()
        {
            base.Refresh();
            this.intervals.Invalidate();
            this.originLabel.Invalidate();
            return this;
        }
    }
}