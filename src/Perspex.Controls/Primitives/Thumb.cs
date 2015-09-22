﻿// Copyright (c) The Perspex Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Perspex.Input;
using Perspex.Interactivity;
using Perspex.Layout;

namespace Perspex.Controls.Primitives
{
    public class Thumb : TemplatedControl
    {
        public static readonly RoutedEvent<VectorEventArgs> DragStartedEvent =
            RoutedEvent.Register<Thumb, VectorEventArgs>("DragStarted", RoutingStrategies.Bubble);

        public static readonly RoutedEvent<VectorEventArgs> DragDeltaEvent =
            RoutedEvent.Register<Thumb, VectorEventArgs>("DragDelta", RoutingStrategies.Bubble);

        public static readonly RoutedEvent<VectorEventArgs> DragCompletedEvent =
            RoutedEvent.Register<Thumb, VectorEventArgs>("DragCompleted", RoutingStrategies.Bubble);

        private Point? _lastPoint;

        static Thumb()
        {
            DragStartedEvent.AddClassHandler<Thumb>(x => x.OnDragStarted, RoutingStrategies.Bubble);
            DragDeltaEvent.AddClassHandler<Thumb>(x => x.OnDragDelta, RoutingStrategies.Bubble);
            DragCompletedEvent.AddClassHandler<Thumb>(x => x.OnDragCompleted, RoutingStrategies.Bubble);
        }

        public event EventHandler<VectorEventArgs> DragStarted
        {
            add { AddHandler(DragStartedEvent, value); }
            remove { RemoveHandler(DragStartedEvent, value); }
        }

        public event EventHandler<VectorEventArgs> DragDelta
        {
            add { AddHandler(DragDeltaEvent, value); }
            remove { RemoveHandler(DragDeltaEvent, value); }
        }

        public event EventHandler<VectorEventArgs> DragCompleted
        {
            add { AddHandler(DragCompletedEvent, value); }
            remove { RemoveHandler(DragCompletedEvent, value); }
        }

        protected virtual void OnDragStarted(VectorEventArgs e)
        {
        }

        protected virtual void OnDragDelta(VectorEventArgs e)
        {
        }

        protected virtual void OnDragCompleted(VectorEventArgs e)
        {
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            if (_lastPoint.HasValue)
            {
                var ev = new VectorEventArgs
                {
                    RoutedEvent = DragDeltaEvent,
                    Vector = e.GetPosition(this) - _lastPoint.Value,
                };

                RaiseEvent(ev);
            }
        }

        protected override void OnPointerPressed(PointerPressEventArgs e)
        {
            e.Device.Capture(this);
            _lastPoint = e.GetPosition(this);

            var ev = new VectorEventArgs
            {
                RoutedEvent = DragStartedEvent,
                Vector = (Vector)_lastPoint,
            };

            RaiseEvent(ev);
        }

        protected override void OnPointerReleased(PointerEventArgs e)
        {
            if (_lastPoint.HasValue)
            {
                e.Device.Capture(null);
                _lastPoint = null;

                var ev = new VectorEventArgs
                {
                    RoutedEvent = DragCompletedEvent,
                    Vector = (Vector)e.GetPosition(this),
                };

                RaiseEvent(ev);
            }
        }
    }
}
