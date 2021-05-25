using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace A.WPF
{
    public static class EventToCommandBind
    {
        public static void Bind(this FrameworkElement element, string eventName, ICommand command, List<object> parameters = null, bool eHandle = false)
        {

            if (parameters == null) parameters = new List<object>();

            var einfo = element.GetType().GetEvent(eventName).EventHandlerType.Name;

            switch (einfo)
            {


                case "RoutedEventHandler":
                    var eventHandler1 = new RoutedEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters, s)); });
                    element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler1);
                    break;
                case "MouseButtonEventHandler":
                    var eventHandler2 = new MouseButtonEventHandler((s, e) =>
                    {
                        if (command.CanExecute(s)) command.Execute(MakeParameter(parameters, s, e));
                        e.Handled = eHandle;
                    });
                    element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler2);
                    break;

                case "DependencyPropertyChangedEventHandler":
                    var eventHandler4 = new DependencyPropertyChangedEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters, s)); });
                    element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler4);
                    break;
                case "RequestBringIntoViewEventHandler":
                    var eventHandler5 = new RequestBringIntoViewEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters, s)); });
                    element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler5);
                    break;
                case "SizeChangedEventHandler":
                    var eventHandler6 = new SizeChangedEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters, s)); });
                    element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler6);
                    break;
                case "EventHandler":
                    var eventHandler7 = new EventHandler((s, e) => { if (command!=null && command.CanExecute(s)) command.Execute(MakeParameter(parameters, s)); });
                    element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler7);
                    break;
                case "ToolTipEventHandler":
                    var eventHandler8 = new ToolTipEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters, s)); });
                    element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler8);
                    break;
                case "ContextMenuEventHandler":
                    var eventHandler9 = new ContextMenuEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters, s)); });
                    element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler9);
                    break;
                case "MouseEventHandler":
                    var eventHandler10 = new MouseEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters, s)); });
                    element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler10);
                    break;
                case "MouseWheelEventHandler":
                    var eventHandler11 = new MouseWheelEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters, s)); });
                    element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler11);
                    break;
                case "QueryCursorEventHandler":
                    var eventHandler12 = new QueryCursorEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters, s)); });
                    element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler12);
                    break;
                case "StylusDownEventHandler":
                    var eventHandler13 = new StylusDownEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters, s)); });
                    element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler13);
                    break;
                case "StylusEventHandler":
                    var eventHandler14 = new StylusEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters, s)); });
                    element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler14);
                    break;
                case "StylusSystemGestureEventHandler":
                    var eventHandler15 = new StylusSystemGestureEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters, s)); });
                    element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler15);
                    break;
                case "StylusButtonEventHandler":
                    var eventHandler16 = new StylusButtonEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters, s)); });
                    element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler16);
                    break;
                case "KeyEventHandler":
                    var eventHandler17 = new KeyEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters, s)); });
                    element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler17);
                    break;
                case "KeyboardFocusChangedEventHandler":
                    var eventHandler18 = new KeyboardFocusChangedEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters, s)); });
                    element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler18);
                    break;
                case "TextCompositionEventHandler":
                    var eventHandler19 = new TextCompositionEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters, s)); });
                    element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler19);
                    break;
                case "QueryContinueDragEventHandler":
                    var eventHandler20 = new QueryContinueDragEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters, s)); });
                    element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler20);
                    break;
                case "GiveFeedbackEventHandler":
                    var eventHandler21 = new GiveFeedbackEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters, s)); });
                    element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler21);
                    break;
                case "DragEventHandler":
                    var eventHandler22 = new DragEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters, s)); });
                    element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler22);
                    break;
                default:
                    MessageBox.Show(einfo);
                    break;
            }

        }


        //public static void Bind(FrameworkElement element, string eventName, ICommand command,List<object> parameters=null, bool eHandle = false)
        //{

        //    if(parameters==null) parameters=new List<object>();

        //    var einfo = element.GetType().GetEvent(eventName).EventHandlerType.Name;

        //    switch (einfo)
        //    {


        //        case "RoutedEventHandler":
        //            var eventHandler1 = new RoutedEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters,s)); });
        //            element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler1);
        //            break;
        //        case "MouseButtonEventHandler":
        //            var eventHandler2 = new MouseButtonEventHandler((s, e) =>
        //            {
        //                if (command.CanExecute(s)) command.Execute(MakeParameter(parameters, s,e));
        //                e.Handled = eHandle;
        //            });
        //            element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler2);
        //            break;

        //        case "DependencyPropertyChangedEventHandler":
        //            var eventHandler4 = new DependencyPropertyChangedEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters,s)); });
        //            element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler4);
        //            break;
        //        case "RequestBringIntoViewEventHandler":
        //            var eventHandler5 = new RequestBringIntoViewEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters,s)); });
        //            element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler5);
        //            break;
        //        case "SizeChangedEventHandler":
        //            var eventHandler6 = new SizeChangedEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters,s)); });
        //            element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler6);
        //            break;
        //        case "EventHandler":
        //            var eventHandler7 = new EventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters,s)); });
        //            element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler7);
        //            break;
        //        case "ToolTipEventHandler":
        //            var eventHandler8 = new ToolTipEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters,s)); });
        //            element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler8);
        //            break;
        //        case "ContextMenuEventHandler":
        //            var eventHandler9 = new ContextMenuEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters,s)); });
        //            element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler9);
        //            break;
        //        case "MouseEventHandler":
        //            var eventHandler10 = new MouseEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters,s)); });
        //            element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler10);
        //            break;
        //        case "MouseWheelEventHandler":
        //            var eventHandler11 = new MouseWheelEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters,s)); });
        //            element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler11);
        //            break;
        //        case "QueryCursorEventHandler":
        //            var eventHandler12 = new QueryCursorEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters,s)); });
        //            element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler12);
        //            break;
        //        case "StylusDownEventHandler":
        //            var eventHandler13 = new StylusDownEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters,s)); });
        //            element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler13);
        //            break;
        //        case "StylusEventHandler":
        //            var eventHandler14 = new StylusEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters,s)); });
        //            element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler14);
        //            break;
        //        case "StylusSystemGestureEventHandler":
        //            var eventHandler15 = new StylusSystemGestureEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters,s)); });
        //            element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler15);
        //            break;
        //        case "StylusButtonEventHandler":
        //            var eventHandler16 = new StylusButtonEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters,s)); });
        //            element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler16);
        //            break;
        //        case "KeyEventHandler":
        //            var eventHandler17 = new KeyEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters,s)); });
        //            element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler17);
        //            break;
        //        case "KeyboardFocusChangedEventHandler":
        //            var eventHandler18 = new KeyboardFocusChangedEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters,s)); });
        //            element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler18);
        //            break;
        //        case "TextCompositionEventHandler":
        //            var eventHandler19 = new TextCompositionEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters,s)); });
        //            element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler19);
        //            break;
        //        case "QueryContinueDragEventHandler":
        //            var eventHandler20 = new QueryContinueDragEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters,s)); });
        //            element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler20);
        //            break;
        //        case "GiveFeedbackEventHandler":
        //            var eventHandler21 = new GiveFeedbackEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters,s)); });
        //            element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler21);
        //            break;
        //        case "DragEventHandler":
        //            var eventHandler22 = new DragEventHandler((s, e) => { if (command.CanExecute(s)) command.Execute(MakeParameter(parameters,s)); });
        //            element.GetType().GetEvent(eventName).AddEventHandler(element, eventHandler22);
        //            break;
        //        default:
        //            MessageBox.Show(einfo);
        //            break;
        //    }

        //}

        public delegate void CalculationHandler(object sender);

        static object MakeParameter(List<object> parametr,object sender,object arg=null)
        {
            parametr.Add(sender);
            if(arg!=null) parametr.Add(arg);
            return parametr;
        }

    }

     

}
